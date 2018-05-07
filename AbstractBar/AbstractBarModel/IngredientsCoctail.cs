using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AbstractBarModel
{
    public class IngredientsCoctail
    {
        public int Id { get; set; }

        public int Coctail_Id { get; set; }

        public int Ingredients_Id { get; set; }

        public int Count { get; set; }
        public virtual Coctail Coctail { get; set; }

        public virtual Ingredients Ingredients { get; set; }

    }
}
