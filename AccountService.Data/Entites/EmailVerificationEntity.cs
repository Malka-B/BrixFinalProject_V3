using System;

namespace Account.Data.Entites
{
    public class EmailVerificationEntity
    {
        public Guid Id { get; set; }
        public string Email { get; set; }
        public string VerificationCode { get; set; }
        public DateTime ExpirationTime { get; set; }
    }
}
