using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SensorLogInserterRe.Utils;

namespace SensorLogInserterRe.Daos
{
    public class DatabaseAccesser
    {
        private static readonly string ConnectionString = "Data Source=" + "ECOLOGDB2016" + ";Initial Catalog=ECOLOGDBver3;Integrated Security=Yes;Connect Timeout=15;";
        private static readonly int NumberOfViolationOfPrimaryKey = 2627;

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
                    // Console.WriteLine($"ERROR: {sqlException.Message}, {sqlException.StackTrace}");
                    LogWritter.WriteLog(LogWritter.LogMode.Error, $"ERROR: {sqlException.Message}, {sqlException.StackTrace}");
                    
                }
                finally
                {
                    sqlConnection.Close();
                }
            }

            return dataTable;
        }

        public static void Delete(string query)
        {
            using (SqlConnection sqlConnection = new SqlConnection(DatabaseAccesser.ConnectionString))
            {

                try
                {
                    sqlConnection.Open();
                    SqlCommand command = new SqlCommand(query, sqlConnection);
                    command.CommandTimeout = 600;
                    command.ExecuteNonQuery();
                }
                catch (SqlException sqlException)
                {
                    // Console.WriteLine($"ERROR: {sqlException.Message}, {sqlException.StackTrace}");
                    LogWritter.WriteLog(LogWritter.LogMode.Error, $"ERROR: {sqlException.Message}, {sqlException.StackTrace}");

                }
                finally
                {
                    sqlConnection.Close();
                }
            }
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
                    if (sqlException.Number != NumberOfViolationOfPrimaryKey)
                    {
                        Console.WriteLine($"ERROR: {sqlException.Message}, {sqlException.StackTrace}");
                        LogWritter.WriteLog(LogWritter.LogMode.Error, $"ERROR: {sqlException.Message}, {sqlException.StackTrace}");
                    }
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
                try
                {
                    sqlConnection.Open();
                    SqlCommand command = new SqlCommand(query, sqlConnection);
                    command.CommandTimeout = 600;
                    command.ExecuteNonQuery();
                }
                catch (SqlException sqlException)
                {
                    if (sqlException.Number != NumberOfViolationOfPrimaryKey)
                    {
                        Console.WriteLine($"ERROR: {sqlException.Message}, {sqlException.StackTrace}");
                        LogWritter.WriteLog(LogWritter.LogMode.Error, $"ERROR: {sqlException.Message}, {sqlException.StackTrace}");
                    }
                }
                finally
                {
                    sqlConnection.Close();
                }
            }
        }

        public static void Update(string query)
        {
            using (SqlConnection sqlConnection = new SqlConnection(DatabaseAccesser.ConnectionString))
            {
                try
                {
                    sqlConnection.Open();
                    SqlCommand command = new SqlCommand(query, sqlConnection);
                    command.CommandTimeout = 600;
                    command.ExecuteNonQuery();
                }
                catch (SqlException sqlException)
                {
                    Console.WriteLine($"ERROR: {sqlException.Message}, {sqlException.StackTrace}");
                    LogWritter.WriteLog(LogWritter.LogMode.Error, $"ERROR: {sqlException.Message}, {sqlException.StackTrace}");
                }
                finally
                {
                    sqlConnection.Close();
                }
            }
        }
    }
}
