using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AbstractBarModel
{
    public class Barmen
    {
        public int Id { get; set; }

        [Required]
        public string BarmenFIO { get; set; }

        [ForeignKey("BarmenId")]
        public virtual List<Request> Requests { get; set; }
    }
}
