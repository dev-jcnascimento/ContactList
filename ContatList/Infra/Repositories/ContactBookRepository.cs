using Dapper;
using Dapper.Contrib.Extensions;
using Microsoft.Data.Sqlite;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ContactList.Core.Domain;
using ContactList.Core.Interfaces.IRepositories;
using ContactList.Database;

namespace ContactList.Infra.Repositories
{
    public class ContactBookRepository : IContactBookRepository
    {
        private readonly DatabaseConfig _databaseConfig;

        public ContactBookRepository(DatabaseConfig databaseConfig)
        {
            _databaseConfig = databaseConfig;
        }

        public async Task<ContactBook> CreateAsync(ContactBook contactBook)
        {
            using var connection = new SqliteConnection(_databaseConfig.ConnectionString);

            var resultId = await connection.InsertAsync(contactBook);

            var result = await GetByIdAsync(resultId);

            if (resultId == 0) throw new TaskCanceledException("Unable to Create!");

            return result;
        }

        public async Task<IEnumerable<ContactBook>> GetAllAsync()
        {
            using var connection = new SqliteConnection(_databaseConfig.ConnectionString);

            var query = "SELECT * FROM ContactBook";

            var result = await connection.QueryAsync<ContactBook>(query);

            return result;
        }

        public async Task<ContactBook> GetByIdAsync(int id)
        {
            using var connection = new SqliteConnection(_databaseConfig.ConnectionString);

            var query = "SELECT * FROM ContactBook WHERE Id =" + id;
            var result = await connection.QueryFirstOrDefaultAsync<ContactBook>(query);

            return result;
        }

        public async Task UpdateAsync(ContactBook contactBook)
        {
            using var connection = new SqliteConnection(_databaseConfig.ConnectionString);

            var result = await connection.UpdateAsync(contactBook);

            if (result == false) throw new TaskCanceledException("Unable to update!");
        }

        public async Task DeleteByIdAsync(int id)
        {
            using var connection = new SqliteConnection(_databaseConfig.ConnectionString);

            var sql = "DELETE FROM ContactBook WHERE Id = " + id;

            await connection.ExecuteAsync(sql);
        }
        public async Task<object> ExistContact(int id)
        {
            using var connection = new SqliteConnection(_databaseConfig.ConnectionString);

            var query = "SELECT Id FROM Contact WHERE ContactBookId =" + id;

            var existContact = await connection.QueryAsync<object>(query);

            return existContact;
        }
        public async Task<object> ExistCompany(int id)
        {
            using var connection = new SqliteConnection(_databaseConfig.ConnectionString);

            var query = "SELECT Id FROM Company WHERE ContactBookId =" + id;

            var existCompany = await connection.QueryAsync<object>(query);

            return existCompany;
        }
    }
}
