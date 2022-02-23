using Dapper.Contrib.Extensions;
using ContactList.Core.Extensions.Validator;

namespace ContactList.Core.Domain
{
    [Table("Company")]
    public class Company
    {
        [Key]
        public int Id { get; private set; }
        public int ContactBookId { get; private set; }
        public string Name { get; private set; }
        protected Company()
        {
        }
        public Company(int contactBookId, string name)
        {
            ContactBookId = ValidatorContactBookId(contactBookId);
            Name = StringValidator.Validating("Company Name", name, 3, 50);
        }
        public void Update(int contactBookId, string name)
        {
            ContactBookId = ValidatorContactBookId(contactBookId);
            Name = StringValidator.Validating("Company Name", name, 3, 50);
        }
        private static int ValidatorContactBookId(int contactBookId)
        {
            if (contactBookId < 0) throw new System.ComponentModel.DataAnnotations.
           ValidationException("ContactBook Id cannot be less than 0!");

            return contactBookId;
        }
    }
}
