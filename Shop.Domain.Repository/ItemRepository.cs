using Dapper;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace Shop.Domain.Repository
{
	public class ItemRepository
	{
		private readonly string connectionString;

		public ItemRepository(string connectionString)
		{
			this.connectionString = connectionString;
		}

		public Item SelectItemById(Guid ItemId)
		{
			var sql = "Select * From Items " +
				"Where Id = @ItemId";

			using (var connection = new SqlConnection(connectionString))
			{
				return connection.QuerySingleOrDefault<Item>(sql, new { ItemId = ItemId});
			}
		}

		public void Update(Item item)
		{
			string sql = "UPDATE Items SET Quantity = @Quantity WHERE Id = @Id;";
			using (var connection = new SqlConnection(connectionString))
			{
				var rowAffected = connection.Execute(sql, item);
				if (rowAffected != 1)
				{
					throw new Exception("Что-то пошло не так");
				}
			}
		}

		public Item ChooseItem(int page, int pageSize, int toSkip)
		{
			var sql = "Select * From Items " +
								"ORDER BY (SELECT NULL)" +
								"OFFSET @Skip ROWS " +
								"FETCH NEXT 1 ROWS ONLY ";

			using (var connection = new SqlConnection(connectionString))
			{
				return connection.QuerySingleOrDefault<Item>(sql, new { Skip = (page - 1) * pageSize + toSkip, Take = pageSize });
			}
		}

		public Item ChooseItem(int page, int pageSize, int toSkip, string search)
		{
			var sql = "Select * From Items " +
							 $"Where Name like '%{search}%'" +
								"ORDER BY (SELECT NULL)" +
								"OFFSET @Skip ROWS " +
								"FETCH NEXT 1 ROWS ONLY ";

			using (var connection = new SqlConnection(connectionString))
			{
				return connection.QuerySingleOrDefault<Item>(sql, new { Skip = (page - 1) * pageSize + toSkip, Take = pageSize });
			}
		}

		public int ItemsInCategory(Guid CategoryId)
		{
			var sql = "Select * From Items " +
								"Where CategoryId = @CategoryId";

			using (var connection = new SqlConnection(connectionString))
			{
				return connection.Query(sql, new { CategoryId = CategoryId}).AsList().Count;
			}
		}

		public ICollection<Item> ItemsOnPage(int page, int pageSize)
		{
			var sql = "Select * From Items " +
								"ORDER BY (SELECT NULL)" +
								"OFFSET @Skip ROWS " +
								"FETCH NEXT @Take ROWS ONLY; ";

			using (var connection = new SqlConnection(connectionString))
			{
				return connection.Query<Item>(sql, new { Skip = (page - 1) * pageSize, Take = pageSize }).ToList();
			}
		}

		public ICollection<Item> ItemsOnPage(int page, int pageSize, string search)
		{
			var sql = "Select * From Items " +
								$"Where Name like '%{search}%'" +
								"ORDER BY (SELECT NULL)" +
								"OFFSET @Skip ROWS " +
								"FETCH NEXT @Take ROWS ONLY; ";

			using (var connection = new SqlConnection(connectionString))
			{
				return connection.Query<Item>(sql, new { Skip = (page - 1) * pageSize, Take = pageSize }).ToList();
			}
		}

		public int GetQuantity(Guid ResultItemId)
		{
			var sql = "Select Quantity From Items " +
								"Where Id = @ResultItemId";

			using (var connection = new SqlConnection(connectionString))
			{
				return connection.QuerySingleOrDefault<int>(sql, new { ResultItemId = ResultItemId});
			}
		}

		public int GetPrice(Guid ResultItemId)
		{
			var sql = "Select Price From Items " +
								"Where Id = @ResultItemId";

			using (var connection = new SqlConnection(connectionString))
			{
				return connection.QuerySingleOrDefault<int>(sql, new { ResultItemId = ResultItemId });
			}
		}

		public int ItemsBySearch(string search)
		{
			var sql = "Select * From Items " +
								$"Where Name like '%{search}%'";

			using (var connection = new SqlConnection(connectionString))
			{
				return connection.Query(sql).Count();
			}
		}
	}
}
