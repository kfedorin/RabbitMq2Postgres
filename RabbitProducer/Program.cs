using System.Reflection;
using FluentValidation.AspNetCore;
using MassTransit;
using Serilog;
using Swashbuckle.AspNetCore.SwaggerUI;

Log.Logger = new LoggerConfiguration()
    .WriteTo.Console()
    .CreateLogger();

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers()
    .AddFluentValidation(options =>
    {
        // Validate child properties and root collection elements
        options.ImplicitlyValidateChildProperties = true;
        options.ImplicitlyValidateRootCollectionElements = true;
        // Automatic registration of validators in assembly
        options.RegisterValidatorsFromAssembly(Assembly.GetExecutingAssembly());
    });

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddMassTransit(x =>
{
    x.UsingRabbitMq((context, cfg) =>
    {
        cfg.ConfigureEndpoints(context);
    });
});


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

//Configure the HTTP request pipeline.

//if (app.Environment.IsDevelopment())
//{
//    app.UseSwagger();
//    app.UseSwaggerUI();
//}

//app.UseHttpsRedirection();

//app.UseAuthorization();

//app.MapControllers();

app.Run();