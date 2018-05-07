
using AbstractBarService.BindingModel;
using AbstractBarService.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AbstractBarService.Interface
{
   public  interface IBarmenService
    {
        List<BarmenViewModel> GetList();

        BarmenViewModel GetElement(int id);

        void AddElement(BarmenBindingModel model);

        void UpdElement(BarmenBindingModel model);

        void DelElement(int id);
    }
}
