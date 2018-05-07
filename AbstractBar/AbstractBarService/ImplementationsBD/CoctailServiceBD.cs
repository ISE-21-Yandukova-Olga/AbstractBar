using AbstractBarModel;
using AbstractBarService.BindingModel;
using AbstractBarService.Interface;
using AbstractBarService.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AbstractBarService.ImplementationsBD
{
    public class CoctailServiceBD : ICoctailService
    {
        private AbstractDbContext context_;

        public CoctailServiceBD(AbstractDbContext context_)
        {
            this.context_ = context_;
        }

        public List<CoctailViewModel> GetList()
        {
            List<CoctailViewModel> result = context_.Coctails
                .Select(rec => new CoctailViewModel
                {
                    Id = rec.Id,
                    CoctailName = rec.CoctailName,
                    Price = rec.Price,
                    CoctailIngredients = context_.CoctailIngredients
                            .Where(recPC => recPC.Coctail_Id == rec.Id)
                            .Select(recPC => new CoctailIngredientsViewModel
                            {
                                Id = recPC.Id,
                                Coctail_Id = recPC.Coctail_Id,
                                Ingredients_Id = recPC.Ingredients_Id,
                                IngredientsName = recPC.Ingredients.IngredientsName,
                                Count = recPC.Count
                            })
                            .ToList()
                })
                .ToList();
            return result;
        }

        public CoctailViewModel GetElement(int id)
        {
            Coctail element = context_.Coctails.FirstOrDefault(rec => rec.Id == id);
            if (element != null)
            {
                return new CoctailViewModel
                {
                    Id = element.Id,
                    CoctailName = element.CoctailName,
                    Price = element.Price,
                    CoctailIngredients = context_.CoctailIngredients
                            .Where(recPC => recPC.Coctail_Id == element.Id)
                            .Select(recPC => new CoctailIngredientsViewModel
                            {
                                Id = recPC.Id,
                                Coctail_Id = recPC.Coctail_Id,
                                Ingredients_Id = recPC.Ingredients_Id,
                                IngredientsName = recPC.Ingredients.IngredientsName,
                                Count = recPC.Count
                            })
                            .ToList()
                };
            }
            throw new Exception("Элемент не найден");
        }

        public void AddElement(CoctailBindingModel model)
        {
            using (var transaction = context_.Database.BeginTransaction())
            {
                try
                {
                    Coctail element = context_.Coctails.FirstOrDefault(rec => rec.CoctailName == model.CoctailName);
                    if (element != null)
                    {
                        throw new Exception("Уже есть изделие с таким названием");
                    }
                    element = new Coctail
                    {
                        CoctailName = model.CoctailName,
                        Price = model.Price
                    };
                    context_.Coctails.Add(element);
                    context_.SaveChanges();
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
                        context_.CoctailIngredients.Add(new IngredientsCoctail
                        {
                            Coctail_Id = element.Id,
                            Ingredients_Id = groupComponent.Ingredients_Id,
                            Count = groupComponent.Count
                        });
                        context_.SaveChanges();
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
            using (var transaction = context_.Database.BeginTransaction())
            {
                try
                {
                    Coctail element = context_.Coctails.FirstOrDefault(rec =>
                                        rec.CoctailName == model.CoctailName && rec.Id != model.Id);
                    if (element != null)
                    {
                        throw new Exception("Уже есть изделие с таким названием");
                    }
                    element = context_.Coctails.FirstOrDefault(rec => rec.Id == model.Id);
                    if (element == null)
                    {
                        throw new Exception("Элемент не найден");
                    }
                    element.CoctailName = model.CoctailName;
                    element.Price = model.Price;
                    context_.SaveChanges();

                    // обновляем существуюущие компоненты
                    var compIds = model.CoctailIngredients.Select(rec => rec.Ingredients_Id).Distinct();
                    var updateComponents = context_.CoctailIngredients
                                                    .Where(rec => rec.Coctail_Id == model.Id &&
                                                        compIds.Contains(rec.Ingredients_Id));
                    foreach (var updateComponent in updateComponents)
                    {
                        updateComponent.Count = model.CoctailIngredients
                                                        .FirstOrDefault(rec => rec.Id == updateComponent.Id).Count;
                    }
                    context_.SaveChanges();
                    context_.CoctailIngredients.RemoveRange(
                                        context_.CoctailIngredients.Where(rec => rec.Coctail_Id == model.Id &&
                                                                            !compIds.Contains(rec.Ingredients_Id)));
                    context_.SaveChanges();
                    // новые записи
                    var groupComponents = model.CoctailIngredients
                                                .Where(rec => rec.Id == 0)
                                                .GroupBy(rec => rec.Ingredients_Id)
                                                .Select(rec => new
                                                {
                                                    ComponentId = rec.Key,
                                                    Count = rec.Sum(r => r.Count)
                                                });
                    foreach (var groupComponent in groupComponents)
                    {
                        IngredientsCoctail elementPC = context_.CoctailIngredients
                                                .FirstOrDefault(rec => rec.Coctail_Id == model.Id &&
                                                                rec.Ingredients_Id == groupComponent.ComponentId);
                        if (elementPC != null)
                        {
                            elementPC.Count += groupComponent.Count;
                            context_.SaveChanges();
                        }
                        else
                        {
                            context_.CoctailIngredients.Add(new IngredientsCoctail
                            {
                                Coctail_Id = model.Id,
                                Ingredients_Id = groupComponent.ComponentId,
                                Count = groupComponent.Count
                            });
                            context_.SaveChanges();
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
            using (var transaction = context_.Database.BeginTransaction())
            {
                try
                {
                    Coctail element = context_.Coctails.FirstOrDefault(rec => rec.Id == id);
                    if (element != null)
                    {
                        // удаяем записи по компонентам при удалении изделия
                        context_.CoctailIngredients.RemoveRange(
                                            context_.CoctailIngredients.Where(rec => rec.Coctail_Id == id));
                        context_.Coctails.Remove(element);
                        context_.SaveChanges();
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
