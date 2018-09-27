using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace AbstractBarService.BindingModels
{
    [DataContract]
    public class CoctailBindingModel
    {
        [DataMember]
        public int Id { get; set; }

        [DataMember]
        public string CoctailName { get; set; }

        [DataMember]
        public decimal Price { get; set; }

        [DataMember]
        public List<CoctailIngredientBindingModel> CoctailIngredients { get; set; }
    }
}