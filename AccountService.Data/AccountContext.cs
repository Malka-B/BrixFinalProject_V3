using Account.Data.Entites;
using Microsoft.EntityFrameworkCore;

namespace Account.Data
{
    public class AccountContext:DbContext
    {
        public AccountContext(DbContextOptions<AccountContext> options) : base(options)
        {

        }

        public AccountContext()
        { }

        public DbSet<AccountEntity> Accounts { get; set; }
        public DbSet<CustomerEntity> Customers { get; set; }       
    }
}
