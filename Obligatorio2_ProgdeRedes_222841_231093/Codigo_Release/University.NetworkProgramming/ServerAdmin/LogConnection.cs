using RabbitMQ.Client;
using System;
using System.Text;
using System.Threading.Tasks;

namespace ServerAdmin
{
    static class LogConnection
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

        public static void PublishMessage( string message)
        {
            var factory = new ConnectionFactory { HostName = "localhost" };
            using IConnection connection = factory.CreateConnection();
            using IModel channel = connection.CreateModel();
            DeclareQueue(channel);
            byte[] body = Encoding.UTF8.GetBytes(message);

            channel.BasicPublish(
                exchange: "",
                routingKey: SimpleQueueName,
                basicProperties: null,
                body: body);
        }
    }
}
