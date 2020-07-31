using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using Transaction.Service.Interfaces;
using Transaction.Share.Models;
using Transaction.WebApi.DTO;

namespace Transaction.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TransactionInfoController : ControllerBase
    {
        private readonly ITransactionInfoService _transactionService;
        private readonly IMapper _mapper;
        public TransactionInfoController(ITransactionInfoService transactionInfoService, IMapper mapper)
        {
            _transactionService = transactionInfoService;
            _mapper = mapper;
        }
        [HttpGet("transactionInfo")]
        public async Task<TransactionInfoDTO> GetTransactionInfoAsync([FromQuery] Guid transactionId)
        {
            TransactionInfoModel transactionInfoModel = await _transactionService.GetTransactionInfoAsync(transactionId);
            TransactionInfoDTO transactionInfoDTO = _mapper.Map<TransactionInfoDTO>(transactionInfoModel);
                /*new TransactionInfoDTO()
            {
                Amount = transactionInfoModel.Amount,
                Date = transactionInfoModel.Date,
                FromAccountId = transactionInfoModel.FromAccountId,
                ToAccountId = transactionInfoModel.ToAccountId
            };
            */
            return transactionInfoDTO;
        }
    }
}