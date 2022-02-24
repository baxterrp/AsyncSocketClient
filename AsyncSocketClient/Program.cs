using AsyncSocketClient;
using Microsoft.Extensions.Configuration;
using System.Text.Json;

var appConfiguration = new ConfigurationBuilder()
    .SetBasePath($"{Directory.GetCurrentDirectory()}/../../../")
    .AddJsonFile("appsettings.json", optional: false)
    .Build();

var serverConfiguration = appConfiguration.GetSection("Configuration").Get<ServerConfiguration>();

Console.WriteLine("Enter a username:");
var username = Console.ReadLine();

username = !string.IsNullOrWhiteSpace(username) ? username : $"user-{Guid.NewGuid()}";

try
{
    var client = new Client();
    client.Connect(serverConfiguration);

    var user = new User
    {
        Id = Guid.NewGuid().ToString(),
        UserName = username
    };

    Console.Clear();
    Console.WriteLine($"Welcome, {username}. Enter a message and hit enter to share.");

    string input;
    do
    {
        input = Console.ReadLine() ?? string.Empty;

        if (input != "<EXIT>")
        {
            if (string.IsNullOrWhiteSpace(input))
            {
                Console.WriteLine("You no send blank space - SHAME");
                continue;
            }

            var package = new MessengerPackage
            {
                Message = input,
                User = user,
                SentDate = DateTime.Now
            };

            var serializedPackage = JsonSerializer.Serialize(package);

            await client.Send(serializedPackage);
        }

    } while (input != "<EXIT>");
}
catch
{
    Console.WriteLine($"Oops! you done fucked up {username}");
}