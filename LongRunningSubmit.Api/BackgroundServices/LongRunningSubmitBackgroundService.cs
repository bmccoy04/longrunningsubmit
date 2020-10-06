using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using LongRunningSubmit.Api.Hubs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace LongRunningSubmit.Api.BackgroundServices
{
    public class LongRunningSubmitBackgroundService : BackgroundService
    {
        private IConnection _connection;
        private IModel _channel;
        private IHubContext<NotificationHub, INotificationHub> _notificationHub;

        public LongRunningSubmitBackgroundService(IHubContext<NotificationHub, INotificationHub> notificationHub)
        {
            InitializeRabbitMqListener();
            _notificationHub = notificationHub;
        }

        private void InitializeRabbitMqListener()
        {
            var factory = new ConnectionFactory
            {
                HostName = "localhost",
                UserName = "guest",
                Password = "guest" 
            };

            _connection = factory.CreateConnection();
            _channel = _connection.CreateModel();
            _channel.QueueDeclare(queue: "submissionComplete", durable: false, exclusive: false, autoDelete: false, arguments: null);
        }

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            var consumer = new EventingBasicConsumer(_channel);
            consumer.Received += (ch, ea) =>
            {
                HandleMessage(ea.Body);

                _channel.BasicAck(ea.DeliveryTag, false);
            };

            _channel.BasicConsume("submissionComplete", false, consumer);

            return Task.CompletedTask;
        }

        private async void HandleMessage(ReadOnlyMemory<byte> body)
        {
            await _notificationHub.Clients.All.SendMessage("bryan", "your long task is complete");
        }

        public override void Dispose()
        {
            _channel.Close();
            _connection.Close();
            base.Dispose();
        }
    }
}