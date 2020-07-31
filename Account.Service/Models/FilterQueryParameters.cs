using System;
using System.Collections.Generic;
using System.Text;

namespace Account.Service.Models
{
    public class FilterQueryParameters
    {
        private const int maxPageCount = 50;
        public int Page { get; set; } = 1;
        private int _pageCount = maxPageCount;
        public int PageCount
        {
            get { return _pageCount; }
            set { _pageCount = (value > maxPageCount) ? maxPageCount : value; }
        }
        public string Query { get; set; }
        public string OrderBy { get; set; } = "Date";
        public Guid AccountId { get; set; }
        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }
        public bool OperationType { get; set; }
    }
}

