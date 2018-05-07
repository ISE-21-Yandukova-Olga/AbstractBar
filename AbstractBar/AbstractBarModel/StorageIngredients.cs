using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AbstractBarModel
{
    public class StorageIngredients //склад-компонент
    {
        public int Id { get; set; }

        public int StorageId { get; set; }

        public int Ingredients_Id { get; set; }

        public int Count { get; set; }
    }
}
