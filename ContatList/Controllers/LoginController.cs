using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using ContactList.Core.Arguments.Contact;
using ContactList.Core.Interfaces.IServices;
using ContactList.Security;

namespace ContactList.Controllers
{
    [ApiController]
    [Route("v1/logins")]
    public class LoginController : ControllerBase
    {
        private readonly IContactService _contactService;

        public LoginController(IContactService contactService)
        {
            _contactService = contactService;
        }

        [HttpPost]
        public async Task<ActionResult> AuthenticateAsync(AuthenticateContactRequest request)
        {
            try
            {
                var response = await _contactService.Authenticate(request);

                var token = TokenSecurity.GenerateToken(response.Email);
                return Ok(new { user = response, token = token });
            }
            catch (ValidationException ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}