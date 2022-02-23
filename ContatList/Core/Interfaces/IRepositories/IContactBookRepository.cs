using System.Collections.Generic;
using System.Threading.Tasks;
using ContactList.Core.Domain;

namespace ContactList.Core.Interfaces.IRepositories
{
    public interface IContactBookRepository
    {
        Task<ContactBook> CreateAsync(ContactBook contactBook);
        Task<IEnumerable<ContactBook>> GetAllAsync();
        Task<ContactBook> GetByIdAsync(int id);
        Task UpdateAsync(ContactBook contactBook);
        Task DeleteByIdAsync(int id);
        Task<object> ExistContact(int id);
        Task<object> ExistCompany(int id);
    }
}
