﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Account.WebApi.DTO
{
    public class HistoryDTO
    {         
        public Guid AccountId { get; set; }        
        public bool operationType { get; set; }
        public int Amount { get; set; }
        public int Balance { get; set; }
        public DateTime Date { get; set; }
    }
}
