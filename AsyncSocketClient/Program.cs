using AsyncSocketClient;
using Microsoft.Extensions.Configuration;

var appConfiguration = new ConfigurationBuilder()
    .SetBasePath($"{Directory.GetCurrentDirectory()}/../../../")
    .AddJsonFile("appsettings.json", optional: false)
    .Build();

var serverConfiguration = appConfiguration.GetSection("Configuration").Get<ServerConfiguration>();

var client = new Client();
client.Connect(serverConfiguration);

string input;
do
{
    input = Console.ReadLine() ?? string.Empty;
    if(input != "<EXIT>")
    {
        await client.Send(input);
    }

} while (input != "<EXIT>");