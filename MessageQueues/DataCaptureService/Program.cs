using MessageQueuesLibrary.Models;
using MessageQueuesLibrary.Options;
using Newtonsoft.Json;
using RabbitMQ.Client;
using System.Text;

namespace DataCaptureService
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


                var videoPackets = SplitFile(Options.Sourcefile);

                foreach (var videoPacket in videoPackets)
                {
                    var body = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(videoPacket));
                    channel.BasicPublish(exchange: "files",
                                     routingKey: "",
                                     basicProperties: null,
                                     body: body);
                }
                
               
            }

            Console.WriteLine(" Press [enter] to exit.");
            Console.ReadLine();
        }

        static IList<VideoPacket> SplitFile(string sourceFile)
        {
            var videoPackets = new List<VideoPacket>();
            try
            {
                FileStream fs = new FileStream(sourceFile, FileMode.Open, FileAccess.Read);
                int nNoofFiles = (int)Math.Ceiling((double)fs.Length / Options.SizeofEachFile);
                for (int i = 0; i < nNoofFiles; i++)
                {
                    string baseFileName = Path.GetFileNameWithoutExtension(sourceFile);
                    string Extension = Path.GetExtension(sourceFile);
                    int bytesRead = 0;
                    byte[] buffer = new byte[Options.SizeofEachFile];
                    if ((bytesRead = fs.Read(buffer, 0, Options.SizeofEachFile)) > 0)
                    {
                        string name = baseFileName + Extension.ToString();

                        videoPackets.Add(new VideoPacket() { Name = name, Data = buffer });
                    }
                    
                }
                fs.Close();
                
            }
            catch (Exception Ex)
            {
                throw new ArgumentException(Ex.Message);
            }

            return videoPackets;
        }
    }
}