using AbstractBarService.BindingModel;
using AbstractBarService.ViewModel;
using System;
using System.Collections.Generic;


namespace AbstractBarService.Interface
{
    public interface IMainService
    {
        List<RequestViewModel> GetList();

        void CreateRequest(RequestBindingModel model);

        void TakeRequestInWork(RequestBindingModel model);

        void FinishRequest(int id);

        void PayRequest(int id);

        void PutComponentOnStorage(StorageIngredientsBindingModel model);
    }
}
