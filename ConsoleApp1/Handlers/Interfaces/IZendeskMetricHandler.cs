
namespace ConsoleApp1.Handlers.Implemetations
{
    public interface IZendeskMetricHandler
    {
        Task SendData(int epochTimestamp);
    }
}