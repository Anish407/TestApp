
namespace ConsoleApp1.Handlers.Implemetations
{
    public interface IFetchNewCountFromServerHandler
    {
        Task SendData(short serverId, int epochTimestamp);
        Task SendData(short[] serverIds, int epochTimestamp);
    }
}