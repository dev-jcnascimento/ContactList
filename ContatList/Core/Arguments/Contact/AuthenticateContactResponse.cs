namespace ContactList.Core.Arguments.Contact
{
    public class AuthenticateContactResponse
    {
        public int Id { get; set; }
        public string FullName  { get; set; }
        public string Email { get; set; }

        public static explicit operator AuthenticateContactResponse(Domain.Contact entity)
        {
            return new AuthenticateContactResponse()
            {
                Id = entity.Id,
                FullName = entity.ToString(),
                Email = entity.Email
            };
        }
    }
}
