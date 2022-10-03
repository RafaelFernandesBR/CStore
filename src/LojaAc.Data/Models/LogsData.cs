using Dapper;
using LojaAc.Data.IModels;
using MySql.Data.MySqlClient;
using Newtonsoft.Json;
using System.Data;

namespace LojaAc.Data.Models
{
    public class LogsData : ILogsData
    {
        private MySqlConnection conm { get; set; }

        public string user { get; set; }
        public string TextoLog { get; set; }

        public LogsData()
        {
            //get a file json
            StreamReader r = new StreamReader("conect-sql.json");
            string readFile = r.ReadToEnd();
            conectSql conectData = JsonConvert.DeserializeObject<conectSql>(readFile);

            this.conm = new MySqlConnection("Server=" + conectData.server + ";Database=" + conectData.Database + ";Uid=" + conectData.user + ";Pwd=" + conectData.senha + ";SSL Mode=None");
        }

        public IEnumerable<LogsData> GetLogs()
        {
            using (IDbConnection connectiontst = conm)
            {
                var cost = connectiontst.Query<LogsData>("SELECT * FROM aplicativos_logs");
                return cost;
            }

        }

        public void LimpaLogs()
        {
            using (IDbConnection connectiontst = conm)
            {
                connectiontst.QueryFirstOrDefault("TRUNCATE TABLE aplicativos_logs");
            }

        }

    }
}
