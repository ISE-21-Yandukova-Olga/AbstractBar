﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AbstractBarService.ViewModels
{
    public class CustomerRequestsModel
    {
        public string CustomerName { get; set; }

        public string DateCreate { get; set; }

        public string CoctailName { get; set; }

        public int Count { get; set; }

        public decimal Sum { get; set; }

        public string Status { get; set; }
    }
}