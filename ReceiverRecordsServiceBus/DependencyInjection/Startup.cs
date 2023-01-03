using Azure.Messaging.ServiceBus;
using MessageManager;
using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ReceiverRecordsServiceBus.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

[assembly: FunctionsStartup(typeof(ReceiverRecordsServiceBus.DependencyInjection.Startup))]
namespace ReceiverRecordsServiceBus.DependencyInjection
{
    internal class Startup : FunctionsStartup
    {
        public override void Configure(IFunctionsHostBuilder builder)
        {
            builder.Services.AddSingleton<IBusLogic, BusLogic>();

            var connectionString = Environment.GetEnvironmentVariable("AzureSB", EnvironmentVariableTarget.Process);

            builder.Services.AddSingleton(service =>
            {
                return new ServiceBusClient(connectionString, new ServiceBusClientOptions()
                {
                    TransportType = ServiceBusTransportType.AmqpWebSockets
                });
            });
        }
    }
}
