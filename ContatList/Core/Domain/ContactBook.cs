using Dapper.Contrib.Extensions;
using ContactList.Core.Extensions.Validator;

namespace ContactList.Core.Domain
{
    [Table("ContactBook")]
    public class ContactBook
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        protected ContactBook()
        {
        }
        public ContactBook(string name)
        {
            Name = StringValidator.Validating("ContactBook Name", name, 3, 50);
        }
        public void Update(string name)
        {
           Name = StringValidator.Validating("ContactBook Name", name, 3, 50);
        }
    }
}
