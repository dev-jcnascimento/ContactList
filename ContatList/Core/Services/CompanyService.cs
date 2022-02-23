using Canducci.Pagination;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using ContactList.Core.Arguments.Company;
using ContactList.Core.Domain;
using ContactList.Core.Interfaces.IRepositories;
using ContactList.Core.Interfaces.IServices;

namespace ContactList.Core.Services
{
    public class CompanyService : ICompanyService
    {
        private readonly ICompanyRepository _companyRepository;
        private readonly IContactBookService _contactBookService;

        public CompanyService(ICompanyRepository companyRepository, IContactBookService contactBookService)
        {
            _companyRepository = companyRepository;
            _contactBookService = contactBookService;
        }

        public async Task<CompanyResponse> CreateAsync(CreateCompanyRequest request)
        {
            await ExistContactBook(request.ContactBookId);

            var company = new Company(request.ContactBookId, request.Name);

            var result = await _companyRepository.CreateAsync(company);

            return (CompanyResponse)result;
        }

        public async Task<IEnumerable<CompanyResponse>> GetAllAsync(int page, int size)
        {
            var allCompany = await _companyRepository.GetAllAsync();

            var response = allCompany.ToList().Select(x => (CompanyResponse)x).ToList();

            return response.ToPaginated(page, size);
        }

        public async Task<CompanyResponse> GetByIdAsync(int id)
        {
            var company = await _companyRepository.GetByIdAsync(id);

            if (company == null) throw new ValidationException("Id Company not found!");

            return (CompanyResponse)company;
        }

        public async Task UpdateAsync(int id,UpdateCompanyRequest request)
        {
            if (await GetByIdAsync(id) == null) return;

            await ExistContactBook(request.ContactBookId);

            var company = await _companyRepository.GetByIdAsync(id);

            company.Update(request.ContactBookId, request.Name);

            await _companyRepository.UpdateAsync(company);
        }

        public async Task DeleteByIdAsync(int id)
        {
            if (await GetByIdAsync(id) == null) return;

            await _companyRepository.DeleteByIdAsync(id);
        }

        private async Task ExistContactBook(int id)
        {
            if (await _contactBookService.GetByIdAsync(id) == null)
            {
                throw new ValidationException("Id ContactBook not found!");
            }
        }
    }
}
