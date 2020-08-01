using Account.Service.Models;
using Account.Share.Models;

namespace Account.Service.Intefaces
{
    public interface IOperationHistoryService
    {
        PagingReturn GetAll(QueryParameters queryParameters);
        PagingReturn GetFilteredInfo(QueryParameters queryParameters);
    }
}
