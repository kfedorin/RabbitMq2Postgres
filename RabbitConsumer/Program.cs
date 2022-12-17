using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

builder.ConfigureServices((hostContext, services) =>
{
    services.AddMassTransit(busConfigurator =>
    {
        var entryAssembly = Assembly.GetExecutingAssembly();

        busConfigurator.AddConsumers(entryAssembly);
        busConfigurator.UsingRabbitMq((context, busFactoryConfigurator) =>
        {
            busFactoryConfigurator.Host("rabbitmq", "/", h => { });

            busFactoryConfigurator.ConfigureEndpoints(context);
        });
    });
});

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
