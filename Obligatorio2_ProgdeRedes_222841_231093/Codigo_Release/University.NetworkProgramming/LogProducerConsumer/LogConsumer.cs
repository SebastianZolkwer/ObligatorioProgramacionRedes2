using Bussiness.Domain;
using DataAccessInterface;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Text;

namespace LogProducerConsumer
{
    public class LogConsumer
    {
        private const string SimpleQueueName = "n6aBasicQueue";

        private ILogRepository logRepository;

        public LogConsumer (ILogRepository _logRepository)
        {
            this.logRepository = _logRepository;
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
                Log log = ConvertToLog(message);
                logRepository.Add(log);
                Console.WriteLine("Received message : " + message);
            };

            channel.BasicConsume(
                queue: SimpleQueueName,
                autoAck: true,
                consumer: consumer);
        }

        private Log ConvertToLog(string message)
        {
            string[] data = message.Split("-");
            Log newLog = new Log()
            {
                clientName = data[0],
                gameTitle = data[1],
                date = data[2],
                message = data[3]
            };
            return newLog;

        }
    }
}

