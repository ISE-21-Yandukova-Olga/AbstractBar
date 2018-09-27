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
            List<BarmenViewModel> result = new List<BarmenViewModel>();
            for (int i = 0; i < source.Barmens.Count; ++i)
            {
                result.Add(new BarmenViewModel
                {
                    Id = source.Barmens[i].Id,
                    BarmenFIO = source.Barmens[i].BarmenFIO
                });
            }
            return result;
        }

        public BarmenViewModel GetElement(int id)
        {
            for (int i = 0; i < source.Barmens.Count; ++i)
            {
                if (source.Barmens[i].Id == id)
                {
                    return new BarmenViewModel
                    {
                        Id = source.Barmens[i].Id,
                        BarmenFIO = source.Barmens[i].BarmenFIO
                    };
                }
            }
            throw new Exception("Элемент не найден");
        }

        public void AddElement(BarmenBindingModel model)
        {
            int maxId = 0;
            for (int i = 0; i < source.Barmens.Count; ++i)
            {
                if (source.Barmens[i].Id > maxId)
                {
                    maxId = source.Barmens[i].Id;
                }
                if (source.Barmens[i].BarmenFIO == model.BarmenFIO)
                {
                    throw new Exception("Уже есть сотрудник с таким ФИО");
                }
            }
            source.Barmens.Add(new Barmen
            {
                Id = maxId + 1,
                BarmenFIO = model.BarmenFIO
            });
        }

        public void UpdElement(BarmenBindingModel model)
        {
            int index = -1;
            for (int i = 0; i < source.Barmens.Count; ++i)
            {
                if (source.Barmens[i].Id == model.Id)
                {
                    index = i;
                }
                if (source.Barmens[i].BarmenFIO == model.BarmenFIO &&
                    source.Barmens[i].Id != model.Id)
                {
                    throw new Exception("Уже есть сотрудник с таким ФИО");
                }
            }
            if (index == -1)
            {
                throw new Exception("Элемент не найден");
            }
            source.Barmens[index].BarmenFIO = model.BarmenFIO;
        }

        public void DelElement(int id)
        {
            for (int i = 0; i < source.Barmens.Count; ++i)
            {
                if (source.Barmens[i].Id == id)
                {
                    source.Barmens.RemoveAt(i);
                    return;
                }
            }
            throw new Exception("Элемент не найден");
        }
    }
}