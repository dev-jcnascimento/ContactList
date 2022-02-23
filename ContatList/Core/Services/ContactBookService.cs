using Canducci.Pagination;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using ContactList.Core.Arguments.ContactBook;
using ContactList.Core.Domain;
using ContactList.Core.Interfaces.IRepositories;
using ContactList.Core.Interfaces.IServices;

namespace ContactList.Core.Services
{
    public class ContactBookService : IContactBookService
    {
        private readonly IContactBookRepository _contactBookRepository;

        public ContactBookService(IContactBookRepository contactBookRepository)
        {
            _contactBookRepository = contactBookRepository;
        }

        public async Task<ContactBookResponse> CreateAsync(CreateContactBookRequest request)
        {
            var contactBook = new ContactBook(request.Name);

            var result = await _contactBookRepository.CreateAsync(contactBook);

            return (ContactBookResponse)result;
        }

        public async Task<IEnumerable<ContactBookResponse>> GetAllAsync(int page, int size)
        {
            var result = await _contactBookRepository.GetAllAsync();

            var response = result.ToList().Select(x => (ContactBookResponse)x).ToList();

            return response.ToPaginated(page, size);
        }

        public async Task<ContactBookResponse> GetByIdAsync(int id)
        {
            var result = await _contactBookRepository.GetByIdAsync(id);

            if (result == null) throw new ValidationException("Id ContactBook not found!");

            return (ContactBookResponse)result;
        }

        public async Task UpdateAsync(int id,UpdateContactBookRequest request)
        {
            if (await GetByIdAsync(id) == null) return;

            var contactBook = await _contactBookRepository.GetByIdAsync(id);

            contactBook.Update(request.Name);

            await _contactBookRepository.UpdateAsync(contactBook);

        }
        public async Task DeleteByIdAsync(int id)
        {
            if(await GetByIdAsync(id) == null) return;
            await ExistContactAndCompany(id);
            await _contactBookRepository.DeleteByIdAsync(id);
        }
        public async Task ExistContactAndCompany(int id)
        {
            IEnumerable<object> contact = (IEnumerable<object>)await _contactBookRepository.ExistContact(id);
           
            if(contact.Count() > 0) throw new ValidationException("There are Contacts linked to ContactBook!");

            IEnumerable<object> company = (IEnumerable<object>)await _contactBookRepository.ExistCompany(id);

            if (company.Count() > 0) throw new ValidationException("There are Company linked to ContactBook!");
        }
    }
}
