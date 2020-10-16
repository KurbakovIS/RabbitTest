using System;
using RabbitMQ.Client;
using System.Text;
using Newtonsoft.Json.Linq;

namespace RabbitTest
{
    class Program
    {
        static void Main(string[] args)
        {
            var factory = new ConnectionFactory() { HostName = "localhost" };
            using var connection = factory.CreateConnection() ;
            using var chanell = connection.CreateModel();

            var queueName = chanell.QueueDeclare().QueueName;

            chanell.ExchangeDeclare("logs", ExchangeType.Fanout);

            var message = GetMessage(args);
            var body = Encoding.UTF8.GetBytes(message.ToString());

            chanell.BasicPublish(exchange: "logs",
                              routingKey: "",
                              basicProperties: null,
                              body: body);

            Console.WriteLine(" [x] Sent {0}", message);

            Console.WriteLine(" Press [enter] to exit.");
            Console.ReadLine();
        }

        private static JObject GetMessage(string[] args)
        {
            JObject o = JObject.FromObject(new
            {
                channel = new
                {
                    title = "James Newton-King",
                    link = "http://james.newtonking.com",
                    description = "James Newton-King's blog.",
                }
            });

            return o;
        }
    }
}
