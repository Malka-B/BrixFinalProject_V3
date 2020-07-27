using Microsoft.EntityFrameworkCore;
using Transaction.Data.Entities;

namespace Transaction.Data
{
    public class TransactionContext: DbContext
    {
        public TransactionContext(DbContextOptions<TransactionContext> options) : base(options)
        { }

        public TransactionContext()
        { }

        public DbSet<TransactionEntity> Transactions { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }
    }
}
