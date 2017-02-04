using Broadlink.Net;
using System;
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
            if(deviceToUse != null)
            {
                var rmDevice = deviceToUse as RMDevice;
                rmDevice.AuthorizeAsync().Wait();
                var temp = rmDevice.GetTemperatureAsync().Result;
                rmDevice.EnterLearningModeAsync().Wait();
                var data = rmDevice.ReadLearningDataAsync().Result;
                Console.WriteLine(BitConverter.ToString(data));
                rmDevice.SendRemoteCommandAsync(data).Wait();
            }
            
        }
        Console.ReadLine();
    }
}