using AutoMapper.Extensions.ExpressionMapping;
using MassTransit;
using RabbitConsumer.Services;
using MediatR;
using Microsoft.EntityFrameworkCore;
using RabbitConsumer.Controllers;
using RabbitConsumer.Interface;
using RabbitConsumer.Repositories;

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

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

//app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();

