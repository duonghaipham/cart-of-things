using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

#nullable disable

namespace customer.Models
{
    public partial class ShopContext : DbContext
    {
        public ShopContext()
        {
        }

        public ShopContext(DbContextOptions<ShopContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Account> Accounts { get; set; }
        public virtual DbSet<Cart> Carts { get; set; }
        public virtual DbSet<Category> Categories { get; set; }
        public virtual DbSet<Comment> Comments { get; set; }
        public virtual DbSet<Order> Orders { get; set; }
        public virtual DbSet<Payment> Payments { get; set; }
        public virtual DbSet<Place> Places { get; set; }
        public virtual DbSet<Product> Products { get; set; }
        public virtual DbSet<State> States { get; set; }
        public virtual DbSet<Ware> Wares { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer("Server=localhost;Database=Shop;Trusted_Connection=True;MultipleActiveResultSets=true");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("Relational:Collation", "SQL_Latin1_General_CP1_CI_AS");

            modelBuilder.Entity<Account>(entity =>
            {
                entity.ToTable("Account");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Avatar)
                    .HasMaxLength(200)
                    .HasColumnName("avatar");

                entity.Property(e => e.Email)
                    .IsRequired()
                    .HasMaxLength(200)
                    .HasColumnName("email");

                entity.Property(e => e.IdPlace).HasColumnName("id_place");

                entity.Property(e => e.IdState).HasColumnName("id_state");
                
                entity.Property(e => e.Lock).HasColumnName("lock");
                entity.Property(e => e.Lock).HasColumnName("lock");

                entity.Property(e => e.IdentityCard)
                    .IsRequired()
                    .HasMaxLength(200)
                    .HasColumnName("identity_card");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(200)
                    .HasColumnName("name");

                entity.Property(e => e.Password)
                    .IsRequired()
                    .HasMaxLength(200)
                    .HasColumnName("password");

                entity.Property(e => e.Role)
                    .IsRequired()
                    .HasMaxLength(200)
                    .HasColumnName("role");
            });

            modelBuilder.Entity<Cart>(entity =>
            {
                entity.ToTable("Cart");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Amount).HasColumnName("amount");

                entity.Property(e => e.IdAccount).HasColumnName("id_account");

                entity.Property(e => e.IdProduct).HasColumnName("id_product");
            });

            modelBuilder.Entity<Category>(entity =>
            {
                entity.ToTable("Category");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Avatar)
                    .HasMaxLength(200)
                    .HasColumnName("avatar");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(200)
                    .HasColumnName("name");
            });

            modelBuilder.Entity<Comment>(entity =>
            {
                entity.ToTable("Comment");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Content)
                    .IsRequired()
                    .HasMaxLength(200)
                    .HasColumnName("content");

                entity.Property(e => e.CreatedAt)
                    .HasColumnType("datetime")
                    .HasColumnName("created_at");

                entity.Property(e => e.IdAccount).HasColumnName("id_account");

                entity.Property(e => e.IdProduct).HasColumnName("id_product");
            });

            modelBuilder.Entity<Order>(entity =>
            {
                entity.ToTable("Order");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Address)
                    .IsRequired()
                    .HasMaxLength(200)
                    .HasColumnName("address");

                entity.Property(e => e.CreatedAt)
                    .HasColumnType("datetime")
                    .HasColumnName("created_at");

                entity.Property(e => e.IdAccount).HasColumnName("id_account");

                entity.Property(e => e.IdPayment).HasColumnName("id_payment");

                entity.Property(e => e.Note)
                    .IsRequired()
                    .HasMaxLength(200)
                    .HasColumnName("note");

                entity.Property(e => e.Phone)
                    .IsRequired()
                    .HasMaxLength(200)
                    .HasColumnName("phone");

                entity.Property(e => e.RecipientName)
                    .IsRequired()
                    .HasMaxLength(200)
                    .HasColumnName("recipient_name");
            });

            modelBuilder.Entity<Payment>(entity =>
            {
                entity.ToTable("Payment");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Cost).HasColumnName("cost");

                entity.Property(e => e.Details)
                    .IsRequired()
                    .HasMaxLength(200)
                    .HasColumnName("details");

                entity.Property(e => e.State).HasColumnName("state");

                entity.Property(e => e.Time)
                    .HasColumnType("datetime")
                    .HasColumnName("time");
            });

            modelBuilder.Entity<Place>(entity =>
            {
                entity.ToTable("Place");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Address)
                    .IsRequired()
                    .HasMaxLength(200)
                    .HasColumnName("address");

                entity.Property(e => e.NumberStaff).HasColumnName("number_staff");
            });

            modelBuilder.Entity<Product>(entity =>
            {
                entity.ToTable("Product");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Avatar)
                    .HasMaxLength(200)
                    .HasColumnName("avatar");

                entity.Property(e => e.Description)
                    .HasMaxLength(200)
                    .HasColumnName("description");

                entity.Property(e => e.IdCategory).HasColumnName("id_category");

                entity.Property(e => e.Introduction)
                    .HasMaxLength(200)
                    .HasColumnName("introduction");

                entity.Property(e => e.IsArchived).HasColumnName("is_archived");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(200)
                    .HasColumnName("name");

                entity.Property(e => e.Price).HasColumnName("price");
            });

            modelBuilder.Entity<State>(entity =>
            {
                entity.ToTable("State");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.MinuteLimit).HasColumnName("minute_limit");

                entity.Property(e => e.Token)
                    .IsRequired()
                    .HasMaxLength(200)
                    .HasColumnName("token");
            });

            modelBuilder.Entity<Ware>(entity =>
            {
                entity.ToTable("Ware");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Amount).HasColumnName("amount");

                entity.Property(e => e.IdOrder).HasColumnName("id_order");

                entity.Property(e => e.IdProduct).HasColumnName("id_product");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
