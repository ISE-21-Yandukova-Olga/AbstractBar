

using AbstractBarModel;
using AbstractBarService.BindingModel;
using AbstractBarService.Interface;
using AbstractBarService.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AbstractBarService.ImplementationsList
{
    public class CoctailServiceList : ICoctailService
    {
        private DataListSingleton source;

        public CoctailServiceList()
        {
            source = DataListSingleton.GetInstance();
        }

        public List<CoctailViewModel> GetList()
        {
            List<CoctailViewModel> result = source.Coctails
                .Select(rec => new CoctailViewModel
                {
                    Id = rec.Id,
                    CoctailName = rec.CoctailName,
                    Price = rec.Price,
                    CoctailIngredients = source.CoctailIngredients
                            .Where(recPC => recPC.Coctail_Id == rec.Id)
                            .Select(recPC => new CoctailIngredientsViewModel
                            {
                                Id = recPC.Id,
                                Coctail_Id = recPC.Coctail_Id,
                                Ingredients_Id = recPC.Ingredients_Id,
                                IngredientsName = source.Ingredients
                                    .FirstOrDefault(recC => recC.Id == recPC.Coctail_Id)?.IngredientsName,
                                Count = recPC.Count
                            })
                            .ToList()
                })
                .ToList();
            return result;
        }

        public CoctailViewModel GetElement(int id)
        {
            Coctail element = source.Coctails.FirstOrDefault(rec => rec.Id == id);
            if (element != null)
            {
                return new CoctailViewModel
                {
                    Id = element.Id,
                    CoctailName = element.CoctailName,
                    Price = element.Price,
                    CoctailIngredients= source.CoctailIngredients
                            .Where(recPC => recPC.Coctail_Id == element.Id)
                            .Select(recPC => new CoctailIngredientsViewModel
                            {
                                Id = recPC.Id,
                                Coctail_Id = recPC.Coctail_Id,
                                Ingredients_Id = recPC.Ingredients_Id,
                                IngredientsName = source.Ingredients
                                        .FirstOrDefault(recC => recC.Id == recPC.Ingredients_Id)?.IngredientsName,
                                Count = recPC.Count
                            })
                            .ToList()
                };
            }
            throw new Exception("Элемент не найден");
        }

        public void AddElement(CoctailBindingModel model)
        {
            Coctail element = source.Coctails.FirstOrDefault(rec => rec.CoctailName == model.CoctailName);
            if (element != null)
            {
                throw new Exception("Уже есть изделие с таким названием");
            }
            int maxId = source.Coctails.Count > 0 ? source.Coctails.Max(rec => rec.Id) : 0;
            source.Coctails.Add(new Coctail
            {
                Id = maxId + 1,
                CoctailName = model.CoctailName,
                Price = model.Price
            });
            // компоненты для изделия
            int maxPCId = source.CoctailIngredients.Count > 0 ?
                                    source.CoctailIngredients.Max(rec => rec.Id) : 0;
            // убираем дубли по компонентам
            var groupComponents = model.CoctailIngredients
                                        .GroupBy(rec => rec.Ingredients_Id)
                                        .Select(rec => new
                                        {
                                            Ingredients_Id = rec.Key,
                                            Count = rec.Sum(r => r.Count)
                                        });
            // добавляем компоненты
            foreach (var groupComponent in groupComponents)
            {
                source.CoctailIngredients.Add(new IngredientsCoctail
                {
                    Id = ++maxPCId,
                    Coctail_Id = maxId + 1,
                    Ingredients_Id = groupComponent.Ingredients_Id,
                    Count = groupComponent.Count
                });
            }
        }

        public void UpdElement(CoctailBindingModel model)
        {
            Coctail element = source.Coctails.FirstOrDefault(rec =>
                                        rec.CoctailName == model.CoctailName && rec.Id != model.Id);
            if (element != null)
            {
                throw new Exception("Уже есть изделие с таким названием");
            }
            element = source.Coctails.FirstOrDefault(rec => rec.Id == model.Id);
            if (element == null)
            {
                throw new Exception("Элемент не найден");
            }
            element.CoctailName = model.CoctailName;
            element.Price = model.Price;

            int maxPCId = source.CoctailIngredients.Count > 0 ? source.CoctailIngredients.Max(rec => rec.Id) : 0;
            // обновляем существуюущие компоненты
            var compIds = model.CoctailIngredients.Select(rec => rec.Ingredients_Id).Distinct();
            var updateComponents = source.CoctailIngredients
                                            .Where(rec => rec.Coctail_Id == model.Id &&
                                           compIds.Contains(rec.Ingredients_Id));
            foreach (var updateComponent in updateComponents)
            {
                updateComponent.Count = model.CoctailIngredients
                                                .FirstOrDefault(rec => rec.Id == updateComponent.Id).Count;
            }
            source.CoctailIngredients.RemoveAll(rec => rec.Coctail_Id == model.Id &&
                                       !compIds.Contains(rec.Ingredients_Id));
            // новые записи
            var groupComponents = model.CoctailIngredients
                                        .Where(rec => rec.Id == 0)
                                        .GroupBy(rec => rec.Ingredients_Id)
                                        .Select(rec => new
                                        {
                                            Ingredients_Id = rec.Key,
                                            Count = rec.Sum(r => r.Count)
                                        });
            foreach (var groupComponent in groupComponents)
            {
                IngredientsCoctail elementPC = source.CoctailIngredients
                                        .FirstOrDefault(rec => rec.Coctail_Id == model.Id &&
                                                        rec.Ingredients_Id == groupComponent.Ingredients_Id);
                if (elementPC != null)
                {
                    elementPC.Count += groupComponent.Count;
                }
                else
                {
                    source.CoctailIngredients.Add(new IngredientsCoctail
                    {
                        Id = ++maxPCId,
                        Coctail_Id = model.Id,
                        Ingredients_Id = groupComponent.Ingredients_Id,
                        Count = groupComponent.Count
                    });
                }
            }
        }

        public void DelElement(int id)
        {
            Coctail element = source.Coctails.FirstOrDefault(rec => rec.Id == id);
            if (element != null)
            {
                // удаяем записи по компонентам при удалении изделия
                source.CoctailIngredients.RemoveAll(rec => rec.Coctail_Id == id);
                source.Coctails.Remove(element);
            }
            else
            {
                throw new Exception("Элемент не найден");
            }
        }
    }
}
