using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AbstractBarService.ViewModel
{
    public class CoctailIngredientsViewModel
    {
        public int Id { get; set; }

        public int Coctail_Id { get; set; }

        public int Ingredients_Id { get; set; }

        public string IngredientsName { get; set; }

        public int Count { get; set; }
    }
}
