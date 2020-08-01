using System;

namespace Account.Share.Models
{
    public class QueryParameters
    {
        private const int maxPageCount = 50;
        public int Page { get; set; } = 1;
        private int _pageCount = maxPageCount;
        public int PageCount
        {
            get { return _pageCount; }
            set { _pageCount = (value > maxPageCount) ? maxPageCount : value; }
        }
        public Filter Query { get; set; }
        public string OrderBy { get; set; } = "Date";
        public Guid AccountId { get; set; }
    }
}
