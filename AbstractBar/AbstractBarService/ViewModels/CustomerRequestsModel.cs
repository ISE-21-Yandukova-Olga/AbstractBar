using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace AbstractBarService.ViewModels
{
    [DataContract]
    public class CustomerRequestsModel
    {
        [DataMember]
        public string CustomerName { get; set; }

        [DataMember]
        public string DateCreate { get; set; }

        [DataMember]
        public string CoctailName { get; set; }

        [DataMember]
        public int Count { get; set; }

        [DataMember]
        public decimal Sum { get; set; }

        [DataMember]
        public string Status { get; set; }
    }
}