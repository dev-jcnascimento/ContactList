using System.ComponentModel.DataAnnotations;

namespace ContactList.Core.Extensions.Validator
{
    public static class EmailValidator
    {
        public static string Validating(string webmail)
        {
            if (string.IsNullOrEmpty(webmail))
            {
                throw new ValidationException("Email cannot be empty");
            }
            if (!IsMail(webmail))
            {
                throw new ValidationException("The email provided is not valid");
            }
            return webmail;
        }
        private static bool IsMail(string email)
        {
            bool validEmail = false;
            int indexArr = email.IndexOf('@');
            if (indexArr > 0)
            {
                int indexDot = email.IndexOf('.', indexArr);
                if (indexDot - 1 > indexArr)
                {
                    if (indexDot + 1 < email.Length)
                    {
                        string indexDot2 = email.Substring(indexDot + 1, 1);
                        if (indexDot2 != ".")
                        {
                            validEmail = true;
                        }
                    }
                }
            }
            return validEmail;
        }
    }
}
