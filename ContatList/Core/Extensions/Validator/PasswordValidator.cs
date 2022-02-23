using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace ContactList.Core.Extensions.Validator
{
    public static class PasswordValidator
    {
        public static string Validating(string password)
        {
            if (string.IsNullOrEmpty(password))
            {
                throw new ValidationException("Password cannot be empty");
            }
            if (password.Length <= 7)
            {
                throw new ValidationException("Password must be longer than 8 characters");
            }
            if (!password.Any(x => char.IsUpper(x))) 
            {
                throw new ValidationException("Password must contain at least one uppercase character");
            }
            var word = PasswordConvert.ConvertToMD5(password);
            return word;
        }
    }
}
