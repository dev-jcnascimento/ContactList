using System.Collections.Generic;
using System.IO;
using ContactList.Core.Domain;

namespace ContactList.Core.Extensions
{
    public static class WriteContactInFile
    {
        public static void WriteFile(this string url, List<Contact> contacts)
        {
            string sourceFolderPath = Path.GetDirectoryName(url);
            string targetFolderPath = sourceFolderPath + @"\out";
            string targetFilePath = targetFolderPath + @"\contactNew.csv";

            Directory.CreateDirectory(targetFolderPath);

            using (StreamWriter sw = File.AppendText(targetFilePath))
            {
                foreach (var line in contacts)
                {
                    sw.WriteLine($"{line.ContactBookId}, {line.CompanyId}, {line.FirstName}, " +
                        $"{line.LastName}, {line.Phone}, {line.Email}, {line.Address}");
                }
            }
        }
    }
}
