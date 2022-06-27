

using MassTransit;
using MessageBus.Handler.Contracts;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
ConfigureServices(builder.Services);
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

 void ConfigureServices(IServiceCollection services)
{
    var connectionString =
"Endpoint=sb://servicebustestnetcore.servicebus.windows.net/;SharedAccessKeyName=RootManageSharedAccessKey;SharedAccessKey=blablasharedaccesskey";

    var newPurchaseTopic = "new-purchase-topic";

    // create the bus using Azure Service bus
    var azureServiceBus =  Bus.Factory.CreateUsingAzureServiceBus(busFactoryConfig =>
    {
        busFactoryConfig.Host(connectionString);

        // specify the message Purchase to be sent to a specific topic
        busFactoryConfig.Message<IQuotesEvent>(configTopology =>
        {
            configTopology.SetEntityName(newPurchaseTopic);
        });

    });

    services.AddMassTransit
        (
            config =>
            {
                config.AddBus(provider => azureServiceBus);
            }
        );

    services.AddSingleton<IPublishEndpoint>(azureServiceBus);
    services.AddSingleton<IBus>(azureServiceBus);

    services.AddControllers();
}