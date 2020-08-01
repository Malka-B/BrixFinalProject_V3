using Account.Share.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace Account.Share.Models
{
    public class Filter
    {
        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }
        public OperationType OperationType { get; set; }
    }
}
