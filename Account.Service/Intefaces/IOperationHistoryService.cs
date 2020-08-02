using Account.Service.Models;
using Account.Share.Models;

namespace Account.Service.Intefaces
{
    public interface IOperationHistoryService
    {
        PagingReturn GetFilteredInfo(QueryParameters queryParameters);
    }
}
