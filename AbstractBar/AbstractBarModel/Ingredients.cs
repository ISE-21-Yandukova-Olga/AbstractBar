using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AbstractBarModel
{
    public class Ingredients
    {
        public int Id { get; set; }
        [Required]
        public string IngredientsName { get; set; }
        [ForeignKey("Ingredients_Id")]
        public virtual List<IngredientsCoctail> IngredientsCoctail { get; set; }

        [ForeignKey("Ingredients_Id")]
        public virtual List<StorageIngredients> StorageIngredients { get; set; }

    }
}
