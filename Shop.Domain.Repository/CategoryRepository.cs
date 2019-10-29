using Dapper;
using System.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Shop.Domain.Repository
{
	public class CategoryRepository
	{
		private readonly string connectionString;

		public CategoryRepository(string connectionString)
		{
			this.connectionString = connectionString;
		}

		public int AllCategoriesCount()
		{
			var sql = "Select * From Categories";

			using (var connection = new SqlConnection(connectionString))
			{
				return connection.Query(sql).AsList().Count;
			}
		}

		public ICollection<Category> CategoriesOnPage(int page, int pageSize)
		{
			var sql = "Select * From Categories " +
								"ORDER BY (SELECT NULL)" +
								"OFFSET @Skip ROWS " +
								"FETCH NEXT @Take ROWS ONLY; ";

			using (var connection = new SqlConnection(connectionString))
			{
				return connection.Query<Category>(sql, new { Skip = (page - 1) * pageSize, Take = pageSize }).ToList();
			}
		}

		public int CategoriesOnPageCnt(int page, int pageSize)
		{
			var sql = "Select * From Categories " +
								"ORDER BY (SELECT NULL)" +
								"OFFSET @Skip ROWS " +
								"FETCH NEXT @Take ROWS ONLY; ";
								
			using (var connection = new SqlConnection(connectionString))
			{
				return connection.Query(sql, new { Skip = (page - 1) * pageSize, Take = pageSize}).AsList().Count;
			}
		}

		public Category ChooseCategory(int page, int pageSize, int categoryNum)
		{
			var sql = "Select * From Categories " +
								"ORDER BY (SELECT NULL)" +
								"OFFSET @Skip ROWS " +
								"FETCH NEXT 1 ROWS ONLY ";

			using (var connection = new SqlConnection(connectionString))
			{
				return connection.QuerySingleOrDefault<Category>(sql, new { Skip = (page - 1) * pageSize + categoryNum - 1, Take = pageSize});
			}
		}


	}
}
