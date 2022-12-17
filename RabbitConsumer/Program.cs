using System.Reflection;
using MassTransit;
using RabbitConsumer;
using RabbitConsumer.Interface;

var builder = Host.CreateDefaultBuilder(args);

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

//var busControl = Bus.Factory.CreateUsingRabbitMq(cfg =>
//{
//    cfg.ReceiveEndpoint("created-event", e =>
//    {
//        e.Consumer<NotificationCreatedConsumer>();
//    });
//});

//await busControl.StartAsync(new CancellationToken());
//try
//{
//    Console.WriteLine("Press enter to exit");
//    await Task.Run(() => Console.ReadLine());
//}
//finally
//{
//    await busControl.StopAsync();
//}



//builder.Services.AddControllers();
//builder.Services.AddEndpointsApiExplorer();
//builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
//if (app.Environment.IsDevelopment())
//{
//    app.UseSwagger();
//    app.UseSwaggerUI();
//}

//app.UseHttpsRedirection();

//app.UseAuthorization();

//app.MapControllers();

app.Run();
