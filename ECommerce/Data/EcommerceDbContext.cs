using ECommerce.Entities;
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

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ProductCategoryEntity>(entity =>
            {
                entity.Property(e => e.Name).IsRequired();
                entity.Property(e => e.Link).IsRequired();
                entity.Property(e => e.Type).IsRequired()
                        .HasConversion(
                        c=> c.ToString(),
                        c => (ProductCategoryType)Enum.Parse(typeof(ProductCategoryType), c));
                 });

            modelBuilder.Entity<ProductEntity>(entity =>
            {
                entity.HasIndex(e => e.CategoryId);

                entity.Property(e => e.Name).IsRequired();

                entity.Property(e => e.CategoryId).IsRequired();

                entity.Property(e => e.Image).IsRequired();

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