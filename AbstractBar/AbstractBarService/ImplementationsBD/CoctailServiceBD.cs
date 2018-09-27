using AbstractBarModel;
using AbstractBarService.BindingModels;
using AbstractBarService.Interfaces;
using AbstractBarService.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AbstractBarService.ImplementationsBD
{
    public class CoctailServiceBD : ICoctailService
    {
        private AbstractDbContext context;

        public CoctailServiceBD(AbstractDbContext context)
        {
            this.context = context;
        }

        public List<CoctailViewModel> GetList()
        {
            List<CoctailViewModel> result = context.Coctails
                .Select(rec => new CoctailViewModel
                {
                    Id = rec.Id,
                    CoctailName = rec.CoctailName,
                    Price = rec.Price,
                    CoctailIngredients = context.CoctailIngredients
                            .Where(recPC => recPC.CoctailId == rec.Id)
                            .Select(recPC => new CoctailIngredientViewModel
                            {
                                Id = recPC.Id,
                                CoctailId = recPC.CoctailId,
                                IngredientId = recPC.IngredientId,
                                IngredientName = recPC.Ingredient.IngredientName,
                                Count = recPC.Count
                            })
                            .ToList()
                })
                .ToList();
            return result;
        }

        public CoctailViewModel GetElement(int id)
        {
            Coctail element = context.Coctails.FirstOrDefault(rec => rec.Id == id);
            if (element != null)
            {
                return new CoctailViewModel
                {
                    Id = element.Id,
                    CoctailName = element.CoctailName,
                    Price = element.Price,
                    CoctailIngredients = context.CoctailIngredients
                            .Where(recPC => recPC.CoctailId == element.Id)
                            .Select(recPC => new CoctailIngredientViewModel
                            {
                                Id = recPC.Id,
                                CoctailId = recPC.CoctailId,
                                IngredientId = recPC.IngredientId,
                                IngredientName = recPC.Ingredient.IngredientName,
                                Count = recPC.Count
                            })
                            .ToList()
                };
            }
            throw new Exception("Элемент не найден");
        }

        public void AddElement(CoctailBindingModel model)
        {
            using (var transaction = context.Database.BeginTransaction())
            {
                try
                {
                    Coctail element = context.Coctails.FirstOrDefault(rec => rec.CoctailName == model.CoctailName);
                    if (element != null)
                    {
                        throw new Exception("Уже есть изделие с таким названием");
                    }
                    element = new Coctail
                    {
                        CoctailName = model.CoctailName,
                        Price = model.Price
                    };
                    context.Coctails.Add(element);
                    context.SaveChanges();
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
                        context.CoctailIngredients.Add(new CoctailIngredient
                        {
                            CoctailId = element.Id,
                            IngredientId = groupIngredient.IngredientId,
                            Count = groupIngredient.Count
                        });
                        context.SaveChanges();
                    }
                    transaction.Commit();
                }
                catch (Exception)
                {
                    transaction.Rollback();
                    throw;
                }
            }
        }

        public void UpdElement(CoctailBindingModel model)
        {
            using (var transaction = context.Database.BeginTransaction())
            {
                try
                {
                    Coctail element = context.Coctails.FirstOrDefault(rec =>
                                        rec.CoctailName == model.CoctailName && rec.Id != model.Id);
                    if (element != null)
                    {
                        throw new Exception("Уже есть изделие с таким названием");
                    }
                    element = context.Coctails.FirstOrDefault(rec => rec.Id == model.Id);
                    if (element == null)
                    {
                        throw new Exception("Элемент не найден");
                    }
                    element.CoctailName = model.CoctailName;
                    element.Price = model.Price;
                    context.SaveChanges();

                    // обновляем существуюущие компоненты
                    var compIds = model.CoctailIngredients.Select(rec => rec.IngredientId).Distinct();
                    var updateIngredients = context.CoctailIngredients
                                                    .Where(rec => rec.CoctailId == model.Id &&
                                                        compIds.Contains(rec.IngredientId));
                    foreach (var updateIngredient in updateIngredients)
                    {
                        updateIngredient.Count = model.CoctailIngredients
                                                        .FirstOrDefault(rec => rec.Id == updateIngredient.Id).Count;
                    }
                    context.SaveChanges();
                    context.CoctailIngredients.RemoveRange(
                                        context.CoctailIngredients.Where(rec => rec.CoctailId == model.Id &&
                                                                            !compIds.Contains(rec.IngredientId)));
                    context.SaveChanges();
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
                        CoctailIngredient elementPC = context.CoctailIngredients
                                                .FirstOrDefault(rec => rec.CoctailId == model.Id &&
                                                                rec.IngredientId == groupIngredient.IngredientId);
                        if (elementPC != null)
                        {
                            elementPC.Count += groupIngredient.Count;
                            context.SaveChanges();
                        }
                        else
                        {
                            context.CoctailIngredients.Add(new CoctailIngredient
                            {
                                CoctailId = model.Id,
                                IngredientId = groupIngredient.IngredientId,
                                Count = groupIngredient.Count
                            });
                            context.SaveChanges();
                        }
                    }
                    transaction.Commit();
                }
                catch (Exception)
                {
                    transaction.Rollback();
                    throw;
                }
            }
        }

        public void DelElement(int id)
        {
            using (var transaction = context.Database.BeginTransaction())
            {
                try
                {
                    Coctail element = context.Coctails.FirstOrDefault(rec => rec.Id == id);
                    if (element != null)
                    {
                        // удаяем записи по компонентам при удалении изделия
                        context.CoctailIngredients.RemoveRange(
                                            context.CoctailIngredients.Where(rec => rec.CoctailId == id));
                        context.Coctails.Remove(element);
                        context.SaveChanges();
                    }
                    else
                    {
                        throw new Exception("Элемент не найден");
                    }
                    transaction.Commit();
                }
                catch (Exception)
                {
                    transaction.Rollback();
                    throw;
                }
            }
        }
    }
}