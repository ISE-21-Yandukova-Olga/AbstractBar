
using AbstractBarService.BindingModel;
using AbstractBarService.Interface;
using System.Collections.Generic;


namespace AbstractBarService.Interface
{
    public interface IIngredientsService
    {
        List<IngredientsViewModel> GetList();

        IngredientsViewModel GetElement(int id);

        void AddElement(IngredientsBindingModel model);

        void UpdElement(IngredientsBindingModel model);

        void DelElement(int id);
    }
}
