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
    public class StorageServiceBD : IStorageService
    {
        private AbstractDbContext context_;

        public StorageServiceBD(AbstractDbContext context_)
        {
            this.context_ = context_;
        }

        public List<StorageViewModel> GetList()
        {
            List<StorageViewModel> result = context_.Storages
                .Select(rec => new StorageViewModel
                {
                    Id = rec.Id,
                    StorageName = rec.StorageName,
                    StorageIngredients = context_.StorageIngredients
                            .Where(recPC => recPC.StorageId == rec.Id)
                            .Select(recPC => new StorageIngredientsViewModel
                            {
                                Id = recPC.Id,
                                StorageId = recPC.StorageId,
                                Ingredients_Id = recPC.Ingredients_Id,
                                IngredientsName = recPC.Ingredients.IngredientsName,
                                Count = recPC.Count
                            })
                            .ToList()
                })
                .ToList();
            return result;
        }

        public StorageViewModel GetElement(int id)
        {
            Storage element = context_.Storages.FirstOrDefault(rec => rec.Id == id);
            if (element != null)
            {
                return new StorageViewModel
                {
                    Id = element.Id,
                    StorageName = element.StorageName,
                    StorageIngredients = context_.StorageIngredients
                            .Where(recPC => recPC.StorageId == element.Id)
                            .Select(recPC => new StorageIngredientsViewModel
                            {
                                Id = recPC.Id,
                                StorageId = recPC.StorageId,
                               Ingredients_Id = recPC.Ingredients_Id,
                                IngredientsName = recPC.Ingredients.IngredientsName,
                                Count = recPC.Count
                            })
                            .ToList()
                };
            }
            throw new Exception("Элемент не найден");
        }

        public void AddElement(StorageBindingModel model)
        {
            Storage element = context_.Storages.FirstOrDefault(rec => rec.StorageName == model.StorageName);
            if (element != null)
            {
                throw new Exception("Уже есть склад с таким названием");
            }
            context_.Storages.Add(new Storage
            {
                StorageName = model.StorageName
            });
            context_.SaveChanges();
        }

        public void UpdElement(StorageBindingModel model)
        {
            Storage element = context_.Storages.FirstOrDefault(rec =>
                                        rec.StorageName == model.StorageName && rec.Id != model.Id);
            if (element != null)
            {
                throw new Exception("Уже есть склад с таким названием");
            }
            element = context_.Storages.FirstOrDefault(rec => rec.Id == model.Id);
            if (element == null)
            {
                throw new Exception("Элемент не найден");
            }
            element.StorageName = model.StorageName;
            context_.SaveChanges();
        }

        public void DelElement(int id)
        {
            using (var transaction = context_.Database.BeginTransaction())
            {
                try
                {
                    Storage element = context_.Storages.FirstOrDefault(rec => rec.Id == id);
                    if (element != null)
                    {
                        // при удалении удаляем все записи о компонентах на удаляемом складе
                        context_.StorageIngredients.RemoveRange(
                                            context_.StorageIngredients.Where(rec => rec.StorageId == id));
                        context_.Storages.Remove(element);
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
