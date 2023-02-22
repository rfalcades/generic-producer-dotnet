using RabbitMQ.Client;
using System.Text;

namespace generic_producer_dotnet
{
    public class RabbitProducer
    {

        private readonly ILogger<RabbitProducer> _logger;

        public RabbitProducer(ILogger<RabbitProducer> logger)
        {
            this._logger = logger;
        }

        public async Task Publish(string topicName, string message) 
        {
            await Task.Run(() =>
            {
                var exchange = topicName;

                var factory = new ConnectionFactory { HostName = "localhost" };
                using var connection = factory.CreateConnection();
                using var channel = connection.CreateModel();

                var exchangeType = "topic"; //"queue"


                if (exchangeType == "topic")
                {
                    channel.ExchangeDeclare(exchange, "topic");
                }
                else
                {
                    channel.QueueDeclare(queue: exchange, durable: true, exclusive: false, autoDelete: false, arguments: null);
                }

                var body = Encoding.UTF8.GetBytes(message);

                channel.BasicPublish(exchange: exchange, routingKey: "a.b", basicProperties: null, body: body);
            });
        }
    }
}
