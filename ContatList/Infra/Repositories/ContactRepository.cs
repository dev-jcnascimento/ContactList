using Dapper;
using Dapper.Contrib.Extensions;
using Microsoft.Data.Sqlite;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ContactList.Core.Arguments.Contact;
using ContactList.Core.Domain;
using ContactList.Core.Interfaces.IRepositories;
using ContactList.Database;

namespace ContactList.Infra.Repositories
{
    public class ContactRepository : IContactRepository
    {
        private readonly DatabaseConfig _databaseConfig;

        public ContactRepository(DatabaseConfig databaseConfig)
        {
            _databaseConfig = databaseConfig;
        }

        public async Task<Contact> GetByAuthenticate(string email, string password)
        {
            using var connection = new SqliteConnection(_databaseConfig.ConnectionString);

            var query = $"SELECT * FROM Contact where Email = '{email}' and Password = '{password}';" /*+ "AND Password =" + password*/;

            var result = await connection.QueryFirstOrDefaultAsync<Contact>(query);

            return result;
        }

        public async Task<Contact> CreateAsync(Contact contact)
        {
            using var connection = new SqliteConnection(_databaseConfig.ConnectionString);

            var resultId = await connection.InsertAsync(contact);

            var result = await GetByIdAsync(resultId);

            if (resultId == 0) throw new TaskCanceledException("Unable to Create!");

            return result;
        }

        public async Task<IEnumerable<Contact>> ImportContactAsync(List<Contact> contacts)
        {
            using var connection = new SqliteConnection(_databaseConfig.ConnectionString);

            List<int> idContacts = new List<int>();

            foreach (var contact in contacts)
            {
                int resultId = await connection.InsertAsync(contact);
                idContacts.Add(resultId);
            }

            List<Contact> listContacts = new List<Contact>();

            foreach (var id in idContacts)
            {
                var result = await GetByIdAsync(id);
                listContacts.Add(result);
            }

            if (listContacts.Count() == 0) throw new TaskCanceledException("0 contacts saved!");

            return listContacts;
        }
        public async Task<IEnumerable<Contact>> GetAllAsync()
        {
            using var connection = new SqliteConnection(_databaseConfig.ConnectionString);

            var query = "SELECT * FROM Contact";

            var result = await connection.QueryAsync<Contact>(query);

            return result;
        }

        public async Task<Contact> GetByIdAsync(int id)
        {
            using var connection = new SqliteConnection(_databaseConfig.ConnectionString);

            var query = "SELECT * FROM Contact WHERE Id =" + id;

            var result = await connection.QueryFirstOrDefaultAsync<Contact>(query);

            return result;
        }
        public async Task<IEnumerable<Contact>> GetByIdCompanyAsync(int companyId, int contactBookId)
        {
            using var connection = new SqliteConnection(_databaseConfig.ConnectionString);

            var query = $"SELECT * FROM Contact WHERE CompanyId = {companyId} And ContactBookId = {contactBookId};";

            var result = await connection.QueryAsync<Contact>(query);

            return result;
        }

        public async Task<IEnumerable<Contact>> GetByParametersAsync(List<ConvertToParameters> parameters)
        {
            List<Contact> result = new List<Contact>();

            using var connection = new SqliteConnection(_databaseConfig.ConnectionString);
            foreach (var item in parameters)
            {
                var query = $"SELECT * FROM Contact where {item.Parameter} = '{item.Value}'";
                var contacts = await connection.QueryAsync<Contact>(query);
                foreach (var contact in contacts)
                {
                    if (contact != null && !result.Any(x => x.Id == contact.Id)) result.Add((Contact)contact);
                }
            }

            return result;
        }

        public async Task UpdateAsync(Contact contact)
        {
            using var connection = new SqliteConnection(_databaseConfig.ConnectionString);

            var result = await connection.UpdateAsync(contact);

            if (result == false) throw new TaskCanceledException("Unable to update!");
        }

        public async Task DeleteByIdAsync(int id)
        {
            using var connection = new SqliteConnection(_databaseConfig.ConnectionString);

            var sql = "DELETE FROM Contact WHERE Id = " + id;

            await connection.ExecuteAsync(sql);
        }
    }
}
