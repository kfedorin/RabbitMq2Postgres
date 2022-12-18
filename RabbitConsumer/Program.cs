using System.Reflection;
using MassTransit;
using Microsoft.EntityFrameworkCore;
using RabbitConsumer;
using RabbitConsumer.Interface;
using RabbitConsumer.Repositories;
using RabbitConsumer.Repositories.Models;
using RabbitConsumer.Repositories.Technology;

IConfiguration configuration = new ConfigurationBuilder()
    .AddJsonFile("appsettings.json")
    .Build();

//var builder = WebApplication.CreateBuilder(args);

//builder.Services.AddDbContext<RabbitTestContext>(c =>
//{
//    var connectionString = configuration["DbConnectionString:ConnectionStringNpgsql"];
//    c.UseNpgsql(connectionString);
//});

//builder.Services.AddScoped<ITechnology<User>, UserRepository>();
//builder.Services.AddScoped<ITechnology<Organization>, OrganizationRepository>();


//builder.Services.AddMassTransit(busConfigurator =>
//{
//    var entryAssembly = Assembly.GetExecutingAssembly();

//    busConfigurator.AddConsumers(entryAssembly);
//    busConfigurator.UsingRabbitMq((context, busFactoryConfigurator) =>
//    {
//        busFactoryConfigurator.Host("rabbitmq", "/", h => { });

//        busFactoryConfigurator.ConfigureEndpoints(context);
//    });
//});

var builder = Host.CreateDefaultBuilder(args);

builder.ConfigureServices((hostContext, services) =>
{
    services.AddMassTransit(busConfigurator =>
    {
        //var entryAssembly = Assembly.GetExecutingAssembly();
    
        busConfigurator.AddConsumer<NotificationCreatedConsumer>();

        busConfigurator.UsingRabbitMq((ctx, cfg) =>
        {
            cfg.Host("rabbitmq");

            cfg.ReceiveEndpoint("created-event", e =>
            {
                e.Consumer<NotificationCreatedConsumer>();
            });
            //busFactoryConfigurator.Host("rabbitmq", "/", h => { });

            //busFactoryConfigurator.ConfigureEndpoints(context);
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

//// Configure the HTTP request pipeline.
//if (app.Environment.IsDevelopment())
//{
//    app.UseSwagger();
//    app.UseSwaggerUI();
//}

//app.UseHttpsRedirection();

//app.UseAuthorization();

//app.MapControllers();

app.Run();
