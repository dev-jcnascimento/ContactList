using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using ContactList.Core.Arguments.Contact;
using ContactList.Core.Hateoas;
using ContactList.Core.Interfaces.IServices;

namespace ContactList.Controllers
{
    [ApiController]
    [Route("v1/contacts")]
    public class ContactController : ControllerBase
    {
        private readonly IContactService _contactService;
        private readonly IUrlHelper _urlHelper;

        public ContactController(IContactService contactService, IUrlHelper urlHelper)
        {
            _contactService = contactService;
            _urlHelper = urlHelper;
        }

        [HttpPost]
        public async Task<ActionResult> CreateAsync(CreateContactRequest request)
        {
            try
            {
                var contact = await _contactService.CreateAsync(request);

                GerarLinks(contact);

                return StatusCode(201, contact);
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

        [HttpPost("import/{url}")]
        public async Task<ActionResult> ImportAsync(string url)
        {
            try
            {
                var contact = await _contactService.ImportContactAsync(url);

                contact.ToList().ForEach(c => GerarLinks(c));

                return StatusCode(201, contact);
            }
            catch (Exception ex)
            {
                return StatusCode(400, ex.Message);
            }
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ContactResponse>>> GetAllAsync(int? page, int? size)
        {
            page ??= 1;
            if (page <= 0) page = 1;

            size ??= 10;
            if (size <= 0) size = 10;

            var contact = await _contactService.GetAllAsync((int)page, (int)size);

            contact.ToList().ForEach(c => GerarLinks(c));

            return Ok(contact);
        }

        [HttpGet("{id}", Name = nameof(GetByIdContactAsync))]
        public async Task<ActionResult> GetByIdContactAsync(int id)
        {
            try
            {
                var contact = await _contactService.GetByIdAsync(id);

                GerarLinks(contact);

                return Ok(contact);
            }
            catch (ValidationException ex)
            {
                return NotFound(ex.Message);
            }
        }
        [HttpGet("company/{id}")]
        public async Task<ActionResult> GetByIdCompanyAsync(int id, int? page, int? size)
        {
            page ??= 1;
            if (page <= 0) page = 1;

            size ??= 10;
            if (size <= 0) size = 10;

            try
            {
                var contact = await _contactService.GetByIdCompanyAsync(id, (int)page, (int)size);

                contact.ToList().ForEach(c => GerarLinks(c));

                return Ok(contact);
            }
            catch (ValidationException ex)
            {
                return NotFound(ex.Message);
            }
        }

        [HttpGet("byParameters")]
        public async Task<ActionResult> GetByParametersAsync(int? page, int? size,int id, int contactBookId,
            string firstName, string lastName, string phone, string email, string address,int? companyId)
        {
            page ??= 1;
            if (page <= 0) page = 1;

            size ??= 10;
            if (size <= 0) size = 10;

           companyId ??= -1;
            if (companyId < 0) companyId = -1;

            try
            {
                var contact = await _contactService.GetByParametersAsync((int)page, (int)size,id,contactBookId,(int)companyId,
                    firstName,lastName,phone,email,address);

                contact.ToList().ForEach(c => GerarLinks(c));

                return Ok(contact);
            }
            catch (ValidationException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("export/{url}")]
        public async Task<ActionResult> GetExport(string url)
        {
            try
            {
               await _contactService.GetExport(url);
            }
            catch (ValidationException ex)
            {
                return NotFound(ex.Message);
            }
            return NoContent();
        }

        [Authorize]
        [HttpPut("{id}", Name = nameof(PutContactAsync))]
        public async Task<ActionResult> PutContactAsync(int id, UpdateContactRequest request)
        {
            try
            {
                await _contactService.UpdateAsync(id,request);
            }
            catch (ValidationException ex)
            {
                return BadRequest(ex.Message);
            }
            return NoContent();
        }

        [Authorize]
        [HttpDelete("{id}", Name = nameof(DeleteByIdContactAsync))]
        public async Task<ActionResult> DeleteByIdContactAsync(int id)
        {
            try
            {
                await _contactService.DeleteByIdAsync(id);
            }
            catch (ValidationException ex)
            {
                return NotFound(ex.Message);
            }
            return NoContent();
        }
        private void GerarLinks(ContactResponse contact)
        {
            contact.Links.Add(new LinkDTO(_urlHelper.Link(nameof(GetByIdContactAsync), new { id = contact.Id }), rel: "self-contact", metodo: "GET"));
            contact.Links.Add(new LinkDTO(_urlHelper.Link(nameof(PutContactAsync), new { id = contact.Id }), rel: "update-contact", metodo: "PUT"));
            contact.Links.Add(new LinkDTO(_urlHelper.Link(nameof(DeleteByIdContactAsync), new { id = contact.Id }), rel: "delete-contact", metodo: "DELETE"));
        }
    }
}
