using Dapper;
using System.Data.SqlClient;
using System;

namespace Shop.Domain.Repository
{
	public class UserRepository
	{
		private readonly string connectionString;

		public UserRepository(string connectionString)
		{
			this.connectionString = connectionString;
		}

		public void Add(User user)
		{
			var sql = "Insert into Users (Id, CreationDate, Login, Balance, FullName, PhoneNumber, Email, Address, Password, VerificationCode) Values (@Id, @CreationDate, @Login, @Balance, @FullName, @PhoneNumber, @Email, @Address, @Password, @VerificationCode);";

			using (var connection = new SqlConnection(connectionString))
			{
				var rowAffected = connection.Execute(sql, user);
				if (rowAffected != 1)   // так как вставка всего на 1 строку
				{
					throw new Exception("Что-то пошло не так");
				}
			}
		}

		public void Update(User user)
		{
			string sql = "UPDATE Users SET Balance = @Balance WHERE Id = @Id;";
			using (var connection = new SqlConnection(connectionString))
			{
				var rowAffected = connection.Execute(sql, user);
				if (rowAffected != 1)   
				{
					throw new Exception("Что-то пошло не так");
				}
			}
		}

		public string GetHashedVerCodeByLogin(string login)
		{
			var sql = "Select VerificationCode from Users where Login = @Login;";
			using (var connection = new SqlConnection(connectionString))
			{
				return connection.QuerySingleOrDefault<string>(sql, new { Login = login });
			}
		}

		public string GetHashedPassByLogin(string login)
		{
			var sql = "Select Password from Users where Login = @Login;";
			using (var connection = new SqlConnection(connectionString))
			{
				return connection.QuerySingleOrDefault<string>(sql, new { Login = login });
			}
		}

		public int HowMuchOfLoginsExist(string login)
		{
			var sql = "Select * from Users where Login = @Login;";
			using (var connection = new SqlConnection(connectionString))
			{
				return connection.Query(sql, new { Login = login }).AsList().Count;
			}
		}

		public User GetUserByLogin(string login)
		{
			var sql = "Select * from Users where Login = @Login;";

			using (var connection = new SqlConnection(connectionString))
			{
				return connection.QuerySingleOrDefault<User>(sql, new { Login = login });
			}
		}
			 
		public User GetUserById(Guid id)
		{
			var sql = "Select * from Users " +
								"Where Id = @Id;";

			using (var connection = new SqlConnection(connectionString))
			{
				return connection.QuerySingleOrDefault<User>(sql, new { Id = id });
			}
		}
	}
}
