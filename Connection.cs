using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PokeTrainerBackEnd
{
    public class Connection
    {
        private string _connectionString = "SERVER=localhost; DATABASE=poketrainer; UID=root; PWD=root; Pooling=false;";

        public MySqlConnection con = null;

        public void OpenConnection()
        {
            try
            {
                con = new MySqlConnection(_connectionString);
                con.Open();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public void CloseConnection()
        {
            try
            {
                con.Close();
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}