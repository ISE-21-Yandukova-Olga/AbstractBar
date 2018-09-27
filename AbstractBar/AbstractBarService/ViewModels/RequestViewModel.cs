using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace AbstractBarService.ViewModels
{
    [DataContract]
    public class RequestViewModel
    {
        [DataMember]
        public int Id { get; set; }

        [DataMember]
        public int CustomerId { get; set; }

        [DataMember]
        public string CustomerFIO { get; set; }

        [DataMember]
        public int CoctailId { get; set; }

        [DataMember]
        public string CoctailName { get; set; }

        [DataMember]
        public int? BarmenId { get; set; }

        [DataMember]
        public string BarmenName { get; set; }

        [DataMember]
        public int Count { get; set; }

        [DataMember]
        public decimal Sum { get; set; }

        [DataMember]
        public string Status { get; set; }

        [DataMember]
        public string DateCreate { get; set; }

        [DataMember]
        public string DateImplement { get; set; }
    }
}