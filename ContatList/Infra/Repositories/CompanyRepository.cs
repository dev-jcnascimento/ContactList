using Dapper;
using Dapper.Contrib.Extensions;
using Microsoft.Data.Sqlite;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using ContactList.Core.Domain;
using ContactList.Core.Interfaces.IRepositories;
using ContactList.Database;

namespace ContactList.Infra.Repositories
{
    public class CompanyRepository : ICompanyRepository
    {
        private readonly DatabaseConfig _databaseConfig;

        public CompanyRepository(DatabaseConfig databaseConfig)
        {
            _databaseConfig = databaseConfig;
        }

        public async Task<Company> CreateAsync(Company company)
        {
            using var connection = new SqliteConnection(_databaseConfig.ConnectionString);

            var resultId = await connection.InsertAsync(company);

            var result = await GetByIdAsync(resultId);

            if (resultId == 0) throw new TaskCanceledException("Unable to Create!");

            return result;
        }

        public async Task<IEnumerable<Company>> GetAllAsync()
        {
            using var connection = new SqliteConnection(_databaseConfig.ConnectionString);

            var query = "SELECT * FROM Company";

            var result = await connection.QueryAsync<Company>(query);

            return result;
        }

        public async Task<Company> GetByIdAsync(int id)
        {
            using var connection = new SqliteConnection(_databaseConfig.ConnectionString);

            var query = "SELECT * FROM Company WHERE Id =" + id;

            var result = await connection.QuerySingleOrDefaultAsync<Company>(query);

            return result;
        }

        public async Task UpdateAsync(Company company)
        {
            using var connection = new SqliteConnection(_databaseConfig.ConnectionString);

            var result = await connection.UpdateAsync(company);

            if (result == false) throw new TaskCanceledException("Unable to update!");
        }

        public async Task DeleteByIdAsync(int id)
        {
            using var connection = new SqliteConnection(_databaseConfig.ConnectionString);

            var sql = new StringBuilder();
            sql.AppendLine("DELETE FROM Company WHERE Id = @id;");
            sql.AppendLine("UPDATE Contact SET CompanyId = null WHERE CompanyId = @id;");

            await connection.ExecuteAsync(sql.ToString(),new { id });
        }
    }
}
