using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AbstractBarModel
{
    public class Ingredient
    {
        public int Id { get; set; }

        [Required]
        public string IngredientName { get; set; }

        [ForeignKey("IngredientId")]
        public virtual List<CoctailIngredient> CoctailIngredients { get; set; }

        [ForeignKey("IngredientId")]
        public virtual List<StorageIngredient> StorageIngredients { get; set; }
    }
}