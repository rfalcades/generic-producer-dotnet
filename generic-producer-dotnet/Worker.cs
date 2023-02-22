using System.Text;

namespace generic_producer_dotnet
{
    public class Worker
    {
        private readonly RabbitProducer rabbitProducer;
        private readonly IList<string>  allowedEntities;
        private readonly ILogger<Worker> logger;

        public Worker(RabbitProducer rabbitProducer, IConfiguration configuration, ILogger<Worker> logger)
        {
            this.rabbitProducer = rabbitProducer;
            this.allowedEntities = configuration.GetValue<string>("AllowedEntities").Split(',').ToList();
            this.logger = logger;
        }

        public async Task Send(string topic, string content) 
        {
            await this.rabbitProducer.Publish(topic, content);
        }

        public async Task<string> GetBodyContent(Stream body)
        {
            using var reader = new StreamReader(body, Encoding.UTF8);
            return await reader.ReadToEndAsync();
        }

        public async Task<string> GetEntityName(string pathUrl)
        {
            return await Task.Run(() =>
            {
                if (String.IsNullOrWhiteSpace(pathUrl))
                    return string.Empty;

                var parts = pathUrl.Split("/");

                if (parts.Length > 1)
                    if (this.allowedEntities.Contains(parts[1].ToUpper()))
                        return parts[1];

                logger.LogInformation($"Entity not found! Path: {pathUrl}");
                return string.Empty;
            });            
        }
    }
}
