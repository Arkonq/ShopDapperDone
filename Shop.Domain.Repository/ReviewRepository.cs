using Dapper;
using System.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Shop.Domain.Repository
{
	public class ReviewRepository
	{
		private readonly string connectionString;

		public ReviewRepository(string connectionString)
		{
			this.connectionString = connectionString;
		}

		public void Add(Review review)
		{
			var sql = "Insert into Reviews (Id, CreationDate, UserId, ItemId, Rate, Value) Values (@Id, @CreationDate, @UserId, @ItemId, @Rate, @Value);";

			using (var connection = new SqlConnection(connectionString))
			{
				var rowAffected = connection.Execute(sql, review);
				if (rowAffected != 1)                             // так как вставка всего на 1 строку
				{
					throw new Exception("Что-то пошло не так");
				}
			}
		}

		public int ReviewsCount(Guid ResultItemId)
		{
			var sql = "Select * From Reviews " +
				"WHERE ItemId = @ResultItemId";

			using (var connection = new SqlConnection(connectionString))
			{
				return connection.Query(sql, new { ResultItemId = ResultItemId }).AsList().Count;
			}
		}

		public ICollection<Review> GetAll(Guid ItemId)
		{
			var sql = "Select * From Reviews " +
				"WHERE ItemId = @ItemId " +
				"Order by CreationDate";

			using (var connection = new SqlConnection(connectionString))
			{
				return connection.Query<Review>(sql, new { ItemId = ItemId }).ToList();
			}
		}

		public ICollection<Review> ReviewsOnPage(int page, int pageSize, Guid ItemId)
		{
			var sql = "Select * From Reviews " +
								$"Where ItemId = @ItemId " +
								"ORDER BY (SELECT NULL) " +
								"OFFSET @Skip ROWS " +
								"FETCH NEXT @Take ROWS ONLY; ";

			using (var connection = new SqlConnection(connectionString))
			{
				return connection.Query<Review>(sql, new { Skip = (page - 1) * pageSize, Take = pageSize , ItemId = ItemId}).ToList();
			}
		}

		public Guid GetUserId(Guid Id)
		{
			var sql = "Select * From Reviews " +
								$"Where Id = @Id ";

			using (var connection = new SqlConnection(connectionString))
			{
				return connection.QuerySingleOrDefault<Review>(sql, new { Id = Id}).UserId;
			}
		}
	}
}
