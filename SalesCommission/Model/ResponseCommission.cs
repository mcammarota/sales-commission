﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SalesCommission.Model
{
    public class ResponseCommission
    {
        public int SellerId { get; set; }
        public int Month { get; set; }
        public double TotalValueMonth { get; set; }
    }
}
