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
    public class BarmenServiceBD : IBarmenService
    {
        private AbstractDbContext context;

        public BarmenServiceBD(AbstractDbContext context)
        {
            this.context = context;
        }

        public List<BarmenViewModel> GetList()
        {
            List<BarmenViewModel> result = context.Barmens
                .Select(rec => new BarmenViewModel
                {
                    Id = rec.Id,
                    BarmenFIO = rec.BarmenFIO
                })
                .ToList();
            return result;
        }

        public BarmenViewModel GetElement(int id)
        {
            Barmen element = context.Barmens.FirstOrDefault(rec => rec.Id == id);
            if (element != null)
            {
                return new BarmenViewModel
                {
                    Id = element.Id,
                    BarmenFIO = element.BarmenFIO
                };
            }
            throw new Exception("Элемент не найден");
        }

        public void AddElement(BarmenBindingModel model)
        {
            Barmen element = context.Barmens.FirstOrDefault(rec => rec.BarmenFIO == model.BarmenFIO);
            if (element != null)
            {
                throw new Exception("Уже есть сотрудник с таким ФИО");
            }
            context.Barmens.Add(new Barmen
            {
                BarmenFIO = model.BarmenFIO
            });
            context.SaveChanges();
        }

        public void UpdElement(BarmenBindingModel model)
        {
            Barmen element = context.Barmens.FirstOrDefault(rec =>
                                        rec.BarmenFIO == model.BarmenFIO && rec.Id != model.Id);
            if (element != null)
            {
                throw new Exception("Уже есть сотрудник с таким ФИО");
            }
            element = context.Barmens.FirstOrDefault(rec => rec.Id == model.Id);
            if (element == null)
            {
                throw new Exception("Элемент не найден");
            }
            element.BarmenFIO = model.BarmenFIO;
            context.SaveChanges();
        }

        public void DelElement(int id)
        {
            Barmen element = context.Barmens.FirstOrDefault(rec => rec.Id == id);
            if (element != null)
            {
                context.Barmens.Remove(element);
                context.SaveChanges();
            }
            else
            {
                throw new Exception("Элемент не найден");
            }
        }
    }
}