using System.Text;

namespace ContactList.Core.Extensions
{
    public static class PasswordConvert
    {
        public static string ConvertToMD5(string passWord)
        {
            if (string.IsNullOrEmpty(passWord)) return "";
            var password = passWord += "|2d131cca-f6c0-41c0-bb43-6e32959c2841";
            var md5 = System.Security.Cryptography.MD5.Create();
            var data = md5.ComputeHash(Encoding.Default.GetBytes(password));
            var sbString = new StringBuilder();
            foreach (var t in data)
                sbString.Append(t.ToString("x2"));

            return sbString.ToString();
        }
    }
}
