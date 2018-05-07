
using AbstractBarModel;
using AbstractBarService.BindingModel;
using AbstractBarService.Interface;
using AbstractBarService.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;


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
                    Coctail_Id = rec.Coctail_Id,
                    BarmenId = rec.BarmenId,
                    DateCreate = rec.DateCreate.ToLongDateString(),
                    DateBarmen = rec.DateBarmen?.ToLongDateString(),
                    Status = rec.Status.ToString(),
                    Count = rec.Count,
                    Sum = rec.Sum,
                    CustomerFIO = source.Customers
                                    .FirstOrDefault(recC => recC.Id == rec.CustomerId)?.CustomerFIO,
                   CoctailName = source.Coctails
                                    .FirstOrDefault(recP => recP.Id == rec.Coctail_Id)?.CoctailName,
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
                Coctail_Id = model.Coctail_Id,
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
            var coctailIngredients = source.CoctailIngredients.Where(rec => rec.Coctail_Id == element.Coctail_Id);
            foreach (var coctailIngredient in coctailIngredients)
            {
                int countOnStorages = source.StorageIngredients
                                            .Where(rec => rec.Ingredients_Id == coctailIngredient.Ingredients_Id)
                                            .Sum(rec => rec.Count);
                if (countOnStorages < coctailIngredient.Count * element.Count)
                {
                    var componentName = source.Ingredients
                                    .FirstOrDefault(rec => rec.Id == coctailIngredient.Ingredients_Id);
                    throw new Exception("Не достаточно компонента " + componentName?.IngredientsName +
                        " требуется " + coctailIngredient.Count + ", в наличии " + countOnStorages);
                }
            }
            // списываем
            foreach (var coctailIngredient in coctailIngredients)
            {
                int countOnStorages = coctailIngredient.Count * element.Count;
                var StorageIngredients = source.StorageIngredients
                                            .Where(rec => rec.Ingredients_Id == coctailIngredient.Ingredients_Id);
                foreach (var storageIngredients in StorageIngredients)
                {
                    // компонентов на одном слкаде может не хватать
                    if (storageIngredients.Count >= countOnStorages)
                    {
                        storageIngredients.Count -= countOnStorages;
                        break;
                    }
                    else
                    {
                        countOnStorages -= storageIngredients.Count;
                        storageIngredients.Count = 0;
                    }
                }
            }
            element.BarmenId = model.BarmenId;
            element.DateBarmen = DateTime.Now;
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

        public void PutComponentOnStorage(StorageIngredientsBindingModel model)
        {
            StorageIngredients element = source.StorageIngredients
                                                .FirstOrDefault(rec => rec.StorageId == model.StorageId &&
                                                                    rec.Ingredients_Id == model.Ingredients_Id);
            if (element != null)
            {
                element.Count += model.Count;
            }
            else
            {
                int maxId = source.StorageIngredients.Count > 0 ? source.StorageIngredients.Max(rec => rec.Id) : 0;
                source.StorageIngredients.Add(new StorageIngredients
                {
                    Id = ++maxId,
                    StorageId = model.StorageId,
                    Ingredients_Id = model.Ingredients_Id,
                    Count = model.Count
                });
            }
        }
    }
}
