using LojaAc.Data.Models;

namespace LojaAc.Data.IModels
{
    public interface ILogsData
    {
        IEnumerable<LogsData> GetLogs();
        void LimpaLogs();

    }
}
