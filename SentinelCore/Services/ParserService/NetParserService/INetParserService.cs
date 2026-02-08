using SentinelCore.DAL.Data.Models;

namespace SentinelCore.Services.ParserService
{
    public interface INetParserService
    {
        bool CanParse(string logText);
    }
}
