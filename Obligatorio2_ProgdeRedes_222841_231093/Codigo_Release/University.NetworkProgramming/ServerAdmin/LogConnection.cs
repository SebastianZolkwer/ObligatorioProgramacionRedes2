using RabbitMQ.Client;
using System;
using System.Text;
using System.Threading.Tasks;

namespace ServerAdmin
{
    public class LogConnection
    {
       
        private const string SimpleQueueName = "BasicQueue";
        private const string ExchangeName = "BasicLogs";
        private IModel channel { get; set; }

        public LogConnection()
        {
            var factory = new ConnectionFactory { HostName = "localhost" };
            using IConnection connection = factory.CreateConnection();
            channel = connection.CreateModel();
            DeclareExchange(channel);
            DeclareQueue(channel);
            channel.QueueBind(
                queue: SimpleQueueName,
                exchange: ExchangeName,
                routingKey: SimpleQueueName
                );
        }

        private void DeclareExchange(IModel channel)
        {
            channel.ExchangeDeclare(
                exchange: ExchangeName,
                type: ExchangeType.Fanout
                );
        }

        public  void DeclareQueue(IModel channel)
        {
            channel.QueueDeclare(
                queue: SimpleQueueName,
                durable: false,
                exclusive: false,
                autoDelete: false,
                arguments: null);
        }

        public  void PublishMessage( string message)
        {
            byte[] body = Encoding.UTF8.GetBytes(message);
            channel.BasicPublish(
                exchange: ExchangeName,
                routingKey: SimpleQueueName,
                basicProperties: null,
                body: body
                );
        }
    }
}
