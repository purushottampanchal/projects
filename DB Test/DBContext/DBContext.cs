using MySql.Data.MySqlClient;

namespace DB_Test.DBContext
{
    public class DBContext
    {
        private string _connString { get; set; }

        public DBContext(string connString)
        {
            _connString = connString;
        }

        public MySqlConnection GetConnection()
        {
            return new MySqlConnection(_connString);
        }
    }
}
