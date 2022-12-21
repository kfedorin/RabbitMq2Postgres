using AutoMapper.Extensions.ExpressionMapping;
using MassTransit;
using RabbitConsumer.Services;
using MediatR;
using Microsoft.EntityFrameworkCore;
using RabbitConsumer.Controllers;
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
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddAutoMapper(cfg =>
    cfg.AddExpressionMapping());

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

