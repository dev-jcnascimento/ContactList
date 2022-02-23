using System.Collections.Generic;
using System.Threading.Tasks;
using ContactList.Core.Arguments.ContactBook;

namespace ContactList.Core.Interfaces.IServices
{
    public interface IContactBookService
    {
        Task<ContactBookResponse> CreateAsync(CreateContactBookRequest request);
        Task<IEnumerable<ContactBookResponse>> GetAllAsync(int page, int size);
        Task<ContactBookResponse> GetByIdAsync(int id);
        Task UpdateAsync(int id, UpdateContactBookRequest request);
        Task DeleteByIdAsync(int id);
        Task ExistContactAndCompany(int id);

    }
}
