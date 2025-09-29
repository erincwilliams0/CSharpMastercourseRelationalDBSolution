using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.Json;

namespace SqliteUI
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine(GetConnectionString());
            //SqlCrud sql = new SqlCrud(GetConnectionString());

            //ReadAllContacts(sql);
            //ReadContact(sql, 1);

            //CreateNewContact(sql);
            //Console.WriteLine("Created Contact!");

            //UpdateContact(sql);
            //ReadContact(sql, 1);

            //RemovePhoneNumberFromContact(sql, 1, 1);

            Console.WriteLine("Done Processing Sqlite");


            Console.ReadLine();
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
