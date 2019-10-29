using Dapper;
using System.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Shop.Domain.Repository
{
	public class PurchaseRepository
	{
		private readonly string connectionString;

		public PurchaseRepository(string connectionString)
		{
			this.connectionString = connectionString;
		}

		public void Add(Purchase purchase)
		{
			var sql = "Insert into Purchases (Id, CreationDate, ItemId, UserId) Values (@Id, @CreationDate, @ItemId, @UserId);";

			using (var connection = new SqlConnection(connectionString))
			{
				var rowAffected = connection.Execute(sql, purchase);
				if (rowAffected != 1)															// так как вставка всего на 1 строку
				{
					throw new Exception("Что-то пошло не так");
				}
			}
		}

		public int BoughtItemsByUser(User user, Guid itemId)
		{
			var sql = "Select * From Purchases " +
				"WHERE UserId = @UserId AND ItemId = @ItemId";

			using (var connection = new SqlConnection(connectionString))
			{
				return connection.Query<Purchase>(sql, new {ItemId = itemId, UserId = user.Id}).ToList().Count;
			}
		}

		public ICollection<Purchase> GetAll(Guid UserId)
		{
			var sql = "Select * From Purchases " +
				"WHERE UserId = @UserId " +
				"Order by CreationDate";

			using (var connection = new SqlConnection(connectionString))
			{
				return connection.Query<Purchase>(sql, new { UserId = UserId}).ToList();
			}
		}
	}
}
