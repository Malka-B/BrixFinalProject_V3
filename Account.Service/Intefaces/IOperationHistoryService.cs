using Account.Service.Models;
using Account.Share.Models;
using System.Threading.Tasks;

namespace Account.Service.Intefaces
{
    public interface IOperationHistoryService
    {
        PagingReturn GetAll(QueryParameters queryParameters);
    }
}
