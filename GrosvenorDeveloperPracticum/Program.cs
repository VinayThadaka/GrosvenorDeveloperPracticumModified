using System;
using System.Linq;
using Application;
using Application.Enums;
using Microsoft.Extensions.DependencyInjection;
using Serilog;
using Serilog.Core;

namespace GrosvenorInHousePracticum
{
    class Program
    {
        static void Main()
        {
            // Configure Serilog
            Log.Logger = new LoggerConfiguration()
                .WriteTo.Console()
                .CreateLogger();

            // Set up dependency injection
            var serviceProvider = new ServiceCollection()
                .AddSingleton<ILogger>((Logger)Log.Logger)
                .AddTransient<IDishManager, DishManager>()
                .AddTransient<IServer, Server>()
                .BuildServiceProvider();

            var server = serviceProvider.GetService<IServer>();
            while (true)
            {
                Console.WriteLine("Enter time of day (morning/evening) followed by the order:");
                var input = Console.ReadLine().Split(',');
                var timeOfDay = input[0].Trim().ToLower() == "morning" ? TimeOfDay.Morning : TimeOfDay.Evening;
                var unparsedOrder = string.Join(",", input.Skip(1));
                var output = server.TakeOrder(unparsedOrder,timeOfDay);
                Console.WriteLine(output);
            }
        }
    }
}
