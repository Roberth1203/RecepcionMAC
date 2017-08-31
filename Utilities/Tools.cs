using System;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;

namespace Utilities
{
    public class Tools
    {
        public string catchError = String.Empty;

        public static SqlConnection SQLConnectionStart(string conexion)
        {
            try
            {
                string connectionStr = conexion;
                SqlConnection conn = new SqlConnection(connectionStr);
                conn.Open();
                return conn;
            }
            catch (Exception)
            {
                return null;
            }
        }

        public void closeConnection(SqlConnection conn)
        {
            try
            {
                conn.Close();
                SqlConnection.ClearPool(conn);
            }
            catch (Exception ex)
            {
                catchError = ex.Message;
            }
        }

        public DataTable getRecords(string query, SqlConnection connection, string getConn)
        {
            DataTable dt = new DataTable();
            try
            {
                if (connection == null)
                {
                    connection = SQLConnectionStart(getConn);
                    SqlDataAdapter adapter = new SqlDataAdapter();
                    adapter.SelectCommand = new SqlCommand(query, connection); // Ejecución de la consulta SELECT
                    adapter.Fill(dt); // Los registros obtenidos se cargan al DataTable a retornar
                    closeConnection(connection);
                }
            }
            catch (Exception w)
            {
                catchError = w.Message;
            }
            return dt;
        }

        public void SQLstatement(string sentence, string getConnString, SqlConnection conn = null)
        {
            try
            {
                if (conn == null)
                {
                    conn = SQLConnectionStart(getConnString);
                    SqlCommand query = new SqlCommand(sentence, conn);
                    query.ExecuteNonQuery();

                    closeConnection(conn);
                }
            }
            catch (Exception q)
            {
                catchError = q.Message;
            }
        }
    }
}
