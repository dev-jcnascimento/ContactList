using System.Collections.Generic;
using System.Threading.Tasks;
using ContactList.Core.Arguments.Contact;

namespace ContactList.Core.Interfaces.IServices
{
    public interface IContactService
    {
        Task<AuthenticateContactResponse> Authenticate(AuthenticateContactRequest request);
        Task<ContactResponse> CreateAsync(CreateContactRequest request);
        Task<IEnumerable<ContactResponse>> ImportContactAsync(string url);
        Task<IEnumerable<ContactResponse>> GetAllAsync(int page, int size);
        Task<ContactResponse> GetByIdAsync(int id);
        Task<IEnumerable<ContactResponse>> GetByIdCompanyAsync(int id, int page, int size);
        Task<IEnumerable<ContactResponse>> GetByParametersAsync(int page, int size, int id, int contactBookId,
            int companyId, string firstName, string lastName, string phone, string email, string address);
        Task GetExport(string url);
        Task UpdateAsync(int id, UpdateContactRequest request);
        Task DeleteByIdAsync(int id);
    }
}
