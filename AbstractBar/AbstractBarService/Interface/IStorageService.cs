using AbstractBarService.BindingModel;
using AbstractBarService.ViewModel;
using System.Collections.Generic;


namespace AbstractBarService.Interface
{
   public  interface IStorageService
    {
        List<StorageViewModel> GetList();

        StorageViewModel GetElement(int id);

        void AddElement(StorageBindingModel model);

        void UpdElement(StorageBindingModel model);

        void DelElement(int id);
    }
}
