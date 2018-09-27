using AbstractBarModel;
using AbstractBarService.BindingModels;
using AbstractBarService.Interfaces;
using AbstractBarService.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AbstractBarService.ImplementationsList
{
    public class MainServiceList : IMainService
    {
        private DataListSingleton source;

        public MainServiceList()
        {
            source = DataListSingleton.GetInstance();
        }

        public List<RequestViewModel> GetList()
        {
            List<RequestViewModel> result = source.Requests
                .Select(rec => new RequestViewModel
                {
                    Id = rec.Id,
                    CustomerId = rec.CustomerId,
                    CoctailId = rec.CoctailId,
                   BarmenId = rec.BarmenId,
                    DateCreate = rec.DateCreate.ToLongDateString(),
                    DateImplement = rec.DateImplement?.ToLongDateString(),
                    Status = rec.Status.ToString(),
                    Count = rec.Count,
                    Sum = rec.Sum,
                    CustomerFIO = source.Customers
                                    .FirstOrDefault(recC => recC.Id == rec.CustomerId)?.CustomerFIO,
                    CoctailName = source.Coctails
                                    .FirstOrDefault(recP => recP.Id == rec.CoctailId)?.CoctailName,
                   BarmenName = source.Barmens
                                    .FirstOrDefault(recI => recI.Id == rec.BarmenId)?.BarmenFIO
                })
                .ToList();
            return result;
        }

        public void CreateRequest(RequestBindingModel model)
        {
            int maxId = source.Requests.Count > 0 ? source.Requests.Max(rec => rec.Id) : 0;
            source.Requests.Add(new Request
            {
                Id = maxId + 1,
                CustomerId = model.CustomerId,
                CoctailId = model.CoctailId,
                DateCreate = DateTime.Now,
                Count = model.Count,
                Sum = model.Sum,
                Status = RequestStatus.Принят
            });
        }

        public void TakeRequestInWork(RequestBindingModel model)
        {
            Request element = source.Requests.FirstOrDefault(rec => rec.Id == model.Id);
            if (element == null)
            {
                throw new Exception("Элемент не найден");
            }
            // смотрим по количеству компонентов на складах
            var CoctailIngredients = source.CoctailIngredients.Where(rec => rec.CoctailId == element.CoctailId);
            foreach (var CoctailIngredient in CoctailIngredients)
            {
                int countOnStorages = source.StorageIngredients
                                            .Where(rec => rec.IngredientId == CoctailIngredient.IngredientId)
                                            .Sum(rec => rec.Count);
                if (countOnStorages < CoctailIngredient.Count * element.Count)
                {
                    var IngredientName = source.Ingredients
                                    .FirstOrDefault(rec => rec.Id == CoctailIngredient.IngredientId);
                    throw new Exception("Не достаточно компонента " + IngredientName?.IngredientName +
                        " требуется " + CoctailIngredient.Count + ", в наличии " + countOnStorages);
                }
            }
            // списываем
            foreach (var CoctailIngredient in CoctailIngredients)
            {
                int countOnStorages = CoctailIngredient.Count * element.Count;
                var StorageIngredients = source.StorageIngredients
                                            .Where(rec => rec.IngredientId == CoctailIngredient.IngredientId);
                foreach (var StorageIngredient in StorageIngredients)
                {
                    // компонентов на одном слкаде может не хватать
                    if (StorageIngredient.Count >= countOnStorages)
                    {
                        StorageIngredient.Count -= countOnStorages;
                        break;
                    }
                    else
                    {
                        countOnStorages -= StorageIngredient.Count;
                        StorageIngredient.Count = 0;
                    }
                }
            }
            element.BarmenId = model.BarmenId;
            element.DateImplement = DateTime.Now;
            element.Status = RequestStatus.Выполняется;
        }

        public void FinishRequest(int id)
        {
            Request element = source.Requests.FirstOrDefault(rec => rec.Id == id);
            if (element == null)
            {
                throw new Exception("Элемент не найден");
            }
            element.Status = RequestStatus.Готов;
        }

        public void PayRequest(int id)
        {
            Request element = source.Requests.FirstOrDefault(rec => rec.Id == id);
            if (element == null)
            {
                throw new Exception("Элемент не найден");
            }
            element.Status = RequestStatus.Оплачен;
        }

        public void PutIngredientOnStorage(StorageIngredientBindingModel model)
        {
            StorageIngredient element = source.StorageIngredients
                                                .FirstOrDefault(rec => rec.StorageId == model.StorageId &&
                                                                    rec.IngredientId == model.IngredientId);
            if (element != null)
            {
                element.Count += model.Count;
            }
            else
            {
                int maxId = source.StorageIngredients.Count > 0 ? source.StorageIngredients.Max(rec => rec.Id) : 0;
                source.StorageIngredients.Add(new StorageIngredient
                {
                    Id = ++maxId,
                    StorageId = model.StorageId,
                    IngredientId = model.IngredientId,
                    Count = model.Count
                });
            }
        }
    }
}