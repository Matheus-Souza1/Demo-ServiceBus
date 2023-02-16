using Microsoft.Extensions.Hosting;
using ServiceBus.Service.Interface;
using System.Threading;
using System.Threading.Tasks;

namespace ServiceBus.HostedService
{
    public class ConsumerService : BackgroundService
    {
        private readonly IWorkService _service;

        public ConsumerService(IWorkService service)
        {
            _service = service;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            await _service.Execute(stoppingToken);
        }
    }
}
