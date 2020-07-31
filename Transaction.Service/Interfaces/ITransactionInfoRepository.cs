﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Transaction.Share.Models;

namespace Transaction.Service.Interfaces
{
    public interface ITransactionInfoRepository
    {
        Task<TransactionInfoModel> GetTransactionInfoAsync(Guid transactionId);
    }
}
