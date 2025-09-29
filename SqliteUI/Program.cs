using DataAccessLibrary;
using DataAccessLibrary.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.Json;
using SQLitePCL;

namespace SqliteUI
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Batteries.Init();

            SqliteCrud sqlite = new SqliteCrud(GetConnectionString());

            //ReadAllContacts(sqlite);
            //ReadContact(sqlite, 1);

            //CreateNewContact(sqlite);
            //Console.WriteLine("Created Contact!");

            //UpdateContact(sqlite);
            //ReadContact(sqlite, 1);

            RemovePhoneNumberFromContact(sqlite, 1, 1);

            Console.WriteLine("Done Processing Sqlite");


            Console.ReadLine();
        }

        private static void RemovePhoneNumberFromContact(SqliteCrud sql, int contactId, int phoneNumberId)
        {
            sql.RemovePhoneNumberFromContact(contactId, phoneNumberId);
        }

        private static void UpdateContact(SqliteCrud sql)
        {
            BasicContactModel contact = new BasicContactModel
            {
                Id = 1,
                FirstName = "Erin",
                LastName = "Williams",
            };

            sql.UpdateContactName(contact);
        }

        private static void CreateNewContact(SqliteCrud sql)
        {
            FullContactModel user = new FullContactModel
            {
                BasicInfo = new BasicContactModel
                {
                    FirstName = "Lilly",
                    LastName = "Milly"
                }
            };

            user.EmailAddresses.Add(new EmailAddressModel { EmailAddress = "Lilly@email.com" });
            user.EmailAddresses.Add(new EmailAddressModel { Id = 2, EmailAddress = "me@email.com" });

            user.PhoneNumbers.Add(new PhoneNumberModel { Id = 1, PhoneNumber = "555-1212" });
            user.PhoneNumbers.Add(new PhoneNumberModel { PhoneNumber = "555-1616" });

            sql.CreateContact(user);
        }

        private static void ReadAllContacts(SqliteCrud sql)
        {
            var rows = sql.GetAllContacts();

            foreach (var row in rows)
            {
                Console.WriteLine($"{row.Id}: {row.FirstName} {row.LastName}");
            }
        }

        private static void ReadContact(SqliteCrud sql, int contactId)
        {
            var contact = sql.GetFullContactById(contactId);

            Console.WriteLine($"{contact.BasicInfo.Id}: {contact.BasicInfo.FirstName} {contact.BasicInfo.LastName}");
        }

        private static string GetConnectionString(string connectionStringName = "Default")
        {
            string? output = "";

            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json");

            var config = builder.Build();

            output = config.GetConnectionString(connectionStringName);

            return output;
        }
    }
}
