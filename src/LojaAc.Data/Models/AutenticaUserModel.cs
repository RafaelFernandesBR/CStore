using Dapper;
using LojaAc.Data.IModels;
using MySql.Data.MySqlClient;
using Newtonsoft.Json;
using System.Data;

namespace LojaAc.Data.Models
{
    public class AutenticaUserModel : IAutenticaUserModel
    {
        private MySqlConnection conm { get; set; }

        protected string usuario { get; set; }
        protected string psw { get; set; }

        public AutenticaUserModel()
        {
            //get a file json
            StreamReader r = new StreamReader("conect-sql.json");
            string readFile = r.ReadToEnd();
            conectSql conectData = JsonConvert.DeserializeObject<conectSql>(readFile);

            this.conm = new MySqlConnection("Server=" + conectData.server + ";Database=" + conectData.Database + ";Uid=" + conectData.user + ";Pwd=" + conectData.senha + ";SSL Mode=None");
        }

        //validar o usuario
        public bool AutenticaUser(string user, string psw)
        {
            bool autenticado = false;
            string query = @"SELECT * FROM user WHERE usuario = @User AND psw = @Senha";
            var parametros = new DynamicParameters();
            parametros.Add("@User", user);
            parametros.Add("@Senha", psw);

            using (IDbConnection connectiontst = conm)
            {
                var dataUser = connectiontst.QueryFirstOrDefault<AutenticaUserModel>(query, parametros);

                if (dataUser != null &&
                    dataUser.psw == psw &&
                    dataUser.usuario == user)
                {
                    autenticado = true;
                }
            }

            return autenticado;
        }

        //cadastrar usuario
        public void CadastraUser(string nome, string user, string Senha)
        {
            string query = @"INSERT INTO user(nome,usuario,psw) VALUES(@Nome, @User, @Senha)";
            var parametros = new DynamicParameters();
            parametros.Add("@Nome", nome);
            parametros.Add("@User", user);
            parametros.Add("@Senha", Senha);

            using (IDbConnection connectiontst = conm)
            {
                connectiontst.QueryFirstOrDefault(query, parametros);
            }
        }

        //criptografar a senha
        public string Criptografa(string senha)
        {
            string senhaCriptografada = "";
            for (int i = 0; i < senha.Length; i++)
            {
                senhaCriptografada += (char)(senha[i] + 3);
            }
            return senhaCriptografada;
        }

    }
}
