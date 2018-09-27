using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AbstractBarService.ViewModels
{
    public class CoctailViewModel
    {
        public int Id { get; set; }

        public string CoctailName { get; set; }

        public decimal Price { get; set; }

        public List<CoctailIngredientViewModel> CoctailIngredients { get; set; }
    }
}