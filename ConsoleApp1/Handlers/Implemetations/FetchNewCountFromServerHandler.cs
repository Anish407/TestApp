using Microsoft.Extensions.Configuration;
using System.Text.RegularExpressions;

namespace ConsoleApp1.Handlers.Implemetations
{
    public class FetchNewCountFromServerHandler : BaseHandler, IFetchNewCountFromServerHandler
    {
        private HttpClient campaignCLient;

        public FetchNewCountFromServerHandler(IConfiguration configuration, IHttpClientFactory httpClientFactory)
            : base(configuration, httpClientFactory)
        {
            campaignCLient = httpClientFactory.CreateClient();
        }

        protected override string MetricName { get; } = "Campaign.";

        public async Task SendData(short[] serverIds, int epochTimestamp)
        {
            foreach (var serverId in serverIds)
                await SendData(serverId, epochTimestamp);
        }

        public async Task SendData(short serverId, int epochTimestamp)
        {
            await SendData($"{MetricName}{serverId}", await FetchNewCountFromServer(serverId), epochTimestamp);
        }

        private async Task<string> FetchNewCountFromServer(short serverId)
        {
            try
            {
                HttpResponseMessage? response = await campaignCLient.GetAsync($"http://{serverId}.localhost.com/count");
                response.EnsureSuccessStatusCode();
                string htmlCode = await response.Content.ReadAsStringAsync();
                const string newCount = "new count: (.*)";
                Match? match = new Regex(newCount, RegexOptions.IgnoreCase).Match(htmlCode);
                string? campaignCount = match.Groups[1].Value;
                Console.WriteLine($"Server: {serverId}   Campaign Queue Size: {campaignCount}");
                return campaignCount;
            }
            catch (Exception)
            {
                return "failed";
            }
        }

    }
}
