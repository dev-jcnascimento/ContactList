using System.Collections.Generic;
using System.Threading.Tasks;
using ContactList.Core.Arguments.Company;

namespace ContactList.Core.Interfaces.IServices
{
    public interface ICompanyService
    {
        Task<CompanyResponse> CreateAsync(CreateCompanyRequest request);
        Task<IEnumerable<CompanyResponse>> GetAllAsync(int page, int size);
        Task<CompanyResponse> GetByIdAsync(int id);
        Task UpdateAsync(int id, UpdateCompanyRequest request);
        Task DeleteByIdAsync(int id);
    }
}
