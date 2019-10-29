using Microsoft.EntityFrameworkCore;
using Shop.Domain;
using System;
using System.Collections.Generic;
using System.Text;

namespace Shop.DataAccess
{
	public class ShopContext : DbContext
	{
		private readonly string connectionString;

		public ShopContext(string connectionString)
		{
			this.connectionString = connectionString;
			Database.EnsureCreated();
		}

		public DbSet<Category> Categories { get; set; }
		public DbSet<User> Users { get; set; }
		public DbSet<Item> Items { get; set; }
		public DbSet<Review> Reviews { get; set; }
		public DbSet<Purchase> Purchases { get; set; }


		protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
		{
			optionsBuilder.UseSqlServer(connectionString);
		}
	}
}
