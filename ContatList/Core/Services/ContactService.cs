using Canducci.Pagination;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using ContactList.Core.Arguments.Contact;
using ContactList.Core.Domain;
using ContactList.Core.Extensions;
using ContactList.Core.Extensions.Validator;
using ContactList.Core.Interfaces.IRepositories;
using ContactList.Core.Interfaces.IServices;

namespace ContactList.Core.Services
{
    public class ContactService : IContactService
    {
        private readonly IContactRepository _contactRepository;
        private readonly ICompanyRepository _companyRepository;
        private readonly IContactRepository _contactBookRepository;

        public ContactService(IContactRepository contactRepository, ICompanyRepository companyRepository, IContactRepository contactBookRepository)
        {
            _contactRepository = contactRepository;
            _companyRepository = companyRepository;
            _contactBookRepository = contactBookRepository;
        }

        public async Task<AuthenticateContactResponse> Authenticate(AuthenticateContactRequest request)
        {
            var email = EmailValidator.Validating(request.Email);
            var password = PasswordValidator.Validating(request.Password);
            var contact = await _contactRepository.GetByAuthenticate(email, password);

            if (contact == null) throw new ValidationException("Incorrect email or password");

            return (AuthenticateContactResponse)contact;
        }
        public async Task<ContactResponse> CreateAsync(CreateContactRequest request)
        {
            await ExistContactBook(request.ContactBookId);

            if (request.CompanyId > 0)
            {
                await ExistCompany(request.CompanyId);
            }
            var contact = new Contact(
                request.ContactBookId,
                request.CompanyId,
                request.FirstName,
                request.LastName,
                request.Phone,
                request.Password,
                request.Email,
                request.Address);

            var result = await _contactRepository.CreateAsync(contact);

            return (ContactResponse)result;
        }
        public async Task<IEnumerable<ContactResponse>> ImportContactAsync(string url)
        {
            List<Contact> contacts = new List<Contact>();

            foreach (Contact contact in ImportContactInFile.Read(url))
            {

                var contactBookInContact = await _contactBookRepository.GetByIdAsync(contact.ContactBookId);
                if (contactBookInContact != null)
                {
                    if (contact.CompanyId > 0)
                    {
                        var companyInContact = await _companyRepository.GetByIdAsync(contact.CompanyId);

                        if (companyInContact == null)
                        {
                            goto Add;
                        }
                    }
                    contacts.Add(contact);
                Add:
                    continue;
                }
            }
            var result = await _contactRepository.ImportContactAsync(contacts);

            var response = result.ToList().Select(x => (ContactResponse)x).ToList();

            return response;
        }
        public async Task<IEnumerable<ContactResponse>> GetAllAsync(int page, int size)
        {
            var allContacts = await _contactRepository.GetAllAsync();

            var response = allContacts.ToList().Select(x => (ContactResponse)x).ToList();

            return response.ToPaginated(page, size);
        }

        public async Task<ContactResponse> GetByIdAsync(int id)
        {
            var company = await _contactRepository.GetByIdAsync(id);

            if (company == null) throw new ValidationException("Id Contact not found!");

            return (ContactResponse)company;
        }
        public async Task<IEnumerable<ContactResponse>> GetByIdCompanyAsync(int id, int page, int size)
        {
            var company = await _companyRepository.GetByIdAsync(id);
            var companyId = company.Id;
            var contactBookId = company.ContactBookId;

            var allContactsCompany = await _contactRepository.GetByIdCompanyAsync(companyId, contactBookId);

            var response = allContactsCompany.ToList().Select(x => (ContactResponse)x).ToList();

            return response.ToPaginated(page, size);
        }

        public async Task<IEnumerable<ContactResponse>> GetByParametersAsync(int page, int size, int id, int contactBookId,
            int companyId, string firstName, string lastName, string phone, string email, string address)
        {
            List<ConvertToParameters> listParameters = new List<ConvertToParameters>();
            listParameters.Add(new ConvertToParameters("Id",id.ToString()));
            listParameters.Add(new ConvertToParameters("FirstName", firstName));
            listParameters.Add(new ConvertToParameters("LastName", lastName));
            listParameters.Add(new ConvertToParameters("Phone", phone));
            listParameters.Add(new ConvertToParameters("Email", email));
            listParameters.Add(new ConvertToParameters("Address", address));
            listParameters.Add(new ConvertToParameters("ContactBookId", contactBookId.ToString()));
            listParameters.Add(new ConvertToParameters("CompanyId", companyId.ToString()));

            var result = await _contactRepository.GetByParametersAsync(listParameters);

            var response = result.ToList().Select(x => (ContactResponse)x).ToList();

            return response.ToPaginated(page, size);
        }

        public async Task GetExport(string url)
        {
            var contacts = await _contactRepository.GetAllAsync();

            List<Contact> listContacts = (List<Contact>)contacts;

            WriteContactInFile.WriteFile(url, listContacts);
        }

        public async Task UpdateAsync(int id, UpdateContactRequest request)
        {
            if (await GetByIdAsync(id) == null) return;

            await ExistContactBook(request.ContactBookId);

            if (request.CompanyId > 0)
            {
                await ExistCompany(request.CompanyId);
            }
            var contact = await _contactRepository.GetByIdAsync(id);

            contact.Update(
                request.ContactBookId,
                request.CompanyId,
                request.FirstName,
                request.LastName,
                request.Phone,
                request.Email,
                request.Address);

            await _contactRepository.UpdateAsync(contact);
        }
        public async Task DeleteByIdAsync(int id)
        {
            if (await GetByIdAsync(id) == null) return;

            await _contactRepository.DeleteByIdAsync(id);
        }

        private async Task ExistContactBook(int id)
        {
            var contactBook = await _contactBookRepository.GetByIdAsync(id);

            if (contactBook == null) throw new ValidationException("Id ContactBook not found!");

        }

        private async Task ExistCompany(int id)
        {
            var company = await _companyRepository.GetByIdAsync(id);

            if (company == null) throw new ValidationException("Id Company not found!");
        }
    }
}
