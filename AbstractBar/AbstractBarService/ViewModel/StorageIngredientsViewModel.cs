using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AbstractBarService.ViewModel
{
     public class StorageIngredientsViewModel
    {
        public int Id { get; set; }

        public int StorageId { get; set; }

        public int Ingredients_Id { get; set; }

        public string IngredientsName { get; set; }

        public int Count { get; set; }
    }
}
