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
            List<CoctailViewModel> result = new List<CoctailViewModel>();
            for (int i = 0; i < source.Coctails.Count; ++i)
            {
                // требуется дополнительно получить список компонентов для изделия и их количество
                List<CoctailIngredientViewModel> CoctailIngredients = new List<CoctailIngredientViewModel>();
                for (int j = 0; j < source.CoctailIngredients.Count; ++j)
                {
                    if (source.CoctailIngredients[j].CoctailId == source.Coctails[i].Id)
                    {
                        string IngredientName = string.Empty;
                        for (int k = 0; k < source.Ingredients.Count; ++k)
                        {
                            if (source.CoctailIngredients[j].IngredientId == source.Ingredients[k].Id)
                            {
                                IngredientName = source.Ingredients[k].IngredientName;
                                break;
                            }
                        }
                        CoctailIngredients.Add(new CoctailIngredientViewModel
                        {
                            Id = source.CoctailIngredients[j].Id,
                            CoctailId = source.CoctailIngredients[j].CoctailId,
                            IngredientId = source.CoctailIngredients[j].IngredientId,
                            IngredientName = IngredientName,
                            Count = source.CoctailIngredients[j].Count
                        });
                    }
                }
                result.Add(new CoctailViewModel
                {
                    Id = source.Coctails[i].Id,
                    CoctailName = source.Coctails[i].CoctailName,
                    Price = source.Coctails[i].Price,
                    CoctailIngredients = CoctailIngredients
                });
            }
            return result;
        }

        public CoctailViewModel GetElement(int id)
        {
            for (int i = 0; i < source.Coctails.Count; ++i)
            {
                // требуется дополнительно получить список компонентов для изделия и их количество
                List<CoctailIngredientViewModel> CoctailIngredients = new List<CoctailIngredientViewModel>();
                for (int j = 0; j < source.CoctailIngredients.Count; ++j)
                {
                    if (source.CoctailIngredients[j].CoctailId == source.Coctails[i].Id)
                    {
                        string IngredientName = string.Empty;
                        for (int k = 0; k < source.Ingredients.Count; ++k)
                        {
                            if (source.CoctailIngredients[j].IngredientId == source.Ingredients[k].Id)
                            {
                                IngredientName = source.Ingredients[k].IngredientName;
                                break;
                            }
                        }
                        CoctailIngredients.Add(new CoctailIngredientViewModel
                        {
                            Id = source.CoctailIngredients[j].Id,
                            CoctailId = source.CoctailIngredients[j].CoctailId,
                            IngredientId = source.CoctailIngredients[j].IngredientId,
                            IngredientName = IngredientName,
                            Count = source.CoctailIngredients[j].Count
                        });
                    }
                }
                if (source.Coctails[i].Id == id)
                {
                    return new CoctailViewModel
                    {
                        Id = source.Coctails[i].Id,
                        CoctailName = source.Coctails[i].CoctailName,
                        Price = source.Coctails[i].Price,
                        CoctailIngredients = CoctailIngredients
                    };
                }
            }

            throw new Exception("Элемент не найден");
        }

        public void AddElement(CoctailBindingModel model)
        {
            int maxId = 0;
            for (int i = 0; i < source.Coctails.Count; ++i)
            {
                if (source.Coctails[i].Id > maxId)
                {
                    maxId = source.Coctails[i].Id;
                }
                if (source.Coctails[i].CoctailName == model.CoctailName)
                {
                    throw new Exception("Уже есть изделие с таким названием");
                }
            }
            source.Coctails.Add(new Coctail
            {
                Id = maxId + 1,
                CoctailName = model.CoctailName,
                Price = model.Price
            });
            // компоненты для изделия
            int maxPCId = 0;
            for (int i = 0; i < source.CoctailIngredients.Count; ++i)
            {
                if (source.CoctailIngredients[i].Id > maxPCId)
                {
                    maxPCId = source.CoctailIngredients[i].Id;
                }
            }
            // убираем дубли по компонентам
            for (int i = 0; i < model.CoctailIngredients.Count; ++i)
            {
                for (int j = 1; j < model.CoctailIngredients.Count; ++j)
                {
                    if (model.CoctailIngredients[i].IngredientId ==
                        model.CoctailIngredients[j].IngredientId)
                    {
                        model.CoctailIngredients[i].Count +=
                            model.CoctailIngredients[j].Count;
                        model.CoctailIngredients.RemoveAt(j--);
                    }
                }
            }
            // добавляем компоненты
            for (int i = 0; i < model.CoctailIngredients.Count; ++i)
            {
                source.CoctailIngredients.Add(new CoctailIngredient
                {
                    Id = ++maxPCId,
                    CoctailId = maxId + 1,
                    IngredientId = model.CoctailIngredients[i].IngredientId,
                    Count = model.CoctailIngredients[i].Count
                });
            }
        }

        public void UpdElement(CoctailBindingModel model)
        {
            int index = -1;
            for (int i = 0; i < source.Coctails.Count; ++i)
            {
                if (source.Coctails[i].Id == model.Id)
                {
                    index = i;
                }
                if (source.Coctails[i].CoctailName == model.CoctailName &&
                    source.Coctails[i].Id != model.Id)
                {
                    throw new Exception("Уже есть изделие с таким названием");
                }
            }
            if (index == -1)
            {
                throw new Exception("Элемент не найден");
            }
            source.Coctails[index].CoctailName = model.CoctailName;
            source.Coctails[index].Price = model.Price;
            int maxPCId = 0;
            for (int i = 0; i < source.CoctailIngredients.Count; ++i)
            {
                if (source.CoctailIngredients[i].Id > maxPCId)
                {
                    maxPCId = source.CoctailIngredients[i].Id;
                }
            }
            // обновляем существуюущие компоненты
            for (int i = 0; i < source.CoctailIngredients.Count; ++i)
            {
                if (source.CoctailIngredients[i].CoctailId == model.Id)
                {
                    bool flag = true;
                    for (int j = 0; j < model.CoctailIngredients.Count; ++j)
                    {
                        // если встретили, то изменяем количество
                        if (source.CoctailIngredients[i].Id == model.CoctailIngredients[j].Id)
                        {
                            source.CoctailIngredients[i].Count = model.CoctailIngredients[j].Count;
                            flag = false;
                            break;
                        }
                    }
                    // если не встретили, то удаляем
                    if (flag)
                    {
                        source.CoctailIngredients.RemoveAt(i--);
                    }
                }
            }
            // новые записи
            for (int i = 0; i < model.CoctailIngredients.Count; ++i)
            {
                if (model.CoctailIngredients[i].Id == 0)
                {
                    // ищем дубли
                    for (int j = 0; j < source.CoctailIngredients.Count; ++j)
                    {
                        if (source.CoctailIngredients[j].CoctailId == model.Id &&
                            source.CoctailIngredients[j].IngredientId == model.CoctailIngredients[i].IngredientId)
                        {
                            source.CoctailIngredients[j].Count += model.CoctailIngredients[i].Count;
                            model.CoctailIngredients[i].Id = source.CoctailIngredients[j].Id;
                            break;
                        }
                    }
                    // если не нашли дубли, то новая запись
                    if (model.CoctailIngredients[i].Id == 0)
                    {
                        source.CoctailIngredients.Add(new CoctailIngredient
                        {
                            Id = ++maxPCId,
                            CoctailId = model.Id,
                            IngredientId = model.CoctailIngredients[i].IngredientId,
                            Count = model.CoctailIngredients[i].Count
                        });
                    }
                }
            }
        }

        public void DelElement(int id)
        {
            // удаяем записи по компонентам при удалении изделия
            for (int i = 0; i < source.CoctailIngredients.Count; ++i)
            {
                if (source.CoctailIngredients[i].CoctailId == id)
                {
                    source.CoctailIngredients.RemoveAt(i--);
                }
            }
            for (int i = 0; i < source.Coctails.Count; ++i)
            {
                if (source.Coctails[i].Id == id)
                {
                    source.Coctails.RemoveAt(i);
                    return;
                }
            }
            throw new Exception("Элемент не найден");
        }
    }
}