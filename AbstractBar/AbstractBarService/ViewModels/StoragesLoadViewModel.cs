using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace AbstractBarService.ViewModels
{
    [DataContract]
    public class StoragesLoadViewModel
    {
        [DataMember]
        public string StorageName { get; set; }

        [DataMember]
        public int TotalCount { get; set; }

        [DataMember]
        public List<StorageIngredientLoadViewModel> Ingredients { get; set; }
    }

    [DataContract]
    public class StorageIngredientLoadViewModel
    {
        [DataMember]
        public string IngredientName { get; set; }

        [DataMember]
        public int Count { get; set; }
    }
}