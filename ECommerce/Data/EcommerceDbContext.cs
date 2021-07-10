using ECommerce.Entities;
using ECommerce.Models;
using Microsoft.EntityFrameworkCore;
using System;
using static ECommerce.Models.ProductCategoryEnum;

namespace ECommerce.Data
{
    public class EcommerceDbContext : DbContext
    {
        public EcommerceDbContext(DbContextOptions<EcommerceDbContext> options) : base(options)
        {
        }

        public DbSet<ProductEntity> Products { get; set; }

        public DbSet<ProductCategoryEntity> ProductCategories { get; set; }

        public DbSet<NewsEntity> News { get; set; }

        public DbSet<AttachmentEntity> Attachments { get; set; }

        public DbSet<OrderEntity> Orders { get; set; }

        public DbSet<OrderDetailEntity> OrderDetails { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ProductCategoryEntity>(entity =>
            {
                entity.Property(e => e.Name).IsRequired();
                entity.Property(e => e.Link).IsRequired();
                entity.Property(e => e.Type).IsRequired()
                        .HasConversion(
                        c => c.ToString(),
                        c => (ProductCategoryType)Enum.Parse(typeof(ProductCategoryType), c));
            });

            modelBuilder.Entity<AttachmentEntity>(entity =>
            {
                entity.Property(e => e.ProductId).IsRequired();
                entity.Property(e => e.Image).IsRequired();
                entity.Property(e => e.RefId).IsRequired()
                        .HasConversion(
                        c => c.ToString(),
                        c => (AttachmentRefEnum.RefId)Enum.Parse(typeof(AttachmentRefEnum.RefId), c));
            });

            modelBuilder.Entity<OrderEntity>(entity => {
                entity.Property(o => o.ShipName).IsRequired();
                entity.Property(o => o.ShipPhoneNumber).IsRequired();
                entity.Property(o => o.ShipAddress).IsRequired();
                entity.Property(e => e.Status).IsRequired()
                        .HasConversion(
                        c => c.ToString(),
                        c => (OrderStatus)Enum.Parse(typeof(OrderStatus), c));

            });

            modelBuilder.Entity<OrderDetailEntity>(entity => {
                entity.Property(e => e.ProductId).IsRequired();
                entity.Property(e => e.OrderId).IsRequired();
                entity.Property(e => e.Quantity).IsRequired();
                entity.HasOne(od => od.Product)
                    .WithMany(p => p.OrderDetails)
                    .HasForeignKey(od => od.ProductId);
                entity.HasOne(od => od.Order)
                .WithMany(o => o.OrderDetails)
                .HasForeignKey(od => od.OrderId);
                entity.Property(e => e.Price).IsRequired()
        .HasColumnType("decimal(18,2)");
            });

            modelBuilder.Entity<ProductEntity>(entity =>
            {
                entity.HasIndex(e => e.CategoryId);

                entity.Property(e => e.Name).IsRequired();

                entity.Property(e => e.CategoryId).IsRequired();

                entity.Property(e => e.Price).IsRequired()
                .HasColumnType("decimal(18,2)");

                entity.Property(e => e.DiscountPrice)
                    .HasColumnType("decimal(18,2)");

                entity.HasOne(d => d.Category)
                    .WithMany(p => p.Products)
                    .HasForeignKey(d => d.CategoryId);
            });

            modelBuilder.Entity<NewsEntity>(entity =>
            {
                entity.Property(e => e.Title).IsRequired();
                entity.Property(e => e.Content).IsRequired();
                entity.Property(e => e.Image).IsRequired();
            });
        }
    }
}