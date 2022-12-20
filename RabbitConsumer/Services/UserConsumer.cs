using System.Text.Json;
using MassTransit;
using RabbitModel;


namespace RabbitConsumer.Services
{
    public class UserConsumer : IConsumer<User>
    {
        readonly ILogger<UserConsumer> _logger;
        public async Task Consume(ConsumeContext<User> context)
        {
            var serializedMessage = JsonSerializer.Serialize(context.Message);

            Console.WriteLine($"Message: {serializedMessage}");
        }
    }
}
