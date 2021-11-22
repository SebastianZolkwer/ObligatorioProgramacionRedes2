using RabbitMQ.Client;
using System;
using System.Text;
using System.Threading.Tasks;

namespace ServerAdmin
{
    static class LogConnection
    {
        private const string SimpleQueueName = "n6aBasicQueue";
        private const string ExitMessage = "exit";
        private static IModel channel;

        public static void  SetChannel(IConnection connection)
        {
            if(channel is null)
            {
                using IModel channel = connection.CreateModel();
                DeclareQueue(channel);
            }
        }

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
            using IModel channel2 = connection.CreateModel();
            DeclareQueue(channel2);
            byte[] body = Encoding.UTF8.GetBytes(message);

            channel2.BasicPublish(
                exchange: "",
                routingKey: SimpleQueueName,
                basicProperties: null,
                body: body);
        }
    }
}
