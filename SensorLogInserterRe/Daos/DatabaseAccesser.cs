using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SensorLogInserterRe.Daos
{
    public class DatabaseAccesser
    {
        private static readonly string ConnectionString = "Data Source=" + "ECOLOGDB" + ";Initial Catalog=ECOLOGDBver2;Integrated Security=Yes;Connect Timeout=15;";

        public static DataTable GetResult(string query)
        {
            var dataTable = new DataTable();

            using (SqlConnection sqlConnection = new SqlConnection(DatabaseAccesser.ConnectionString))
            {
                SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(query, DatabaseAccesser.ConnectionString);

                try
                {
                    sqlConnection.Open();
                    SqlCommand command = new SqlCommand(query, sqlConnection);
                    command.CommandTimeout = 600;
                    sqlDataAdapter.SelectCommand = command;
                    sqlDataAdapter.Fill(dataTable);
                }
                catch (SqlException sqlException)
                {
                    
                }
                finally
                {
                    sqlConnection.Close();
                }
            }

            return dataTable;
        }

        public static void Insert(String tableName, DataTable dataTable)
        {
            using (SqlBulkCopy bulkCopy = new SqlBulkCopy(DatabaseAccesser.ConnectionString))
            {
                try
                {
                    bulkCopy.BulkCopyTimeout = 600;
                    bulkCopy.DestinationTableName = tableName;
                    bulkCopy.WriteToServer(dataTable);
                }
                catch (SqlException sqlException)
                {

                }
                finally
                {
                    bulkCopy.Close();
                }
            }
        }

        public static void Insert(string query)
        {
            using (SqlConnection sqlConnection = new SqlConnection(DatabaseAccesser.ConnectionString))
            {
                SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(query, DatabaseAccesser.ConnectionString);

                try
                {
                    sqlConnection.Open();
                    SqlCommand command = new SqlCommand(query, sqlConnection);
                    command.CommandTimeout = 600;
                    command.ExecuteNonQuery();
                }
                catch (SqlException sqlException)
                {

                }
                finally
                {
                    sqlConnection.Close();
                }
            }
        }
    }
}
