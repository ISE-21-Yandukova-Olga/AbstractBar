using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AbstractBarModel
{
    public class StorageIngredient
    {
        public int Id { get; set; }

        public int StorageId { get; set; }

        public int IngredientId { get; set; }

        public int Count { get; set; }
    }
}