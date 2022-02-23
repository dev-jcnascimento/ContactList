using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using ContactList.Core.Arguments.Company;
using ContactList.Core.Hateoas;
using ContactList.Core.Interfaces.IServices;

namespace ContactList.Controllers
{
    [ApiController]
    [Route("v1/companys")]
    public class CompanyController : ControllerBase
    {
        private readonly ICompanyService _companyService;
        private readonly IUrlHelper _urlHelper;

        public CompanyController(ICompanyService companyService, IUrlHelper urlHelper)
        {
            _companyService = companyService;
            _urlHelper = urlHelper;
        }

        [HttpPost]
        public async Task<ActionResult> PostAsync(CreateCompanyRequest request)
        {
            try
            {
                var company = await _companyService.CreateAsync(request);

                GerarLinks(company);

                return StatusCode(201, company);
            }
            catch (ValidationException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (TaskCanceledException ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<CompanyResponse>>> GetAllAsync(int? page, int? size)
        {
            page ??= 1;
            if (page <= 0) page = 1;

            size ??= 10;
            if (size <= 0) size = 10;

            var company = await _companyService.GetAllAsync((int)page, (int)size);

            company.ToList().ForEach(c => GerarLinks(c));

            return Ok(company);
        }

        [HttpGet("{id}", Name = nameof(GetByIdCompanyAsync))]
        public async Task<ActionResult> GetByIdCompanyAsync(int id)
        {
            try
            {
                var company = await _companyService.GetByIdAsync(id);

                GerarLinks(company);

                return Ok(company);
            }
            catch (ValidationException ex)
            {
                return NotFound(ex.Message);
            }
        }

        [Authorize]
        [HttpPut("{id}",Name = nameof(PutCompanyAsync))]
        public async Task<ActionResult> PutCompanyAsync(int id, UpdateCompanyRequest request)
        {
            try
            {
                await _companyService.UpdateAsync(id,request);
            }
            catch (ValidationException ex)
            {
                return BadRequest(ex.Message);
            }
            return NoContent();
        }

        [Authorize]
        [HttpDelete("{id}", Name = nameof(DeleteByIdCompanyAsync))]
        public async Task<ActionResult> DeleteByIdCompanyAsync(int id)
        {
            try
            {
                await _companyService.DeleteByIdAsync(id);
            }
            catch (ValidationException ex)
            {
                return NotFound(ex.Message);
            }
            return NoContent();
        }
        private void GerarLinks(CompanyResponse company)
        {
            company.Links.Add(new LinkDTO(_urlHelper.Link(nameof(GetByIdCompanyAsync), new { id = company.Id }), rel: "self-company", metodo: "GET"));
            company.Links.Add(new LinkDTO(_urlHelper.Link(nameof(PutCompanyAsync), new { id = company.Id }), rel: "update-company", metodo: "PUT"));
            company.Links.Add(new LinkDTO(_urlHelper.Link(nameof(DeleteByIdCompanyAsync), new { id = company.Id }), rel: "delete-company", metodo: "DELETE"));
        }

    }
}
