using Microsoft.Extensions.Configuration;
using Newtonsoft.Json.Linq;
using static ConsoleApp1.Common.Constants.Constants;

namespace ConsoleApp1.Handlers.Implemetations
{
    public class ZendeskMetricHandler : BaseHandler, IZendeskMetricHandler
    {
        private HttpClient zendeskMetricCLient;

        public ZendeskMetricHandler(IConfiguration configuration, IHttpClientFactory httpClientFactory)
          : base(configuration, httpClientFactory)
        {
            zendeskMetricCLient = httpClientFactory.CreateClient(ZendeskClientName);
        }


        protected override string MetricName => "Zendesk.Metric";

        public async Task SendData(int epochTimestamp) =>
            await SendData(MetricName, await FetchZendeskQueueCount(), epochTimestamp);

        private string GetToken() => CaseManagementAuthToken;

        private async Task<string> FetchZendeskQueueCount()
        {
            zendeskMetricCLient.DefaultRequestHeaders.Add("Authorization", GetToken());
            var json = await zendeskMetricCLient.GetStringAsync("");
            var queueCount = JObject.Parse(json)?["count"]?.ToString();
            Console.WriteLine($"Zendesk Engineering Ticket count: {queueCount}");
            return queueCount;
        }
    }
}
