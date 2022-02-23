using AsyncSocketClient;

var client = new Client();
client.Connect();

string input;
do
{
    input = Console.ReadLine();
    if(input != "<EXIT>")
    {
        await client.Send(input);
    }

} while (input != "<EXIT>");