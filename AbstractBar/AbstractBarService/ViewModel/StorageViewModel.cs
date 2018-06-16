﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AbstractBarService.ViewModel
{
   public class StorageViewModel
    {
        public int Id { get; set; }

        public string StorageName { get; set; }

        public List<StorageIngredientsViewModel> StorageIngredients{ get; set;}
    }
}
