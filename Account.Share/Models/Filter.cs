using System;
using System.Collections.Generic;
using System.Text;

namespace Account.Share.Models
{
    public class Filter
    {
        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }
        public bool OperationType { get; set; }
    }
}
