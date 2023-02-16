using ServiceBus.Service.Interface;

using Azure.Messaging.ServiceBus;
using Newtonsoft.Json;
using ServiceBus.Model;

namespace ServiceBus.Service
{
    public class WorkService : IWorkService
    {
        private ServiceBusClient client;
        private readonly ILogger<WorkService> _logger;
        private readonly IConfiguration configuration;

        public WorkService(ILogger<WorkService> logger, IConfiguration configuration)
        {
            this.configuration = configuration;
            _logger = logger;
        }
        public async Task Execute(CancellationToken cancellation)
        {
            var connection = configuration.GetSection("connection").Value;
            client = new ServiceBusClient(connection);

            while (!cancellation.IsCancellationRequested)
            {
                var receiver = client.CreateReceiver("notification-queue");
                var message = await receiver.ReceiveMessageAsync();
                try
                {
                    var body = message.Body.ToString();
                    var data = JsonConvert.DeserializeObject<User>(body);

                    await receiver.CompleteMessageAsync(message);
                }
                catch (System.Exception ex)
                {
                    _logger.LogError("Error occurred in - ", ex);
                    await receiver.DeadLetterMessageAsync(message);
                }
            }
        }
    }
}