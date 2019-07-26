using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace CryptoCognizant.Model
{
    public partial class CryptoCognizantContext : DbContext
    {
        public CryptoCognizantContext()
        {
        }

        public CryptoCognizantContext(DbContextOptions<CryptoCognizantContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Coin> Coin { get; set; }
        public virtual DbSet<Exchange> Exchange { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. See http://go.microsoft.com/fwlink/?LinkId=723263 for guidance on storing connection strings.
                optionsBuilder.UseSqlServer("Server=tcp:cryptocognizant.database.windows.net,1433;Initial Catalog=CryptoCognizant;Persist Security Info=False;User ID=hdnsimpson;Password=ridinG57;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("ProductVersion", "2.2.6-servicing-10079");

            modelBuilder.Entity<Coin>(entity =>
            {
                entity.Property(e => e.CoinSymbol).IsUnicode(false);

                entity.Property(e => e.ImageUrl).IsUnicode(false);
            });

            modelBuilder.Entity<Exchange>(entity =>
            {
                entity.Property(e => e.ExchangeName).IsUnicode(false);

                entity.Property(e => e.Pairs).IsUnicode(false);
            });
        }
    }
}
