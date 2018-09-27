using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AbstractBarModel
{
    public class CoctailIngredient
    {
        public int Id { get; set; }

        public int CoctailId { get; set; }

        public int IngredientId { get; set; }

        public int Count { get; set; }
    }
}