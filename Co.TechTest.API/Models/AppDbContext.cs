using Microsoft.EntityFrameworkCore;

namespace Co.TechTest.API
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {

        }

        public DbSet<Merchant> Merchants { get; set; }
        public DbSet<Payment> Payments { get; set; }
        public DbSet<PaymentDetails> PaymentDetails { get; set; }
        public DbSet<Address> Addresses { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<Payment>()
                .HasOne(p => p.Merchant)
                .WithMany(m => m.Payments)
                .HasForeignKey(p => p.MerchantId)
                .HasPrincipalKey(m => m.Id);

            builder.Entity<Payment>()
                .HasOne(p => p.PaymentDetails)
                .WithOne(pd => pd.Payment);

            builder.Entity<PaymentDetails>()
                .HasOne(pd => pd.Address)
                .WithOne(a => a.PaymentDetails);
        }
    }
}
