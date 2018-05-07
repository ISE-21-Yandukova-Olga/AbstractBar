﻿
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
            List<RequestViewModel> result = new List<RequestViewModel>();
            for (int i = 0; i < source.Requests.Count; ++i)
            {
                string CustomerFIO = string.Empty;
                for (int j = 0; j < source.Customers.Count; ++j)
                {
                    if (source.Customers[j].Id == source.Requests[i].CustomerId)
                    {
                        CustomerFIO = source.Customers[j].CustomerFIO;
                        break;
                    }
                }
                string CoctailName = string.Empty;
                for (int j = 0; j < source.Coctails.Count; ++j)
                {
                    if (source.Coctails[j].Id == source.Requests[i].Coctail_Id)
                    {
                       CoctailName = source.Coctails[j].CoctailName;
                        break;
                    }
                }
                string BarmenFIO = string.Empty;
                if (source.Requests[i].BarmenId.HasValue)
                {
                    for (int j = 0; j < source.Barmens.Count; ++j)
                    {
                        if (source.Barmens[j].Id == source.Requests[i].BarmenId.Value)
                        {
                            BarmenFIO = source.Barmens[j].BarmenFIO;
                            break;
                        }
                    }
                }
                result.Add(new RequestViewModel
                {
                    Id = source.Requests[i].Id,
                    CustomerId = source.Requests[i].CustomerId,
                    CustomerFIO = CustomerFIO,
                    Coctail_Id = source.Requests[i].Coctail_Id,
                    CoctailName = CoctailName,
                    BarmenId = source.Requests[i].BarmenId,
                    BarmenName = BarmenFIO,
                    Count = source.Requests[i].Count,
                    Sum = source.Requests[i].Sum,
                    DateCreate = source.Requests[i].DateCreate.ToLongDateString(),
                    DateBarmen = source.Requests[i].DateBarmen?.ToLongDateString(),
                    Status = source.Requests[i].Status.ToString()
                });
            }
            return result;
        }

        public void CreateRequest(RequestBindingModel model)
        {
            int maxId = 0;
            for (int i = 0; i < source.Requests.Count; ++i)
            {
                if (source.Requests[i].Id > maxId)
                {
                    maxId = source.Customers[i].Id;
                }
            }
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
            int index = -1;
            for (int i = 0; i < source.Requests.Count; ++i)
            {
                if (source.Requests[i].Id == model.Id)
                {
                    index = i;
                    break;
                }
            }
            if (index == -1)
            {
                throw new Exception("Элемент не найден");
            }
            // смотрим по количеству компонентов на складах
            for (int i = 0; i < source.CoctailIngredients.Count; ++i)
            {
                if (source.CoctailIngredients[i].Coctail_Id == source.Requests[index].Coctail_Id)
                {
                    int countOnStocks = 0;
                    for (int j = 0; j < source.StorageIngredients.Count; ++j)
                    {
                        if (source.StorageIngredients[j].Ingredients_Id == source.CoctailIngredients[i].Ingredients_Id)
                        {
                            countOnStocks += source.StorageIngredients[j].Count;
                        }
                    }
                    if (countOnStocks < source.CoctailIngredients[i].Count * source.Requests[index].Count)
                    {
                        for (int j = 0; j < source.Ingredients.Count; ++j)
                        {
                            if (source.Ingredients[j].Id == source.CoctailIngredients[i].Ingredients_Id)
                            {
                                throw new Exception("Не достаточно компонента " + source.Ingredients[j].IngredientsName +
                                    " требуется " + source.CoctailIngredients[i].Count + ", в наличии " + countOnStocks);
                            }
                        }
                    }
                }
            }
            // списываем
            for (int i = 0; i < source.CoctailIngredients.Count; ++i)
            {
                if (source.CoctailIngredients[i].Coctail_Id == source.Requests[index].Coctail_Id)
                {
                    int countOnStocks = source.CoctailIngredients[i].Count * source.Requests[index].Count;
                    for (int j = 0; j < source.StorageIngredients.Count; ++j)
                    {
                        if (source.StorageIngredients[j].Ingredients_Id == source.CoctailIngredients[i].Ingredients_Id)
                        {
                            // компонентов на одном слкаде может не хватать
                            if (source.StorageIngredients[j].Count >= countOnStocks)
                            {
                                source.StorageIngredients[j].Count -= countOnStocks;
                                break;
                            }
                            else
                            {
                                countOnStocks -= source.StorageIngredients[j].Count;
                                source.StorageIngredients[j].Count = 0;
                            }
                        }
                    }
                }
            }
            source.Requests[index].BarmenId = model.BarmenId;
            source.Requests[index].DateBarmen = DateTime.Now;
            source.Requests[index].Status = RequestStatus.Выполняется;
        }

        public void FinishRequest(int id)
        {
            int index = -1;
            for (int i = 0; i < source.Requests.Count; ++i)
            {
                if (source.Customers[i].Id == id)
                {
                    index = i;
                    break;
                }
            }
            if (index == -1)
            {
                throw new Exception("Элемент не найден");
            }
            source.Requests[index].Status = RequestStatus.Готов;
        }

        public void PayRequest(int id)
        {
            int index = -1;
            for (int i = 0; i < source.Requests.Count; ++i)
            {
                if (source.Customers[i].Id == id)
                {
                    index = i;
                    break;
                }
            }
            if (index == -1)
            {
                throw new Exception("Элемент не найден");
            }
            source.Requests[index].Status = RequestStatus.Оплачен;
        }

        public void PutComponentOnStorage(StorageIngredientsBindingModel model)
        {
            int maxId = 0;
            for (int i = 0; i < source.StorageIngredients.Count; ++i)
            {
                if (source.StorageIngredients[i].StorageId == model.StorageId &&
                    source.StorageIngredients[i].Ingredients_Id == model.Ingredients_Id)
                {
                    source.StorageIngredients[i].Count += model.Count;
                    return;
                }
                if (source.StorageIngredients[i].Id > maxId)
                {
                    maxId = source.StorageIngredients[i].Id;
                }
            }
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
