using Account.Share.Enums;
using System;

namespace Account.Data.Entites
{
    public class OperationHistoryFailedEntity
    {
        public Guid Id { get; set; }
        public Guid AccountId { get; set; }
        public Guid TransactionId { get; set; }
        public OperationType operationType { get; set; }
        public int TransactionAmount { get; set; }
        public int Balance { get; set; }
        public DateTime Date { get; set; }
        public string FailureReason { get; set; }

        public void FillFields(Guid accountId, OperationType operationType)
        {
            this.Id = new Guid();
            this.AccountId = accountId;
            this.operationType = operationType;            
        }
    }


}
