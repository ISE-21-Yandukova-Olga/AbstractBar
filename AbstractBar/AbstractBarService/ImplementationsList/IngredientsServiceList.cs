
using AbstractBarModel;
using AbstractBarService.BindingModel;
using AbstractBarService.Interface;
using System;
using System.Collections.Generic;

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
            List<IngredientsViewModel> result = new List<IngredientsViewModel>();
            for (int i = 0; i < source.Ingredients.Count; ++i)
            {
                result.Add(new IngredientsViewModel
                {
                    Id = source.Ingredients[i].Id,
                    IngredientsName = source.Ingredients[i].IngredientsName
                });
            }
            return result;
        }

        public IngredientsViewModel GetElement(int id)
        {
            for (int i = 0; i < source.Ingredients.Count; ++i)
            {
                if (source.Ingredients[i].Id == id)
                {
                    return new IngredientsViewModel
                    {
                        Id = source.Ingredients[i].Id,
                        IngredientsName = source.Ingredients[i].IngredientsName
                    };
                }
            }
            throw new Exception("Элемент не найден");
        }

        public void AddElement(IngredientsBindingModel model)
        {
            int maxId = 0;
            for (int i = 0; i < source.Ingredients.Count; ++i)
            {
                if (source.Ingredients[i].Id > maxId)
                {
                    maxId = source.Ingredients[i].Id;
                }
                if (source.Ingredients[i].IngredientsName == model.IngredientsName)
                {
                    throw new Exception("Уже есть компонент с таким названием");
                }
            }
            source.Ingredients.Add(new Ingredients
            {
                Id = maxId + 1,
                IngredientsName = model.IngredientsName
            });
        }

        public void UpdElement(IngredientsBindingModel model)
        {
            int index = -1;
            for (int i = 0; i < source.Ingredients.Count; ++i)
            {
                if (source.Ingredients[i].Id == model.Id)
                {
                    index = i;
                }
                if (source.Ingredients[i].IngredientsName == model.IngredientsName &&
                    source.Ingredients[i].Id != model.Id)
                {
                    throw new Exception("Уже есть компонент с таким названием");
                }
            }
            if (index == -1)
            {
                throw new Exception("Элемент не найден");
            }
            source.Ingredients[index].IngredientsName = model.IngredientsName;
        }

        public void DelElement(int id)
        {
            for (int i = 0; i < source.Ingredients.Count; ++i)
            {
                if (source.Ingredients[i].Id == id)
                {
                    source.Ingredients.RemoveAt(i);
                    return;
                }
            }
            throw new Exception("Элемент не найден");
        }
    }
}
