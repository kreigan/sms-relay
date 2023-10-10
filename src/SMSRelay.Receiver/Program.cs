using Azure.Identity;

using Microsoft.Azure.Cosmos;
using Microsoft.Extensions.Azure;

using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

using SMSRelay.Core;

var host = new HostBuilder()
    .ConfigureFunctionsWorkerDefaults()
    .ConfigureLogging(builder =>
    {
        builder.SetMinimumLevel(LogLevel.Debug);
    })
    .ConfigureServices((context, services) =>
    {
        services.AddAzureClients(clientsBuilder =>
        {
            string dbConnectionString = context.Configuration[Constants.AppParamDbConnection];
            clientsBuilder.AddClient((CosmosClientOptions options) =>
                dbConnectionString.Contains("localhost:8081")
                    ? new CosmosClient(dbConnectionString, options)
                    : new CosmosClient(dbConnectionString, new DefaultAzureCredential(), options));
        });
    })
    .Build();

host.Run();
