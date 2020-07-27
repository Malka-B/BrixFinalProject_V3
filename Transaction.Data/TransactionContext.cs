using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using Transaction.Data.Entities;

namespace Transaction.Data
{
    public class TransactionContext: DbContext
    {
        public TransactionContext(DbContextOptions<TransactionContext> options) : base(options)
        {

        }

        public TransactionContext()
        { }

        public DbSet<TransactionEntity> Transactions { get; set; }
    }
}
