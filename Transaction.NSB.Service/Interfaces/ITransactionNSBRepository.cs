using Messages.Commands;
using System.Threading.Tasks;

namespace Transaction.NSB.Service.Interfaces
{
    public interface ITransactionNSBRepository
    {
        Task UpdateTransactionStatus(UpdateTransactionStatus message);
    }
}
