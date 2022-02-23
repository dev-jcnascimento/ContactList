using System.ComponentModel.DataAnnotations;

namespace ContactList.Core.Extensions.Validator
{
    public class StringValidator
    {
        public static string Validating(string ident, string value, int min, int max)
        {
            if (string.IsNullOrEmpty(value)) throw new ValidationException($"The {ident} cannot be empty!");
            if (value.Length < min || value.Length > max) throw new ValidationException($"The {ident} must contain more than {min} characters and less than {max}");

            return value;
        }
    }
}
