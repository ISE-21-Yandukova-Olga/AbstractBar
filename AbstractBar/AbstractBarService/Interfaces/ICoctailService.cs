using AbstractBarService.BindingModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AbstractBarService.ViewModels;
namespace AbstractBarService.Interfaces
{
    
   public interface ICoctailService
    {
        List<CoctailViewModel> GetList();

        CoctailViewModel GetElement(int id);

        void AddElement(CoctailBindingModel model);

        void UpdElement(CoctailBindingModel model);

        void DelElement(int id);
    }
}