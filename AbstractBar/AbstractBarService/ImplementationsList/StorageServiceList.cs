﻿
using AbstractBarModel;
using AbstractBarService.BindingModel;
using AbstractBarService.Interface;
using AbstractBarService.ViewModel;
using System;
using System.Collections.Generic;

namespace AbstractBarService.ImplementationsList
{
   public class StorageServiceList : IStorageService
    {
        private DataListSingleton source;

        public StorageServiceList()
        {
            source = DataListSingleton.GetInstance();
        }

        public List<StorageViewModel> GetList()
        {
            List<StorageViewModel> result = new List<StorageViewModel>();
            for (int i = 0; i < source.Storages.Count; ++i)
            {
                // требуется дополнительно получить список компонентов на складе и их количество
                List<StorageIngredientsViewModel> StorageIngredients = new List<StorageIngredientsViewModel>();
                for (int j = 0; j < source.StorageIngredients.Count; ++j)
                {
                    if (source.StorageIngredients[j].StorageId == source.Storages[i].Id)
                    {
                        string IngredientName = string.Empty;
                        for (int k = 0; k < source.Ingredients.Count; ++k)
                        {
                            if (source.CoctailIngredients[j].Ingredients_Id == source.Ingredients[k].Id)
                            {
                                IngredientName = source.Ingredients[k].IngredientsName;
                                break;
                            }
                        }
                        StorageIngredients.Add(new StorageIngredientsViewModel
                        {
                            Id = source.StorageIngredients[j].Id,
                            StorageId = source.StorageIngredients[j].StorageId,
                            Coctail_Id = source.StorageIngredients[j].Ingredients_Id,
                            IngredientsName = IngredientName,
                            Count = source.StorageIngredients[j].Count
                        });
                    }
                }
                result.Add(new StorageViewModel
                {
                    Id = source.Storages[i].Id,
                    StorageName = source.Storages[i].StorageName,
                    StorageIngredients = StorageIngredients
                });
            }
            return result;
        }

        public StorageViewModel GetElement(int id)
        {
            for (int i = 0; i < source.Storages.Count; ++i)
            {
                // требуется дополнительно получить список компонентов на складе и их количество
                List<StorageIngredientsViewModel> StorageIngredients = new List<StorageIngredientsViewModel>();
                for (int j = 0; j < source.StorageIngredients.Count; ++j)
                {
                    if (source.StorageIngredients[j].StorageId == source.Storages[i].Id)
                    {
                        string IngredientName = string.Empty;
                        for (int k = 0; k < source.Ingredients.Count; ++k)
                        {
                            if (source.CoctailIngredients[j].Ingredients_Id == source.Ingredients[k].Id)
                            {
                                IngredientName = source.Ingredients[k].IngredientsName;
                                break;
                            }
                        }
                        StorageIngredients.Add(new StorageIngredientsViewModel
                        {
                            Id = source.StorageIngredients[j].Id,
                            StorageId = source.StorageIngredients[j].StorageId,
                           Coctail_Id = source.StorageIngredients[j].Ingredients_Id,
                            IngredientsName = IngredientName,
                            Count = source.StorageIngredients[j].Count
                        });
                    }
                }
                if (source.Storages[i].Id == id)
                {
                    return new StorageViewModel
                    {
                        Id = source.Storages[i].Id,
                        StorageName = source.Storages[i].StorageName,
                        StorageIngredients = StorageIngredients
                    };
                }
            }
            throw new Exception("Элемент не найден");
        }

        public void AddElement(StorageBindingModel model)
        {
            int maxId = 0;
            for (int i = 0; i < source.Storages.Count; ++i)
            {
                if (source.Storages[i].Id > maxId)
                {
                    maxId = source.Storages[i].Id;
                }
                if (source.Storages[i].StorageName == model.StorageName)
                {
                    throw new Exception("Уже есть склад с таким названием");
                }
            }
            source.Storages.Add(new Storage
            {
                Id = maxId + 1,
                StorageName = model.StorageName
            });
        }

        public void UpdElement(StorageBindingModel model)
        {
            int index = -1;
            for (int i = 0; i < source.Storages.Count; ++i)
            {
                if (source.Storages[i].Id == model.Id)
                {
                    index = i;
                }
                if (source.Storages[i].StorageName == model.StorageName &&
                    source.Storages[i].Id != model.Id)
                {
                    throw new Exception("Уже есть склад с таким названием");
                }
            }
            if (index == -1)
            {
                throw new Exception("Элемент не найден");
            }
            source.Storages[index].StorageName = model.StorageName;
        }

        public void DelElement(int id)
        {
            // при удалении удаляем все записи о компонентах на удаляемом складе
            for (int i = 0; i < source.StorageIngredients.Count; ++i)
            {
                if (source.StorageIngredients[i].StorageId == id)
                {
                    source.StorageIngredients.RemoveAt(i--);
                }
            }
            for (int i = 0; i < source.Storages.Count; ++i)
            {
                if (source.Storages[i].Id == id)
                {
                    source.Storages.RemoveAt(i);
                    return;
                }
            }
            throw new Exception("Элемент не найден");
        }
    }
}
