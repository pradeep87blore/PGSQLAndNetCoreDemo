using Npgsql;
using System;
using System.Data;
using System.Net.NetworkInformation;

namespace PGSqlAccess
{
    public class PGSqlAccessHelper
    {
        static PGSqlAccessHelper instance;
        
        PGSqlAccessHelper()
        {
            
        }

        public static PGSqlAccessHelper GetInstance()
        {
            if (instance == null)
            {
                instance = new PGSqlAccessHelper();
            }

            return instance;
        }

        // We can connect each time we want to perform a query.
        // No need to open a connection and keep it alive since
        // the server 
        public bool ConnectToDB(out NpgsqlConnection connection)
        {
            string serverIp = "", userId = "", password = "", database = "";

            if (!GetServerDetails(out serverIp, out userId, out password, out database))
            {
                // Error has occured reading the config. Return false
                connection = null;
                return false;
            }
            //string connString = "Host=localhost;Username=postgres;Password=pradeep;Database=Patient";

            // No need to pass the server port
            string connString = String.Format("Server={0};User Id={1};Password={2};Database={3};",
                serverIp, userId, password, database); 

            connection = new NpgsqlConnection(connString);

            connection.Open();
            if (connection.State == ConnectionState.Open)
                return true;

            return false;
        }

        private bool GetServerDetails(out string serverIp, out string userId, 
            out string password, out string database)
        {
            // TODO: Move this to a json file and read the config from it
            serverIp = "localhost";
            userId = "postgres";
            password = "pradeep";
            database = "Patient";

            return true;
        }
    }
}
