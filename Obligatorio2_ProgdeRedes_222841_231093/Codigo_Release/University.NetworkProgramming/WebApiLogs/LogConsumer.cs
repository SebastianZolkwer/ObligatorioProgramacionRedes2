using Bussiness.Domain;
using DataAccess;
using DataAccessInterface;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Text;

namespace LogProducerConsumer
{
    public class LogConsumer
    {
        private const string SimpleQueueName = "BasicQueue";

        public static void DeclareQueue(IModel channel)
        {

            channel.QueueDeclare(
                queue: SimpleQueueName,
                durable: false,
                exclusive: false,
                autoDelete: false,
                arguments: null);
        }

        public static void ReceiveMessages(IModel channel)
        {
            var consumer = new EventingBasicConsumer(channel);
            consumer.Received += (model, ea) =>
            {
                byte[] body = ea.Body.ToArray();
                string message = Encoding.UTF8.GetString(body);
                Log log = new Log(message);
                LogRepository.Add(log);
            };

            channel.BasicConsume(
                queue: SimpleQueueName,
                autoAck: true,
                consumer: consumer);
        }
    }
}
