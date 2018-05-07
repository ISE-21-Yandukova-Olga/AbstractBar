using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AbstractBarModel
{
    public class Request //заказ (запрос, заявка)
    {
        public int Id { get; set; }

        public int CustomerId { get; set; }

        public int Coctail_Id { get; set; }

        public int? BarmenId { get; set; }

        public int Count { get; set; }

        public decimal Sum { get; set; }

        public RequestStatus Status { get; set; }

        public DateTime DateCreate { get; set; }

        public DateTime? DateBarmen { get; set; }
    }
}
