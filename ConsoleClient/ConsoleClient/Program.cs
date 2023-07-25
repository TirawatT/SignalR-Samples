using ConsoleClient.Models;
using Microsoft.AspNetCore.SignalR.Client;
using System.Runtime.InteropServices;

namespace ConsoleClient
{
    internal class Program
    {
        private static HubConnection _hub;

        static void Main(string[] args)
        {
            Console.WriteLine("### Start ###");

            InitHub();
            Connect();

            Console.WriteLine("waiting");
            Console.WriteLine("Press ESC to stop");
            Console.WriteLine("Press 'S' to SendToHub \t 'T' to SendToHub");
            ConsoleKey keyPress;
            do
            {
                while (!Console.KeyAvailable)
                {
                    // Do something

                }
                keyPress = Console.ReadKey(true).Key;
                if (keyPress == ConsoleKey.S)
                {
                    SendToHub();
                }
                else if (keyPress == ConsoleKey.T)
                {
                    SendToHubTest();
                }
            } while (keyPress != ConsoleKey.Escape);


            Close();
            _hub.DisposeAsync();
            Console.WriteLine("### End ###");
        }
        static public void InitHub()
        {
            var url = "https://localhost:7121/ChatHub"; // -- HUB Server url
            _hub = new HubConnectionBuilder()
            .WithUrl(url)
            .WithAutomaticReconnect()
            .Build();

            // -- declare function
            _hub.On<ChartHubDto>("Connected", OnConnected);
            // -- inline function
            _hub.Closed += async (error) =>
            {
                Console.WriteLine($"close , state : {_hub.State} , {error?.Message}");
                await Task.Delay(new Random().Next(0, 5) * 1000);
                await _hub.StartAsync();
            };
            _hub.Reconnecting += error =>
            {
                Console.WriteLine($"Reconnecting , state : {_hub.State} , {error?.Message}");
                return Task.CompletedTask;
            };
            _hub.On<string, string>("ReceiveMessage", (user, message) =>
            {
                Console.WriteLine($"ReceiveMessage , {user} message: {message}");
            });
            _hub.On<string>("ServerInterval", (message) =>
            {
                Console.WriteLine($"ServerInterval ,  message: {message}");

            });
            _hub.On<string, string>("ClientSetup", (user, message) =>
            {
                Console.WriteLine($"ClientSetup , {user} message: {message}");

            });
            _hub.On<string, string>("Test", (user, message) =>
            {
                Console.WriteLine($"Test , {user} message: {message}");

            });
        }

        public static void OnConnected(ChartHubDto data)
        {
            Console.WriteLine($"OnConnected , {data.User} message: {data.Message}");
        }
        public static void Connect()
        {
            _hub.StartAsync();
        }
        public static void Close()
        {
            _hub.StopAsync();
        }
        private static void SendToHub()
        {
            _hub.SendAsync("ClientSetup", "111", "222");
        }
        private static void SendToHubTest()
        {
            _hub.SendAsync("Test", "111", "222");
        }
    }
}