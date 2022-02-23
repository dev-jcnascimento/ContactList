using ContactList.Core.Hateoas;

namespace ContactList.Core.Arguments.Contact
{
    public class ContactResponse : Recurso
    {
        public int Id { get; set; }
        public int ContactBookId { get; set; }
        public int CompanyId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public string Address { get; set; }

        public static explicit operator ContactResponse(Domain.Contact entity)
        {
            return new ContactResponse()
            {
                Id = entity.Id,
                ContactBookId = entity.ContactBookId,
                CompanyId = entity.CompanyId,
                FirstName = entity.FirstName,
                LastName = entity.LastName,
                Phone = entity.Phone,
                Email = entity.Email,
                Address = entity.Address
            };
        }
    }
}
