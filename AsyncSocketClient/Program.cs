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

var client = new Client();
client.Connect(serverConfiguration);

var user = new User
{
    Id = Guid.NewGuid().ToString(),
    UserName = username
};

string input;
do
{
    input = Console.ReadLine() ?? string.Empty;

    if(input != "<EXIT>")
    {
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