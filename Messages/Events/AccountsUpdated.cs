using System;

namespace Messages.Events
{
    public class AccountsUpdated
    {
        public Guid TransactionId { get; set; }
        public bool isAccountsUpdateSuccess { get; set; }
        public string FailureReason { get; set; }

        public AccountsUpdated()
        {

        }

        public AccountsUpdated(bool isAccountsUpdateSuccess)
        {
            this.isAccountsUpdateSuccess = isAccountsUpdateSuccess;
        }
    }   
}
