using AbstractBarService.BindingModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AbstractBarService.ViewModels;
namespace AbstractBarService.Interfaces
{
    public interface IMainService
    {
        List<RequestViewModel> GetList();

        void CreateRequest(RequestBindingModel model);

        void TakeRequestInWork(RequestBindingModel model);

        void FinishRequest(int id);

        void PayRequest(int id);

        void PutIngredientOnStorage(StorageIngredientBindingModel model);
    }
}