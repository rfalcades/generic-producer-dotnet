
using Microsoft.Extensions.Logging;

namespace generic_producer_dotnet
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddControllers();
            builder.Services.AddLogging();
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddTransient<RabbitProducer>();
            builder.Services.AddTransient<Worker>();
            //builder.Services.AddSwaggerGen();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                //app.UseSwagger();
                //app.UseSwaggerUI();
            }

            app.UseAuthorization();

            app.UseMiddleware<ProducerInterceptorMiddleware>();

            app.MapControllers();

            app.Run();
        }
    }
}