using Bussiness.Domain;
using DataAccess;
using DataAccessInterface;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Text;
using System.Threading.Tasks;

namespace LogProducerConsumer
{
    public class LogConsumer
    {
        private const string SimpleQueueName = "BasicQueue";

        public static void StartService()
        {
            var factory = new ConnectionFactory { HostName = "localhost" };
            using IConnection connection = factory.CreateConnection();
            using IModel channel = connection.CreateModel();
            Task.Run(() => ReceiveMessages(channel));

        }
        public static void DeclareQueue(IModel channel)
        {
            //Name (queue name)
            //Durable(the queue will survive a broker restart)
            //Exclusive(used by only one connection and the queue will be deleted when that connection closes)
            //Auto - delete(queue that has had at least one consumer is deleted when last consumer unsubscribes)
            //Arguments(optional; used by plugins and broker - specific features such as message TTL, queue length limit, etc)

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
