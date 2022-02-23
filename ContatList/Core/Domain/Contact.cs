using Dapper.Contrib.Extensions;
using System;
using ContactList.Core.Extensions.Validator;

namespace ContactList.Core.Domain
{
    [Table("Contact")]
    public class Contact
    {
        [Key]
        public int Id { get;private set; }
        public int ContactBookId { get; private set; }
        public int CompanyId { get; private set; }
        public string FirstName { get; private set; }
        public string LastName { get; private set; }
        public string Phone { get; private set; }
        public string Password { get; private set; }
        public string Email { get; private set; }
        public string Address { get; private set; }
        protected Contact()
        {
        }
        public Contact(int contactBookId, int companyId, string firstName, string lastName, 
            string phone, string password, string email, string address)
        {
            ContactBookId = contactBookId;
            CompanyId = companyId;
            FirstName = StringValidator.Validating("Contact FirstName", firstName,3,50) ;
            LastName = StringValidator.Validating("Contact LastName", lastName, 3, 50);
            Phone = PhoneValidator.Validating(phone);
            Password = PasswordValidator.Validating(password);
            Email = EmailValidator.Validating(email);
            Address = address;
        }

        internal string[] Split(char v)
        {
            throw new NotImplementedException();
        }

        public void Update(int contactBookId, int companyId, string firstName, string lastName,
            string phone, string email, string address)
        {
            ContactBookId = contactBookId;
            CompanyId = companyId;
            FirstName = StringValidator.Validating("Contact FirstName", firstName, 3, 50);
            LastName = StringValidator.Validating("Contact LastName", lastName, 3, 50);
            Phone = PhoneValidator.Validating(phone);
            Email = EmailValidator.Validating(email);
            Address = address;
        }
        public override string ToString()
        {
            return $"{FirstName} {LastName}";
        }
    }
}
