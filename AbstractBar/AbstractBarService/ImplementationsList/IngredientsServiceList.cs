
using AbstractBarModel;
using AbstractBarService.BindingModel;
using AbstractBarService.Interface;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AbstractBarService.ImplementationsList
{
    public class IngredientsServiceList : IIngredientsService
    {
        private DataListSingleton source;

        public IngredientsServiceList()
        {
            source = DataListSingleton.GetInstance();
        }

        public List<IngredientsViewModel> GetList()
        {
            List<IngredientsViewModel> result = source.Ingredients
               .Select(rec => new IngredientsViewModel
               {
                   Id = rec.Id,
                   IngredientsName = rec.IngredientsName
               })
               .ToList();
            return result;
        }

        public IngredientsViewModel GetElement(int id)
        {
            Ingredients element = source.Ingredients.FirstOrDefault(rec => rec.Id == id);
            if (element != null)
            {
                return new IngredientsViewModel
                {
                    Id = element.Id,
                    IngredientsName = element.IngredientsName
                };
            }
            throw new Exception("Элемент не найден");
        }

        public void AddElement(IngredientsBindingModel model)
        {
            Ingredients element = source.Ingredients.FirstOrDefault(rec => rec.IngredientsName == model.IngredientsName);
            if (element != null)
            {
                throw new Exception("Уже есть компонент с таким названием");
            }
            int maxId = source.Ingredients.Count > 0 ? source.Ingredients.Max(rec => rec.Id) : 0;
            source.Ingredients.Add(new Ingredients
            {
                Id = maxId + 1,
                IngredientsName = model.IngredientsName
            });
        }

        public void UpdElement(IngredientsBindingModel model)
        {
            Ingredients element = source.Ingredients.FirstOrDefault(rec =>
                                        rec.IngredientsName == model.IngredientsName && rec.Id != model.Id);
            if (element != null)
            {
                throw new Exception("Уже есть компонент с таким названием");
            }
            element = source.Ingredients.FirstOrDefault(rec => rec.Id == model.Id);
            if (element == null)
            {
                throw new Exception("Элемент не найден");
            }
            element.IngredientsName = model.IngredientsName;
        }

        public void DelElement(int id)
        {
            Ingredients element = source.Ingredients.FirstOrDefault(rec => rec.Id == id);
            if (element != null)
            {
                source.Ingredients.Remove(element);
            }
            else
            {
                throw new Exception("Элемент не найден");
            }
        }
    }
}
