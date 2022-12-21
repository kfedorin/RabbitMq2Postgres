using System.Text.Json;
using AutoMapper;
using MassTransit;
using MediatR;
using RabbitConsumer.Commands.UserCommand;
using RabbitConsumer.Handlers.UserQuery;
using RabbitModel;
using Serilog;


namespace RabbitConsumer.Services
{
    public class UserConsumer : IConsumer<User>
    {

        private readonly IMediator _mediator;
        public UserConsumer(IMediator mediator)
        {
            _mediator = mediator;
        }

        public async Task Consume(ConsumeContext<User> context)
        {
            var serializedMessage = JsonSerializer.Serialize(context.Message);

            Log.Information($"Get message: {serializedMessage}");

            var mapper = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<User, CreateUserCommand>();
            }).CreateMapper();

            var entity = mapper.Map<CreateUserCommand>(context.Message);

            await _mediator.Send(entity);
        }
    }
}
