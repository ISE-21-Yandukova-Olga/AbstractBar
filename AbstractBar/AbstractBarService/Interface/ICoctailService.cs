using AbstractBarService.BindingModel;
using AbstractBarService.ViewModel;


using System.Collections.Generic;


namespace AbstractBarService.Interface
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
