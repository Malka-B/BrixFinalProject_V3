using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;
using Transaction.Data.Entities;
using Transaction.Service.Interfaces;
using Transaction.Share.Models;

namespace Transaction.Data
{
    public class TransactionInfoRepository : ITransactionInfoRepository
    {
        private readonly TransactionContext _context;
        private readonly IMapper _mapper;
        public TransactionInfoRepository(TransactionContext transactionContext, IMapper mapper)
        {
            _context = transactionContext;
            _mapper = mapper;
        }

        public async Task<TransactionInfoModel> GetTransactionInfoAsync(Guid transactionId)
        {
            TransactionEntity transactionEntity = await _context.Transactions
                .FirstOrDefaultAsync(t => t.Id == transactionId);
            return _mapper.Map<TransactionInfoModel>(transactionEntity);
        }
    }
}
