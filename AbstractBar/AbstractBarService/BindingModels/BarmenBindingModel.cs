using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace AbstractBarService.BindingModels
{

    [DataContract]
    public class BarmenBindingModel
    {
        [DataMember]
        public int Id { get; set; }

        [DataMember]
        public string BarmenFIO { get; set; }
    }
}