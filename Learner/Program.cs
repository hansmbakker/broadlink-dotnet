using Broadlink.Net;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

class Program
{
    static void Main(string[] args)
    {
        var client = new Client();
        var discoveredDevices = client.DiscoverAsync().Result;

        if (discoveredDevices.Any())
        {
            var deviceToUse = discoveredDevices.FirstOrDefault();
            if (deviceToUse != null)
            {
                var rmDevice = deviceToUse as RMDevice;
                rmDevice.AuthorizeAsync().Wait();

                var commandDictionary = new Dictionary<string, string>();

                var input = string.Empty;
                while (true)
                {
                    Console.WriteLine("Enter the command name");
                    input = Console.ReadLine();

                    if(input == "exit")
                    {
                        break;
                    }

                    rmDevice.EnterLearningModeAsync().Wait();

                    Console.WriteLine("Teach the command");
                    Task.Delay(3000).Wait();
                    var data = rmDevice.ReadLearningDataAsync().Result;
                    var base64 = Convert.ToBase64String(data);
                    Console.WriteLine($"Received {base64}");

                    commandDictionary.Add(input, base64);
                }

                var json = JsonConvert.SerializeObject(commandDictionary);
                var commandFilePath = System.IO.Path.GetFullPath("..\\commands.json");
                var writer = System.IO.File.CreateText(commandFilePath);
                writer.WriteLine(json);
                writer.Dispose();
            }

        }
        Console.ReadLine();
    }
}