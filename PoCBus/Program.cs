// See https://aka.ms/new-console-template for more information
using System.Text;
using Azure.Messaging.ServiceBus;
using Microsoft.Extensions.Configuration;


var builder = new ConfigurationBuilder()
 .AddUserSecrets<Program>();

var configuration = builder.Build();

string sbConnectionString = configuration["ServiceBusConnectionString"];
Console.WriteLine($"Sb ConnectionString: {sbConnectionString}");

string queueName = configuration["queueName2"];
Console.WriteLine($"queueName1: {queueName}");

if(sbConnectionString is  null || queueName is null)
{
    Console.WriteLine("Error de configuración!");
    Console.ReadKey();
}

Console.WriteLine("Inicio exportacion de mensajes de la cola!");
await ServiceBusExportMessage(sbConnectionString, queueName);
Console.WriteLine("Terminacion exportacion de mensajes de la cola!");

Console.ReadKey();


static async Task ServiceBusExportMessage(string serviceBusConnectionString, string queueName)
{
    await using var client = new ServiceBusClient(serviceBusConnectionString);
    ServiceBusReceiver receiver = client.CreateReceiver(queueName);
    string timestamp = DateTime.Now.ToString("yyyyMMdd_HHmm");
    string fileName = $"mensajes_pendientes-{queueName}-{timestamp}.txt";
    using StreamWriter file = new StreamWriter(fileName, append: false, encoding: Encoding.UTF8);
    int i=0;
    while (true)
    {
        var messages = await receiver.PeekMessagesAsync(maxMessages: 5000);//Recibir sin eliminarlos
        i++;
        Console.WriteLine($"Recuperó {i*5000}");

        if (messages.Count == 0)
        {
            break;
        }

        foreach (var message in messages)
        {
            // Escribir el contenido del mensaje en el archivo
            await file.WriteLineAsync(message.Body.ToString());
        }
        Console.WriteLine($"Almacenó {i * 5000}");
    }

    Console.WriteLine($"Mensajes guardados en '{fileName}'");

}




