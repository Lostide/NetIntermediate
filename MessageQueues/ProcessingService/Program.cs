using DataCaptureService;
using MessageQueuesLibrary.Models;
using MessageQueuesLibrary.Options;
using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;

namespace ProcessingService
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var factory = new ConnectionFactory() { Uri = new Uri(Options.amqpConnectionURI) };
            using (var connection = factory.CreateConnection())
            using (var channel = connection.CreateModel())
            {

                channel.ExchangeDeclare(exchange: "files", type: ExchangeType.Fanout);

                var queueName = channel.QueueDeclare().QueueName;

                channel.QueueBind(queue: queueName,
                              exchange: "files",
                              routingKey: "");

                Console.WriteLine(" [*] Waiting for messages.");

                var consumer = new EventingBasicConsumer(channel);
                consumer.Received += (model, ea) =>
                {
                    var body = ea.Body.ToArray();
                    var message = Encoding.UTF8.GetString(body);
                    var packet = JsonConvert.DeserializeObject<VideoPacket>(message);
                    Console.WriteLine($"Recieved packet of {packet.Name}");
                    var stream = new FileStream(Options.SaveFileFolder + packet.Name, FileMode.Append);
                    stream.Write(packet.Data, 0, packet.Data.Length);
                    stream.Close();
                };
                channel.BasicConsume(queue: queueName,
                                     autoAck: true,
                                     consumer: consumer);

                Console.WriteLine(" Press [enter] to exit.");
                Console.ReadLine();
            }
        }
    }
}