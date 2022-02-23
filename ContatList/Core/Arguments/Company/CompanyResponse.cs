using ContactList.Core.Hateoas;

namespace ContactList.Core.Arguments.Company
{
    public class CompanyResponse : Recurso
    {
        public int Id { get; set; }
        public int ContactBookId{ get; set; }
        public string Name { get; set; }

        public static explicit operator CompanyResponse(Domain.Company entity)
        {
            return new CompanyResponse()
            {
                Id = entity.Id,
                ContactBookId = entity.ContactBookId,
                Name = entity.Name
            };
        }
    }
}
