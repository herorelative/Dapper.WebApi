using Dapper.Application.Interfaces;
using Dapper.Core.Entities;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dapper.Infrastructure.Repositories
{
    public class ProductRepository : IProductRepository
    {
        private readonly IConfiguration configuration;

        public ProductRepository(IConfiguration configuration)
        {
            this.configuration = configuration;
        }
        public async Task<int> AddAsync(Product entity)
        {
            entity.AddedOn = DateTime.UtcNow;

            var sql = "INSERT INTO Products(Name, Description, Barcode, Rate, AddedOn) VALUES (@Name, @Description, @Barcode, @Rate, @AddedOn)";

            using var connection = new SqlConnection(configuration.GetConnectionString("DefaultConnection"));

            connection.Open();

            return await connection.ExecuteAsync(sql, entity);
        }

        public async Task<int> DeleteAsync(int id)
        {
            var sql = "DELETE FROM Products WHERE Id=@Id";

            using var connection = new SqlConnection(configuration.GetConnectionString("DefaultConnection"));

            connection.Open();

            return await connection.ExecuteAsync(sql, new { Id = id });
        }

        public async Task<IReadOnlyList<Product>> GetAllAsync()
        {
            var sql = "SELECT * FROM Products";

            using var connection = new SqlConnection(configuration.GetConnectionString("DefaultConnection"));

            connection.Open();

            var result = await connection.QueryAsync<Product>(sql);

            return result.ToList();
        }

        public async Task<Product> GetByIdAsync(int id)
        {
            var sql = "SELECT * FROM Products WHERE Id=@Id";

            using var connection = new SqlConnection(configuration.GetConnectionString("DefaultConnection"));

            connection.Open();

            return await connection.QuerySingleOrDefaultAsync<Product>(sql, new { Id = id });
        }

        public async Task<int> UpdateAsync(Product entity)
        {
            entity.ModifiedOn = DateTime.UtcNow;

            var sql = "UPDATE Products SET Name=@Name, Description=@Description, Barcode=@Barcode, Rate=@Rate, ModifiedOn=@ModifiedOn WHERE Id=@Id";

            using var connection = new SqlConnection(configuration.GetConnectionString("DefaultConnection"));

            connection.Open();

            return await connection.ExecuteAsync(sql, entity);
        }
    }
}
