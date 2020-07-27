using AutoMapper;
using Messages.Commands;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;
using Transaction.Data.Entities;
using Transaction.Share.Interfaces;
using Transaction.Share.Models;

namespace Transaction.Data
{
    public class TransactionRepository : ITransactionRepository
    {
        private readonly IMapper _mapper;
        private readonly TransactionContext _transactionContext;
        public TransactionRepository(IMapper mapper, TransactionContext transactionContext)
        {
            _mapper = mapper;
            _transactionContext = transactionContext;
        }
        public async Task<Guid> CreateTransactionAsync(TransactionModel transactionModel)
        {
            TransactionEntity transactionEntity = _mapper.Map<TransactionEntity>(transactionModel);
            try
            {
                await _transactionContext.Transactions.AddAsync(transactionEntity);
                await _transactionContext.SaveChangesAsync();
                return transactionEntity.Id;
            }
            catch (Exception)
            {
                throw new SystemException();
            }
        }

        public async Task UpdateTransactionStatusAsync(UpdateTransactionStatus message)
        {
            TransactionEntity transactionEntity = await _transactionContext.Transactions
                .FirstOrDefaultAsync(t => t.Id == message.TransactionId);
            if (message.IsTransactionSucceeded == false)
            {
                transactionEntity.Status = eStatus.fail;
                transactionEntity.FailureReason = message.FailureReason;
            }
            else
            {
                transactionEntity.Status = eStatus.success;
            }
            await _transactionContext.SaveChangesAsync();
        }
    }
}
