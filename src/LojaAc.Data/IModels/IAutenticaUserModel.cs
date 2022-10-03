using LojaAc.Data.Models;  

namespace LojaAc.Data.IModels
{
    public interface IAutenticaUserModel
    {
        bool AutenticaUser(string user, string psw);
        void CadastraUser(string nome, string user, string Senha);
        string Criptografa(string senha);
    }
}
