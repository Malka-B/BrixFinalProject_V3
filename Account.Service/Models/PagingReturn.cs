using Account.Share.Models;
using System.Collections.Generic;

namespace Account.Service.Models
{
    public class PagingReturn
    {
        public List<HistoryModel> AccountHistory { get; set; }
        public int Count { get; set; }
    }
}
