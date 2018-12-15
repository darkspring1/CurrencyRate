using CurrencyRate.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace CurrencyRate.Domain.Persistent.Ef
{
    public class DataContext : DbContext
    {
        public DataContext(string connectionString)
            : base(new DbContextOptionsBuilder<DataContext>().UseNpgsql(connectionString).Options)
        {

        }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);


            modelBuilder.Entity<Rate>()
                .ToTable("public", "rates");
        }

    }
}
