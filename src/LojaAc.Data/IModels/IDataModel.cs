using LojaAc.Data.Models;

namespace LojaAc.Data.IModels
{
    public interface IDataModel
    {
        IEnumerable<AppsData> GetAll();
        IEnumerable<AppsData> GetAleatorio();
        AppsData GetId(int id);
        void Delete(int id);
        void Insert(string nome, string descricao, string url);
        void InsertLog(string user, string msglog);
        void UpdateClickes(int clicke, int id);
    }
}
