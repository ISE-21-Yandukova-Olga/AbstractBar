﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace AbstractBarService.BindingModels
{

    [DataContract]
    public class CoctailIngredientBindingModel
    {
        [DataMember]
        public int Id { get; set; }

        [DataMember]
        public int CoctailId { get; set; }

        [DataMember]
        public int IngredientId { get; set; }

        [DataMember]
        public int Count { get; set; }
    }
}