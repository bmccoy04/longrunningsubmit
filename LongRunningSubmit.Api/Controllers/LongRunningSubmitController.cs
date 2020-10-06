
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LongRunningSubmit.Api.Hubs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using RabbitMQ.Client;
using Microsoft.AspNetCore.SignalR;
using System.Text.Json;

namespace LongRunningSubmit.Api.Controllers
{
    [ApiController]
    [Route("api/long-running-submit-controller")]
    public class LongRunningSubmitController : ControllerBase
    {
        private static readonly string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        private readonly ILogger<WeatherForecastController> _logger;
        private readonly IHubContext<NotificationHub, INotificationHub> _notificationHub;

        public LongRunningSubmitController(ILogger<WeatherForecastController> logger, IHubContext<NotificationHub, INotificationHub> notificationHub)
        {
            _logger = logger;
            _notificationHub = notificationHub;
        }

        [HttpGet]
        public IEnumerable<WeatherForecast> Get()
        {
            var rng = new Random();
            return Enumerable.Range(1, 5).Select(index => new WeatherForecast
            {
                Date = DateTime.Now.AddDays(index),
                TemperatureC = rng.Next(-20, 55),
                Summary = Summaries[rng.Next(Summaries.Length)]
            })
            .ToArray();
        }

        [HttpPost]
        public IActionResult Post([FromBody]LongRunningSubmission submission)
        {
            var factory = new ConnectionFactory(){HostName = "localhost", UserName = "guest", Password="guest"};
            using (var connection = factory.CreateConnection())
            using (var channel = connection.CreateModel())
            {
                channel.QueueDeclare(queue: "submission", durable: false, exclusive:false, autoDelete:false, arguments:null);
                var json = JsonSerializer.Serialize(submission);
                var body = Encoding.UTF8.GetBytes(json);
                channel.BasicPublish(exchange:"", routingKey: "submission", basicProperties:null, body:body);
            }

            return Ok(submission);
        }
    }

    public class LongRunningSubmission
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }    
        public string SomethingElse { get; set; }
    }
}
