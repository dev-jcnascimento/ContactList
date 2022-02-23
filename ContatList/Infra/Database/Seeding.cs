using Dapper.Contrib.Extensions;
using Microsoft.AspNetCore.Builder;
using Microsoft.Data.Sqlite;
using Microsoft.Extensions.DependencyInjection;
using System.Collections.Generic;
using System.Linq;
using ContactList.Core.Domain;
using ContactList.Database;

namespace ContactList.Infra.Database
{
    public static class Seeding
    {
        public static void Initialize(IApplicationBuilder app)
        {
            using var serviceScope = app.ApplicationServices.CreateScope();
            var databaseConfig = serviceScope.ServiceProvider.GetService<DatabaseConfig>();

            bool contact = false;
            bool contactBook = false;
            bool company = false;

            using var connection = new SqliteConnection(databaseConfig.ConnectionString);
            contact = connection.GetAll<Contact>().Any();
            contactBook = connection.GetAll<ContactBook>().Any();
            company = connection.GetAll<Company>().Any();

            if (contact == true || contactBook == true || company == true)
            {
                return;
            }
            var contactBookList = new List<ContactBook>();

            var contactBook01 = new ContactBook("ContactBook Fake 01");
            var contactBook02 = new ContactBook("ContactBook Fake 01");
            var contactBook03 = new ContactBook("ContactBook Fake 01");

            contactBookList.Add(contactBook01);
            contactBookList.Add(contactBook02);
            contactBookList.Add(contactBook03);

            connection.Insert(contactBookList);

            var companyList = new List<Company>();

            var company01 = new Company( 1, "Company Fake 01");
            var company02 = new Company(3, "Company Fake 02");
            var company03 = new Company(2, "Company Fake 03");
            var company04 = new Company(1, "Company Fake 04");

            companyList.Add(company01);
            companyList.Add(company02);
            companyList.Add(company03);
            companyList.Add(company04);

            connection.Insert(companyList);

            var contactList = new List<Contact>();

            var contactList01 = new Contact(1, 0, "User 01", "Fake 1", "2233335555",
           "12345678A", "user1@fake.com.br", "Street Afonso Pena, N 26, Rio de Janeiro, Brasil");
            var contactList02 = new Contact(3, 2, "User 02", "Fake 2", "2233335555",
           "12345678B", "user2@fake.com.br", "Street Afonso Pena, N 26, Rio de Janeiro, Brasil");
            var contactList03 = new Contact(1, 2, "User 03", "Fake 3", "2233335555",
           "12345678C", "user3@fake.com.br", "Street Afonso Pena, N 26, Rio de Janeiro, Brasil");
            var contactList04 = new Contact(2, 3, "User 04", "Fake 5", "2233335555",
           "12345678D", "user4@fake.com.br", "Street Afonso Pena, N 26, Rio de Janeiro, Brasil");
            var contactList05 = new Contact(3, 1, "User 05", "Fake 5", "2233335555",
           "12345678E", "user5@fake.com.br", "Street Afonso Pena, N 26, Rio de Janeiro, Brasil");
            var contactList06 = new Contact(1, 3, "User 06", "Fake 6", "2233335555",
           "12345678F", "user6@fake.com.br", "Street Afonso Pena, N 26, Rio de Janeiro, Brasil");
            var contactList07 = new Contact(2, 2, "User 07", "Fake 7", "2233335555",
           "12345678G", "user7@fake.com.br", "Street Afonso Pena, N 26, Rio de Janeiro, Brasil");
            var contactList08 = new Contact(1, 2, "User 08", "Fake 8", "2233335555",
           "12345678H", "user8@fake.com.br", "Street Afonso Pena, N 26, Rio de Janeiro, Brasil");

            contactList.Add(contactList01);
            contactList.Add(contactList02);
            contactList.Add(contactList03);
            contactList.Add(contactList04);
            contactList.Add(contactList05);
            contactList.Add(contactList06);
            contactList.Add(contactList07);
            contactList.Add(contactList08);
            
            connection.Insert(contactList);
        }
    }
}
