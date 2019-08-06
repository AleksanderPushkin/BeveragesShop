using System.Collections.Generic;

namespace BeveragesShop.Models
{
    public partial class BeveragesShopContext : DbContext
    {
        public BeveragesShopContext()
        {
        }

        public BeveragesShopContext(DbContextOptions<BeveragesShopContext> options)
            : base(options)
        {
        }

        public virtual ISet<Beverages> Beverages { get; set; }
        public virtual DbSet<Coins> Coins { get; set; }
        public virtual DbSet<Payments> Payments { get; set; }
        public virtual DbSet<Producers> Producers { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#pragma warning disable CS1030 // #warning directive
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. See http://go.microsoft.com/fwlink/?LinkId=723263 for guidance on storing connection strings.
                optionsBuilder.UseSqlServer("Data Source=(LocalDB)\\MSSQLLocalDB;AttachDbFilename=|DataDirectory|\\BeveragesShop.mdf;Integrated Security=True");
#pragma warning restore CS1030 // #warning directive
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("ProductVersion", "2.2.4-servicing-10062");

            modelBuilder.Entity<Beverages>(entity =>
            {
                entity.Property(e => e.Category).HasMaxLength(50);

                entity.Property(e => e.Description).HasMaxLength(255);

                entity.Property(e => e.Image).HasColumnType("image");

                entity.Property(e => e.Price);

                entity.Property(e => e.Qty);

                entity.Property(e => e.Title)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.HasOne(d => d.Producer)
                    .WithMany(p => p.Beverages)
                    .HasForeignKey(d => d.ProducerId)
                    .HasConstraintName("FK_Beverages_Producers");
            });

            modelBuilder.Entity<Coins>(entity =>
            {
                entity.Property(e => e.Title)
                    .IsRequired()
                    .HasMaxLength(50);
            });

            modelBuilder.Entity<Payments>(entity =>
            {
                entity.Property(e => e.BeverageId).HasColumnName("BeverageID");
            });

            modelBuilder.Entity<Producers>(entity =>
            {
                entity.Property(e => e.Title).HasMaxLength(50);
            });
        }
    }
}
