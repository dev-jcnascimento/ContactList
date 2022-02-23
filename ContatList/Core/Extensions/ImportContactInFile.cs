using System;
using System.Collections.Generic;
using System.IO;
using ContactList.Core.Domain;

namespace ContactList.Core.Extensions
{
    public static class ImportContactInFile
    {
        public static List<Contact> Read(this string url)
        {
            List<Contact> fileContacts = new List<Contact>();
            try
            {
                string[] lines = File.ReadAllLines(url);

                for (int i = 0; i < lines.Length; i++)
                {
                    try
                    {
                        string[] fields = lines[i].Split(',');
                        int contactBookId = int.Parse(fields[0]);
                        int companyId = int.Parse(fields[1]);
                        string firstName = fields[2];
                        string lastName = fields[3];
                        string phone = fields[4];
                        string password = fields[5];
                        string email = fields[6];
                        string address = fields[7];

                        var contact = new Contact(
                        contactBookId,
                        companyId,
                        firstName,
                        lastName,
                        phone,
                        password,
                        email,
                        address);

                        fileContacts.Add(contact);
                    }
                    catch (Exception)
                    {
                        continue;
                    }
                }

            }
            catch (UnauthorizedAccessException)
            {
                throw new UnauthorizedAccessException("Arquivo não encontrado");
            }
            catch (IndexOutOfRangeException)
            {
                throw new IndexOutOfRangeException("Erro na leitura do arquivo");
            }
            catch (IOException)
            {
                throw new IOException("A sintaxe do nome do arquivo, do nome do diretório ou do rótulo do volume está incorreta");
            }

            return fileContacts;
        }
    }
}
