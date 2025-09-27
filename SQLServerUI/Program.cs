using System;
using Microsoft.Extensions.Configuration;

namespace SQLServerUI;
class Program
{
    static void Main(string[] args)
    {
        Console.WriteLine(GetConnectionString());

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


