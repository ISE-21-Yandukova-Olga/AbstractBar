using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AbstractBarService.BindingModels
{
    public class RequestBindingModel
    {
        public int Id { get; set; }

        public int CustomerId { get; set; }

        public int CoctailId { get; set; }

        public int? BarmenId { get; set; }

        public int Count { get; set; }

        public decimal Sum { get; set; }
    }
}