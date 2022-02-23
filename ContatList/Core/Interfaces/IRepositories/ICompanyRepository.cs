using System.Collections.Generic;
using System.Threading.Tasks;
using ContactList.Core.Domain;

namespace ContactList.Core.Interfaces.IRepositories
{
    public interface ICompanyRepository
    {
        Task<Company> CreateAsync(Company company);
        Task<IEnumerable<Company>> GetAllAsync();
        Task<Company> GetByIdAsync(int id);
        Task UpdateAsync(Company company);
        Task DeleteByIdAsync(int id);
    }
}
