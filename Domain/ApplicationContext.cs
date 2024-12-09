using Domain.Transactions;
using Microsoft.EntityFrameworkCore;

namespace Data
{
    public class ApplicationContext : DbContext
    {
        private Guid[] clientIds = Enumerable.Range(0, 100).Select(_ => Guid.NewGuid()).ToArray();

        public DbSet<Transaction> Transactions { get; set; }

        public ApplicationContext()
        {
        }

        public ApplicationContext(DbContextOptions<ApplicationContext> options) : base(options) =>
            Database.EnsureCreated();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder
                .UseNpgsql("Host=localhost;Port=5432;Database=database;Username=user;Password=password")
                .UseSeeding((context, _) =>
                {
                    DateTime dateTime = DateTime.UtcNow;
                    for (int i = 0; i < 10; i++)
                    {
                        dateTime = dateTime.AddDays(-1);
                        foreach (Guid clientId in clientIds)
                        {
                            for (int j = 0; j < 00; j++)
                            {
                                context.Set<CreditTransaction>().Add(new CreditTransaction
                                { ClientId = clientId, DateTime = dateTime, Amount = i + j });
                                context.Set<DebitTransaction>().Add(new DebitTransaction
                                { ClientId = clientId, DateTime = dateTime, Amount = i + j });
                                context.SaveChanges();
                            }
                        }
                    }
                });
        }
    }
}
