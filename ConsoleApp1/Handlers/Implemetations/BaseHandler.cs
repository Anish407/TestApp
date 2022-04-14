using Microsoft.Extensions.Configuration;
using System.Text;
using static ConsoleApp1.Common.Constants.Constants;

namespace ConsoleApp1.Handlers.Implemetations
{
    public abstract class BaseHandler
    {
        private HttpClient client;

        protected abstract string MetricName { get; }

        public BaseHandler(IConfiguration configuration, IHttpClientFactory httpClientFactory)
        {
            client = httpClientFactory.CreateClient(MetricClientName);
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        protected async Task SendData(string metric, string value, int epochTimestamp)
        {
            string json = $"{{'series':[{{'metric':'{ metric } ','points':[[{ epochTimestamp },{value}]],'type':'count'}}]}}";

            using HttpResponseMessage? response = await client.PostAsync($"?api_key={VisualiserApiKey}", new StringContent(json, Encoding.UTF8));
            response.EnsureSuccessStatusCode();
            Stream stream = await response.Content.ReadAsStreamAsync() ;
            using StreamReader? reader = new StreamReader(stream ?? throw new InvalidOperationException());
            await reader?.ReadToEndAsync();
        }
    }
}
