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
    public class BarmenServiceList : IBarmenService
    {
        private DataListSingleton source;

        public BarmenServiceList()
        {
            source = DataListSingleton.GetInstance();
        }

        public List<BarmenViewModel> GetList()
        {
            List<BarmenViewModel> result = source.Barmens
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
            Barmen element = source.Barmens.FirstOrDefault(rec => rec.Id == id);
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
            Barmen element = source.Barmens.FirstOrDefault(rec => rec.BarmenFIO == model.BarmenFIO);
            if (element != null)
            {
                throw new Exception("Уже есть сотрудник с таким ФИО");
            }
            int maxId = source.Barmens.Count > 0 ? source.Barmens.Max(rec => rec.Id) : 0;
            source.Barmens.Add(new Barmen
            {
                Id = maxId + 1,
                BarmenFIO = model.BarmenFIO
            });
        }

        public void UpdElement(BarmenBindingModel model)
        {
            Barmen element = source.Barmens.FirstOrDefault(rec =>
                                        rec.BarmenFIO == model.BarmenFIO && rec.Id != model.Id);
            if (element != null)
            {
                throw new Exception("Уже есть сотрудник с таким ФИО");
            }
            element = source.Barmens.FirstOrDefault(rec => rec.Id == model.Id);
            if (element == null)
            {
                throw new Exception("Элемент не найден");
            }
            element.BarmenFIO = model.BarmenFIO;
        }

        public void DelElement(int id)
        {
            Barmen element = source.Barmens.FirstOrDefault(rec => rec.Id == id);
            if (element != null)
            {
                source.Barmens.Remove(element);
            }
            else
            {
                throw new Exception("Элемент не найден");
            }
        }
    }
}