using System;
using InventoryApp.Models;
using Microsoft.EntityFrameworkCore;

namespace InventoryApp.Data
{
	public class InventoryContext : DbContext
	{
		public DbSet<Item> Items { get; set; } = null!;

		protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
		{
	
			var conn = "Server=localhost,1433;Database=InventoryDb;User Id=sa;Password=Your_password123;TrustServerCertificate=True";

			
			optionsBuilder.UseSqlServer(conn, sqlOptions =>
			{
				
				sqlOptions.EnableRetryOnFailure(maxRetryCount: 5, maxRetryDelay: TimeSpan.FromSeconds(10), errorNumbersToAdd: null);
			
				sqlOptions.CommandTimeout(60);
			});
		}

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			modelBuilder.Entity<Item>()
				.HasIndex(i => i.ItemNum)
				.IsUnique();

			modelBuilder.Entity<Item>()
				.Property(i => i.Price)
				.HasColumnType("decimal(18,2)");

			modelBuilder.Entity<Item>()
				.Property(i => i.ItemNum)
				.IsRequired()
				.HasMaxLength(50);

			modelBuilder.Entity<Item>()
				.Property(i => i.Category)
				.HasMaxLength(20);
		}
	}
}
