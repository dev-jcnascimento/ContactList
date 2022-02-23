using ContactList.Core.Hateoas;

namespace ContactList.Core.Arguments.ContactBook
{
    public class ContactBookResponse : Recurso
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public static explicit operator ContactBookResponse(Domain.ContactBook entity)
        {
            return new ContactBookResponse()
            {
                Id = entity.Id,
                Name = entity.Name,
            };
        }
    }
}
