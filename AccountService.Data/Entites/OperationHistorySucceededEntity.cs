﻿using System;

namespace Account.Data.Entites
{
    public class OperationHistorySucceededEntity
    {
        public Guid Id { get; set; }
        public Guid AccountId { get; set; }
        public Guid TransactionId { get; set; }
        public bool operationType { get; set; }
        public int TransactionAmount { get; set; }
        public int Balance { get; set; }
        public DateTime Date { get; set; }

        public void FillFields(Guid accountId, bool operationType, int balance)
        {
            this.Id = new Guid();
            this.AccountId = accountId;
            this.operationType = operationType;
            this.Balance = balance;
        }
    }
}
