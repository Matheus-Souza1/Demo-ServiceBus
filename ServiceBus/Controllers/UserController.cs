using Azure.Messaging.ServiceBus;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using ServiceBus.Model;

namespace ServiceBus.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly ServiceBusClient _client;
        private const string QUEUE_NAME = "notification-queue";

        public UserController(IConfiguration configuration)
        {
            _configuration = configuration;
            var connection = _configuration.GetSection("connection").Value;
            _client = new ServiceBusClient(connection);
        }

        [HttpPost]
        public async Task<IActionResult> Send([FromBody] User user)
        {
            var data = JsonConvert.SerializeObject(user);
            
            ServiceBusSender sender = _client.CreateSender(QUEUE_NAME);
            ServiceBusMessage message = new ServiceBusMessage(data);
            
            await sender.SendMessageAsync(message);
            return Ok();
        }
    }
}
