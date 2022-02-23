using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace ContactList.Core.Extensions.Validator
{
    public static class PhoneValidator
    {
        public static string Validating(string phone)
        {
            if (phone.Length < 10)
            {
                throw new ValidationException("The phone number must contain more than 10 digits!");
            }
            if (phone.Any(x => !char.IsNumber(x)))
            {
                throw new ValidationException("The phone number cannot contain letters and special characters");
            }
            return phone;
        }
    }
}
