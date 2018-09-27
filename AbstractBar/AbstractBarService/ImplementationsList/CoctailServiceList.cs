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
                            .Where(recPC => recPC.CoctailId == rec.Id)
                            .Select(recPC => new CoctailIngredientViewModel
                            {
                                Id = recPC.Id,
                                CoctailId = recPC.CoctailId,
                                IngredientId = recPC.IngredientId,
                                IngredientName = source.Ingredients
                                    .FirstOrDefault(recC => recC.Id == recPC.IngredientId)?.IngredientName,
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
                    CoctailIngredients = source.CoctailIngredients
                            .Where(recPC => recPC.CoctailId == element.Id)
                            .Select(recPC => new CoctailIngredientViewModel
                            {
                                Id = recPC.Id,
                                CoctailId = recPC.CoctailId,
                                IngredientId = recPC.IngredientId,
                                IngredientName = source.Ingredients
                                        .FirstOrDefault(recC => recC.Id == recPC.IngredientId)?.IngredientName,
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
            var groupIngredients = model.CoctailIngredients
                                        .GroupBy(rec => rec.IngredientId)
                                        .Select(rec => new
                                        {
                                            IngredientId = rec.Key,
                                            Count = rec.Sum(r => r.Count)
                                        });
            // добавляем компоненты
            foreach (var groupIngredient in groupIngredients)
            {
                source.CoctailIngredients.Add(new CoctailIngredient
                {
                    Id = ++maxPCId,
                    CoctailId = maxId + 1,
                    IngredientId = groupIngredient.IngredientId,
                    Count = groupIngredient.Count
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
            var compIds = model.CoctailIngredients.Select(rec => rec.IngredientId).Distinct();
            var updateIngredients = source.CoctailIngredients
                                            .Where(rec => rec.CoctailId == model.Id &&
                                           compIds.Contains(rec.IngredientId));
            foreach (var updateIngredient in updateIngredients)
            {
                updateIngredient.Count = model.CoctailIngredients
                                                .FirstOrDefault(rec => rec.Id == updateIngredient.Id).Count;
            }
            source.CoctailIngredients.RemoveAll(rec => rec.CoctailId == model.Id &&
                                       !compIds.Contains(rec.IngredientId));
            // новые записи
            var groupIngredients = model.CoctailIngredients
                                        .Where(rec => rec.Id == 0)
                                        .GroupBy(rec => rec.IngredientId)
                                        .Select(rec => new
                                        {
                                            IngredientId = rec.Key,
                                            Count = rec.Sum(r => r.Count)
                                        });
            foreach (var groupIngredient in groupIngredients)
            {
                CoctailIngredient elementPC = source.CoctailIngredients
                                        .FirstOrDefault(rec => rec.CoctailId == model.Id &&
                                                        rec.IngredientId == groupIngredient.IngredientId);
                if (elementPC != null)
                {
                    elementPC.Count += groupIngredient.Count;
                }
                else
                {
                    source.CoctailIngredients.Add(new CoctailIngredient
                    {
                        Id = ++maxPCId,
                        CoctailId = model.Id,
                        IngredientId = groupIngredient.IngredientId,
                        Count = groupIngredient.Count
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
                source.CoctailIngredients.RemoveAll(rec => rec.CoctailId == id);
                source.Coctails.Remove(element);
            }
            else
            {
                throw new Exception("Элемент не найден");
            }
        }
    }
}