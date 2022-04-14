using ConsoleApp1.Handlers.Implemetations;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using static ConsoleApp1.Common.Constants.Constants;


IServiceProvider serviceProvider = SetupDi();
IFetchNewCountFromServerHandler fetchNewCountFromServerHandler = serviceProvider.GetRequiredService<IFetchNewCountFromServerHandler>();
IZendeskMetricHandler zendeskMetricHandler = serviceProvider.GetRequiredService<IZendeskMetricHandler>();

int epochTimestamp = (int)(DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1))).TotalSeconds;
short[] serverIds = { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11 };

await fetchNewCountFromServerHandler.SendData(serverIds, epochTimestamp);
await zendeskMetricHandler.SendData(epochTimestamp);


 IServiceProvider SetupDi()
{
    ServiceCollection services = new ServiceCollection();

    services.AddScoped<IFetchNewCountFromServerHandler, FetchNewCountFromServerHandler>();
    services.AddScoped<IZendeskMetricHandler, ZendeskMetricHandler>();

    services.AddHttpClient(MetricClientName, client =>
    {
        client.BaseAddress = new Uri(VisualiserSeriesUri);
        client.DefaultRequestHeaders.Add("Accept", "application/json");
    });

    services.AddHttpClient(ZendeskClientName, client =>
    {
        client.BaseAddress = new Uri(CaseManagementQueueCountUrl);
        client.DefaultRequestHeaders.Add("Accept", "application/json");
    });

    IConfiguration SetupConfiguration()
             => new ConfigurationBuilder()
            .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
            .AddUserSecrets<Program>()
            .Build();

    services.AddSingleton(SetupConfiguration());

    return services.BuildServiceProvider();
}


