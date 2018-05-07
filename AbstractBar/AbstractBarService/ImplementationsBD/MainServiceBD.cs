using AbstractBarModel;
using AbstractBarService.BindingModel;
using AbstractBarService.Interface;
using AbstractBarService.ViewModel;
using System;
using System.Collections.Generic;
using System.Data.Entity.SqlServer;
using System.Linq;
using System.Data.Entity;

namespace AbstractBarService.ImplementationsBD
{
    public class MainServiceBD : IMainService
    {
        private AbstractDbContext context_;

        public MainServiceBD(AbstractDbContext context_)
        {
            this.context_ = context_;
        }

        public List<RequestViewModel> GetList()
        {
            List<RequestViewModel> result = context_.Requests
                .Select(rec => new RequestViewModel
                {
                    Id = rec.Id,
                   CustomerId = rec.CustomerId,
                    Coctail_Id = rec.Coctail_Id,
                    BarmenId = rec.BarmenId,
                    DateCreate = SqlFunctions.DateName("dd", rec.DateCreate) + " " +
                                SqlFunctions.DateName("mm", rec.DateCreate) + " " +
                                SqlFunctions.DateName("yyyy", rec.DateCreate),
                    DateBarmen= rec.DateBarmen == null ? "" :
                                        SqlFunctions.DateName("dd", rec.DateBarmen.Value) + " " +
                                        SqlFunctions.DateName("mm", rec.DateBarmen.Value) + " " +
                                        SqlFunctions.DateName("yyyy", rec.DateBarmen.Value),
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
            context_.Requests.Add(new Request
            {
                CustomerId = model.CustomerId,
                Coctail_Id = model.Coctail_Id,
                DateCreate = DateTime.Now,
                Count = model.Count,
                Sum = model.Sum,
                Status = RequestStatus.Принят
            });
            context_.SaveChanges();
        }

        public void TakeRequestInWork(RequestBindingModel model)
        {
            using (var transaction = context_.Database.BeginTransaction())
            {
                try
                {

                    Request element = context_.Requests.FirstOrDefault(rec => rec.Id == model.Id);
                    if (element == null)
                    {
                        throw new Exception("Элемент не найден");
                    }
                    var coctailIngredients = context_.CoctailIngredients
                                               .Include(rec => rec.Ingredients)
                                                .Where(rec => rec.Coctail_Id == element.Coctail_Id);
                    // списываем
                    foreach (var coctailIngredient in coctailIngredients)
                    {
                        int countOnStorages = coctailIngredient.Count * element.Count;
                        var storageIngredients = context_.StorageIngredients
                                                    .Where(rec => rec.Ingredients_Id == coctailIngredient.Ingredients_Id);
                        foreach (var storageIngredient in storageIngredients)
                        {
                            // компонентов на одном слкаде может не хватать
                            if (storageIngredient.Count >= countOnStorages)
                            {
                                storageIngredient.Count -= countOnStorages;
                                countOnStorages = 0;
                                context_.SaveChanges();
                                break;
                            }
                            else
                            {
                                countOnStorages -= storageIngredient.Count;
                                storageIngredient.Count = 0;
                                context_.SaveChanges();
                            }
                        }
                        if (countOnStorages > 0)
                        {
                            throw new Exception("Не достаточно компонента " +
                                 coctailIngredient.Ingredients.IngredientsName + " требуется " +
                                 coctailIngredient.Count + ", не хватает " + countOnStorages);
                        }
                    }
                    element.BarmenId = model.BarmenId;
                    element.DateBarmen = DateTime.Now;
                    element.Status = RequestStatus.Выполняется;
                    context_.SaveChanges();
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
            Request element = context_.Requests.FirstOrDefault(rec => rec.Id == id);
            if (element == null)
            {
                throw new Exception("Элемент не найден");
            }
            element.Status = RequestStatus.Готов;
            context_.SaveChanges();
        }

        public void PayRequest(int id)
        {
            Request element = context_.Requests.FirstOrDefault(rec => rec.Id == id);
            if (element == null)
            {
                throw new Exception("Элемент не найден");
            }
            element.Status = RequestStatus.Оплачен;
            context_.SaveChanges();
        }

        public void PutComponentOnStorage(StorageIngredientsBindingModel model)
        {
            StorageIngredients element = context_.StorageIngredients
                                                .FirstOrDefault(rec => rec.StorageId == model.StorageId &&
                                                                    rec.Ingredients_Id == model.Ingredients_Id);
            if (element != null)
            {
                element.Count += model.Count;
            }
            else
            {
                context_.StorageIngredients.Add(new StorageIngredients
                {
                    StorageId = model.StorageId,
                    Ingredients_Id = model.Ingredients_Id,
                    Count = model.Count
                });
            }
            context_.SaveChanges();
        }
    }
}
