using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Azure.Messaging.ServiceBus;
using MessageManager;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Extensions.Logging;

namespace ReceiverRecordsServiceBus
{
    public class EnqueueMessage
    {
        private readonly IBusLogic busLogic;
        private readonly ServiceBusClient serviceBusClient;

        public EnqueueMessage(IBusLogic busLogic, ServiceBusClient serviceBusClient)
        {
            this.busLogic = busLogic;
            this.serviceBusClient = serviceBusClient;
            this.busLogic.SetInstance(serviceBusClient);
        }
        [FunctionName("DequeueMessage")]
        public async Task Run([TimerTrigger("0 */1 * * * *")]TimerInfo myTimer, ILogger log, CancellationToken cancellationToken)
        {
            try
            {
                List<bool> messagesResult = await this.busLogic.GetQueue(cancellationToken, Environment.GetEnvironmentVariable("databaseConnection", EnvironmentVariableTarget.Process));
                if(messagesResult.Count > 0) {
                    log.LogInformation($"CANTIDAD DE VUELOS RECIBIDOS {messagesResult.Count}");
                }
                else
                {
                    log.LogInformation($"SIN VUELOS PENDENTES DE REGISTRO");
                }
            }
            catch (Exception ex)
            {
                log.LogInformation(ex.Message);
            }
        }
    }
}
