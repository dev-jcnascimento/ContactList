using System.Collections.Generic;
using System.Threading.Tasks;
using ContactList.Core.Arguments.Contact;
using ContactList.Core.Domain;

namespace ContactList.Core.Interfaces.IRepositories
{
    public interface IContactRepository
    {
        Task<Contact> GetByAuthenticate(string email,string password);
        Task<Contact> CreateAsync(Contact contact);
        Task<IEnumerable<Contact>> ImportContactAsync(List<Contact> contacts);
        Task<IEnumerable<Contact>> GetAllAsync();
        Task<Contact> GetByIdAsync(int id);
        Task<IEnumerable<Contact>> GetByIdCompanyAsync(int companyId, int contactBookId);
        Task<IEnumerable<Contact>> GetByParametersAsync(List<ConvertToParameters> parameters);
        Task UpdateAsync(Contact contact);
        Task DeleteByIdAsync(int id);
    }
}
