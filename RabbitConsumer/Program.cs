using FluentValidation;
using MassTransit;
using RabbitConsumer.Services;
using MediatR;
using Microsoft.EntityFrameworkCore;
using RabbitConsumer.Commands.UserCommand;
using RabbitConsumer.Interface;
using RabbitConsumer.Repositories;
using Serilog;
using Swashbuckle.AspNetCore.SwaggerUI;

Log.Logger = new LoggerConfiguration()
    .WriteTo.Console()
    .CreateLogger();

IConfiguration configuration = new ConfigurationBuilder()
    .AddJsonFile("appsettings.json")
    .Build();

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

builder.Services.AddScoped<IValidator<CreateUserCommand>, CreateUserCommandHandler.CreateUserCommandValidator>();
builder.Services.AddScoped<IValidator<UpdateUserCommand>, UpdateUserCommandHandler.UpdateUserCommandValidator>();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

builder.Services.AddDbContext<IDbContext, RabbitTestContext>(c =>
{
    var connectionString = configuration["DbConnectionString:ConnectionStringNpgsql"];
    c.UseNpgsql(connectionString);
});

builder.Services.AddMassTransit(x =>
{
    x.AddConsumer<UserConsumer>();

    x.SetKebabCaseEndpointNameFormatter();

    x.UsingRabbitMq((context, cfg) =>
    {
        cfg.ConfigureEndpoints(context);
    });
});

builder.Services.AddMediatR(typeof(Program));

var app = builder.Build();

app.UseSwagger()
    .UseSwaggerUI(c =>
    {
        c.ConfigObject = new ConfigObject
        {
            ShowCommonExtensions = true
        };
    })
    .UseStaticFiles()
    .UseRouting()
    .UseEndpoints(endpoints =>
    {
        endpoints.MapControllers();
    });

app.Run();

