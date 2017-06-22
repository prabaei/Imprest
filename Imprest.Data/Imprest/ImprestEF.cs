namespace Imprest.Data.Imprest
{
    using System;
    using System.Data.Entity;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;

    public partial class ImprestEF : DbContext
    {
        public ImprestEF()
            : base("name=ImprestEF")
        {
        }

        public virtual DbSet<AccountMaster> AccountMaster { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<AccountMaster>()
                .Property(e => e.AccountNo)
                .HasPrecision(13, 0);
        }
    }
}
