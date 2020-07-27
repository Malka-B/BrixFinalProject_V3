using AutoMapper;
using Messages.Commands;
using Microsoft.AspNetCore.Mvc;
using NServiceBus;
using System;
using System.Threading.Tasks;
using Tracking.WebApi.DTO;
using Transaction.Service.Interfaces;
using Transaction.Share.Models;

namespace Transaction.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TransactionController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly ITransactionService _transactionService;
        private readonly IMessageSession _messageSession;
        public TransactionController(IMapper mapper, ITransactionService transactionService, IMessageSession messageSession)
        {
            _mapper = mapper;
            _transactionService = transactionService;
            _messageSession = messageSession;
        } 

        [HttpPost("transaction")]
        public async Task<bool> CreateTransactionAsync([FromBody] TransactionDTO transactionDTO)
        {
            TransactionModel transactionModel = _mapper.Map<TransactionModel>(transactionDTO);
            Guid transactionId=await _transactionService.CreateTransactionAsync(transactionModel);
            StartTransaction startTransaction = new StartTransaction()
            {
                Amount = transactionModel.Amount,
                FromAccountId = transactionModel.FromAccountId,
                ToAccountId = transactionModel.ToAccountId,
                TransactionId = transactionId
            };

            await _messageSession.Send(startTransaction)
              .ConfigureAwait(false);
            
            return true;
        }
    }
}
