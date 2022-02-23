using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using ContactList.Core.Arguments.ContactBook;
using ContactList.Core.Hateoas;
using ContactList.Core.Interfaces.IServices;

namespace ContactList.Controllers
{
    [ApiController]
    [Route("v1/contactBooks")]
    public class ContactBookController : ControllerBase
    {
        private readonly IContactBookService _contactBookService;
        private readonly IUrlHelper _urlHelper;

        public ContactBookController(IContactBookService contactBookService, IUrlHelper urlHelper)
        {
            _contactBookService = contactBookService;
            _urlHelper = urlHelper;
        }

        [HttpPost]
        public async Task<ActionResult> PostAsync(CreateContactBookRequest request)
        {
            try
            {
                var contactBook = await _contactBookService.CreateAsync(request);

                GerarLinks(contactBook);

                return StatusCode(201, contactBook);
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
        public async Task<ActionResult<IEnumerable<ContactBookResponse>>> GetAllAsync(int? page, int? size)
        {
            page ??= 1;
            if (page <= 0) page = 1;

            size ??= 10;
            if (size <= 0) size = 10;

            var contactBook = await _contactBookService.GetAllAsync((int)page, (int)size);

            contactBook.ToList().ForEach(c => GerarLinks(c));

            return Ok(contactBook);
        }

        [HttpGet("{id}", Name = nameof(GetByIdContactBookAsync))]
        public async Task<ActionResult> GetByIdContactBookAsync(int id)
        {
            try
            {
                var contactBook = await _contactBookService.GetByIdAsync(id);

                GerarLinks(contactBook);

                return Ok(contactBook);
            }
            catch (ValidationException ex)
            {
                return NotFound(ex.Message);
            }
        }

        [Authorize]
        [HttpPut("{id}", Name = nameof(PutContactBookAsync))]
        public async Task<ActionResult> PutContactBookAsync(int id, UpdateContactBookRequest request)
        {
            try
            {
                await _contactBookService.UpdateAsync(id, request);
            }
            catch (ValidationException ex)
            {
                return BadRequest(ex.Message);
            }
            return NoContent();
        }

        [Authorize]
        [HttpDelete("{id}", Name = nameof(DeleteByIdContactBookAsync))]
        public async Task<ActionResult> DeleteByIdContactBookAsync(int id)
        {
            try
            {
                await _contactBookService.DeleteByIdAsync(id);
            }
            catch (ValidationException ex)
            {
                return NotFound(ex.Message);
            }
            return NoContent();
        }
        private void GerarLinks(ContactBookResponse contactBook)
        {
            contactBook.Links.Add(new LinkDTO(_urlHelper.Link(nameof(GetByIdContactBookAsync), new { id = contactBook.Id }), rel: "self-contactBook", metodo: "GET"));
            contactBook.Links.Add(new LinkDTO(_urlHelper.Link(nameof(PutContactBookAsync), new { id = contactBook.Id }), rel: "update-contactBook", metodo: "PUT"));
            contactBook.Links.Add(new LinkDTO(_urlHelper.Link(nameof(DeleteByIdContactBookAsync), new { id = contactBook.Id }), rel: "delete-contactBook", metodo: "DELETE"));
        }
    }
}
