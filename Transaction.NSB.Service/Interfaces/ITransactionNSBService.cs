using Messages.Commands;
using System.Threading.Tasks;

namespace Transaction.NSB.Service.Interfaces
{
    public interface ITransactionNSBService
    {
        Task UpdateTransactionStatus(UpdateTransactionStatus message);
    }
}
