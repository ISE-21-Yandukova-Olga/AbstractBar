using AbstractBarModel;
using AbstractBarService.BindingModel;
using AbstractBarService.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AbstractBarService.ImplementationsBD
{
    public class IngredientsServiceBD : IIngredientsService
    {
        private AbstractDbContext context_;

        public IngredientsServiceBD(AbstractDbContext context_)
        {
            this.context_ = context_;
        }

        public List<IngredientsViewModel> GetList()
        {
            List<IngredientsViewModel> result = context_.Ingredients
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
            Ingredients element = context_.Ingredients.FirstOrDefault(rec => rec.Id == id);
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
            Ingredients element = context_.Ingredients.FirstOrDefault(rec => rec.IngredientsName == model.IngredientsName);
            if (element != null)
            {
                throw new Exception("Уже есть компонент с таким названием");
            }
            context_.Ingredients.Add(new Ingredients
            {
                IngredientsName = model.IngredientsName
            });
            context_.SaveChanges();
        }

        public void UpdElement(IngredientsBindingModel model)
        {
            Ingredients element = context_.Ingredients.FirstOrDefault(rec =>
                                       rec.IngredientsName == model.IngredientsName && rec.Id != model.Id);
            if (element != null)
            {
                throw new Exception("Уже есть компонент с таким названием");
            }
            element = context_.Ingredients.FirstOrDefault(rec => rec.Id == model.Id);
            if (element == null)
            {
                throw new Exception("Элемент не найден");
            }
            element.IngredientsName = model.IngredientsName;
            context_.SaveChanges();
        }

        public void DelElement(int id)
        {
            Ingredients element = context_.Ingredients.FirstOrDefault(rec => rec.Id == id);
            if (element != null)
            {
                context_.Ingredients.Remove(element);
                context_.SaveChanges();
            }
            else
            {
                throw new Exception("Элемент не найден");
            }
        }
    }
}

