using AbstractBarModel;
using AbstractBarService.BindingModels;
using AbstractBarService.Interfaces;
using AbstractBarService.ViewModels;
using System;
using System.Collections.Generic;
using System.Data.Entity.SqlServer;
using System.Linq;
using System.Data.Entity;
using System.Text;
using System.Threading.Tasks;

namespace AbstractBarService.ImplementationsBD
{
    public class MainServiceBD : IMainService
    {
        private AbstractDbContext context;

        public MainServiceBD(AbstractDbContext context)
        {
            this.context = context;
        }

        public List<RequestViewModel> GetList()
        {
            List<RequestViewModel> result = context.Requests
                .Select(rec => new RequestViewModel
                {
                    Id = rec.Id,
                    CustomerId = rec.CustomerId,
                    CoctailId = rec.CoctailId,
                    BarmenId = rec.BarmenId,
                    DateCreate = SqlFunctions.DateName("dd", rec.DateCreate) + " " +
                                SqlFunctions.DateName("mm", rec.DateCreate) + " " +
                                SqlFunctions.DateName("yyyy", rec.DateCreate),
                    DateImplement = rec.DateImplement == null ? "" :
                                        SqlFunctions.DateName("dd", rec.DateImplement.Value) + " " +
                                        SqlFunctions.DateName("mm", rec.DateImplement.Value) + " " +
                                        SqlFunctions.DateName("yyyy", rec.DateImplement.Value),
                    Status = rec.Status.ToString(),
                    Count = rec.Count,
                    Sum = rec.Sum,
                    CustomerFIO = rec.Customer.CustomerFIO,
                    CoctailName = rec.Coctail.CoctailName,
                    BarmenName = rec.Barmen.BarmenFIO
                })
                .ToList();
            return result;
        }

        public void CreateRequest(RequestBindingModel model)
        {
            context.Requests.Add(new Request
            {
                CustomerId = model.CustomerId,
                CoctailId = model.CoctailId,
                DateCreate = DateTime.Now,
                Count = model.Count,
                Sum = model.Sum,
                Status = RequestStatus.Принят
            });
            context.SaveChanges();
        }

        public void TakeRequestInWork(RequestBindingModel model)
        {
            using (var transaction = context.Database.BeginTransaction())
            {
                try
                {

                    Request element = context.Requests.FirstOrDefault(rec => rec.Id == model.Id);
                    if (element == null)
                    {
                        throw new Exception("Элемент не найден");
                    }
                    var CoctailIngredients = context.CoctailIngredients
                                                .Include(rec => rec.Ingredient)
                                                .Where(rec => rec.CoctailId == element.CoctailId);
                    // списываем
                    foreach (var CoctailIngredient in CoctailIngredients)
                    {
                        int countOnStorages = CoctailIngredient.Count * element.Count;
                        var StorageIngredients = context.StorageIngredients
                                                    .Where(rec => rec.IngredientId == CoctailIngredient.IngredientId);
                        foreach (var StorageIngredient in StorageIngredients)
                        {
                            // компонентов на одном слкаде может не хватать
                            if (StorageIngredient.Count >= countOnStorages)
                            {
                                StorageIngredient.Count -= countOnStorages;
                                countOnStorages = 0;
                                context.SaveChanges();
                                break;
                            }
                            else
                            {
                                countOnStorages -= StorageIngredient.Count;
                                StorageIngredient.Count = 0;
                                context.SaveChanges();
                            }
                        }
                        if (countOnStorages > 0)
                        {
                            throw new Exception("Не достаточно компонента " +
                               CoctailIngredient.Ingredient.IngredientName + " требуется " +
                               CoctailIngredient.Count + ", не хватает " + countOnStorages);
                        }
                    }
                    element.BarmenId = model.BarmenId;
                    element.DateImplement = DateTime.Now;
                    element.Status = RequestStatus.Выполняется;
                    context.SaveChanges();
                    transaction.Commit();
                }
                catch (Exception)
                {
                    transaction.Rollback();
                    throw;
                }
            }
        }

        public void FinishRequest(int id)
        {
            Request element = context.Requests.FirstOrDefault(rec => rec.Id == id);
            if (element == null)
            {
                throw new Exception("Элемент не найден");
            }
            element.Status = RequestStatus.Готов;
            context.SaveChanges();
        }

        public void PayRequest(int id)
        {
            Request element = context.Requests.FirstOrDefault(rec => rec.Id == id);
            if (element == null)
            {
                throw new Exception("Элемент не найден");
            }
            element.Status = RequestStatus.Оплачен;
            context.SaveChanges();
        }

        public void PutIngredientOnStorage(StorageIngredientBindingModel model)
        {
            StorageIngredient element = context.StorageIngredients
                                                .FirstOrDefault(rec => rec.StorageId == model.StorageId &&
                                                                    rec.IngredientId == model.IngredientId);
            if (element != null)
            {
                element.Count += model.Count;
            }
            else
            {
                context.StorageIngredients.Add(new StorageIngredient
                {
                    StorageId = model.StorageId,
                    IngredientId = model.IngredientId,
                    Count = model.Count
                });
            }
            context.SaveChanges();
        }
    }
}