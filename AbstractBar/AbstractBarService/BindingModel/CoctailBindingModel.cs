using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AbstractBarService.BindingModel //список компонентов
{
   public class CoctailBindingModel
    {
        public int Id { get; set; }

        public string CoctailName { get; set; }

        public decimal Price { get; set; }

        public List<CoctailIngredientsBindingModel> CoctailIngredients { get; set; }
    }
}
