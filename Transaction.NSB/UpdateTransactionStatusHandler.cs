﻿using Messages.Commands;
using NServiceBus;
using System.Threading.Tasks;
using Transaction.Share.Interfaces;

namespace Transaction.NSB
{
    public class UpdateTransactionStatusHandler : IHandleMessages<UpdateTransactionStatus>
    {
        private readonly ITransactionRepository _transactionRepository;

        public UpdateTransactionStatusHandler(ITransactionRepository transactionRepository)
        {
            _transactionRepository = transactionRepository;
        }

        public async Task Handle(UpdateTransactionStatus message, IMessageHandlerContext context)
        {
            await _transactionRepository.UpdateTransactionStatusAsync(message); 
            await Task.CompletedTask;
        }
    }
}
