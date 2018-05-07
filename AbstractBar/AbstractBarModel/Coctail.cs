using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AbstractBarModel
{
    public class Coctail
    {
        public int Id { get; set; }
        [Required]
        public string CoctailName { get; set; }
        [Required]
        public decimal Price { get; set; }

        [ForeignKey("Coctail_Id")]
        public virtual List<Request> Requests { get; set; }

        [ForeignKey("Coctail_Id")]
        public virtual List<IngredientsCoctail> IngredientsCoctail { get; set; }
    }
}

