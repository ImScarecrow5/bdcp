using System;
using System.Data.SqlClient;
using System.IO;
using System.Text.RegularExpressions;

namespace MusicVideoStoreCourseProject.Services
{
    public static class DatabaseInitializer
    {
        public static void EnsureDatabase()
        {
            SqlServerCon.EnsureInitialized();
            CreateDatabaseIfMissing();

            if (DatabaseTablesExist())
                return;

            ExecuteCreateScript();
        }

        private static void CreateDatabaseIfMissing()
        {
            using (SqlConnection connection = new SqlConnection(SqlServerCon.MasterConnectionString))
            {
                connection.Open();
                using (SqlCommand command = new SqlCommand(
                    "IF DB_ID(N'MusicVideoStore') IS NULL CREATE DATABASE MusicVideoStore", connection))
                {
                    command.ExecuteNonQuery();
                }
            }
        }

        private static bool DatabaseTablesExist()
        {
            string sql = @"
SELECT COUNT(*)
FROM sys.tables
WHERE name IN
(
    N'Media_type',
    N'Type_of_genre',
    N'Type_of_subgenre',
    N'Shops',
    N'Supplier',
    N'Product_information'
)";

            using (SqlConnection connection = new SqlConnection(SqlServerCon.DefaultConnectionString))
            using (SqlCommand command = new SqlCommand(sql, connection))
            {
                connection.Open();
                int count = Convert.ToInt32(command.ExecuteScalar());
                return count == 6;
            }
        }

        private static void ExecuteCreateScript()
        {
            string scriptPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Scripts", "CreateDatabase.sql");
            if (!File.Exists(scriptPath))
            {
                scriptPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "..", "..", "Scripts", "CreateDatabase.sql");
            }

            if (!File.Exists(scriptPath))
                throw new FileNotFoundException("Не найден SQL-скрипт создания базы данных", scriptPath);

            string sql = File.ReadAllText(scriptPath);
            string[] batches = Regex.Split(sql, @"^\s*GO\s*$", RegexOptions.Multiline | RegexOptions.IgnoreCase);

            using (SqlConnection connection = new SqlConnection(SqlServerCon.DefaultConnectionString))
            {
                connection.Open();
                foreach (string batch in batches)
                {
                    if (string.IsNullOrWhiteSpace(batch))
                        continue;

                    using (SqlCommand command = new SqlCommand(batch, connection))
                    {
                        command.CommandTimeout = 60;
                        command.ExecuteNonQuery();
                    }
                }
            }
        }
    }
}
