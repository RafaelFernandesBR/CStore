using MySql.Data.MySqlClient;
using Newtonsoft.Json;
using Dapper;
using System.Data;
using LojaAc.Data.IModels;

namespace LojaAc.Data.Models
{
    public class DataModel : IDataModel
    {
        private MySqlConnection conm { get; set; }

        public DataModel()
        {
            //get a file json
            StreamReader r = new StreamReader("conect-sql.json");
            string readFile = r.ReadToEnd();
            conectSql conectData = JsonConvert.DeserializeObject<conectSql>(readFile);

            this.conm = new MySqlConnection("Server=" + conectData.server + ";Database=" + conectData.Database + ";Uid=" + conectData.user + ";Pwd=" + conectData.senha + ";");
        }

        public IEnumerable<AppsData> GetAll()
        {
            using (IDbConnection connectiontst = conm)
            {
                var cost = connectiontst.Query<AppsData>("SELECT * FROM aplicativos");
                return cost;
            }
        }

        public IEnumerable<AppsData> GetAleatorio()
        {
            using (IDbConnection connectiontst = conm)
            {
                var cost = connectiontst.Query<AppsData>("SELECT * FROM aplicativos ORDER BY rand() LIMIT 5");
                return cost;
            }
        }

        //return app for id.
        public AppsData GetId(int id)
        {
            string query = @"SELECT * FROM aplicativos WHERE id = @id";
            var parametros = new DynamicParameters();
            parametros.Add("@id", id.ToString());

            using (IDbConnection connectiontst = conm)
            {
                var cost = connectiontst.QueryFirstOrDefault<AppsData>(query, parametros);
                return cost;
            }

        }

        //delet fron id
        public void Delete(int id)
        {
            string query = @"DELETE FROM aplicativos WHERE id = @Id";
            var parametros = new DynamicParameters();
            parametros.Add("@Id", id);

            using (IDbConnection connectiontst = conm)
            {
                connectiontst.QueryFirstOrDefault(query, parametros);
            }

        }

        //insert a data fron mysql.
        public void Insert(string nome, string descricao, string url)
        {
            string query = @"INSERT INTO aplicativos(nome,descricao,url) VALUES(@Nome, @Descricao, @URL )";
            var parametros = new DynamicParameters();
            parametros.Add("@Nome", nome);
            parametros.Add("@Descricao", descricao);
            parametros.Add("@URL", url);

            using (IDbConnection connectiontst = conm)
            {
                connectiontst.QueryFirstOrDefault(query, parametros);
            }

        }

        //insert a data fron mysql log.
        public void InsertLog(string user, string msglog)
        {
            string query = @"INSERT INTO aplicativos_logs(user,textolog) VALUES(@User, @TextoLog)";
            var parametros = new DynamicParameters();
            parametros.Add("@User", user);
            parametros.Add("@TextoLog", msglog);

            using (IDbConnection connectiontst = conm)
            {
                connectiontst.QueryFirstOrDefault(query, parametros);
            }

        }

        //update a data fron mysql.
        public void UpdateClickes(int clicke, int id)
        {
            string query = @"UPDATE aplicativos SET clickes = @Clicke WHERE id = @Id";
            var parametros = new DynamicParameters();
            parametros.Add("@Clicke", clicke);
            parametros.Add("@Id", id);

            using (IDbConnection connectiontst = conm)
            {
                connectiontst.QueryFirstOrDefault(query, parametros);
            }
        }

    }
}
