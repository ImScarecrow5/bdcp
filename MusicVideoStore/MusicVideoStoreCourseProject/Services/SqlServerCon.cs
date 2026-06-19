using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;

namespace MusicVideoStoreCourseProject.Services
{
    public static class SqlServerCon
    {
        public const string DatabaseName = "MusicVideoStore";

        private static bool initialized;
        private static string masterConnectionString;
        private static string defaultConnectionString;

        public static string MasterConnectionString
        {
            get
            {
                EnsureInitialized();
                return masterConnectionString;
            }
        }

        public static string DefaultConnectionString
        {
            get
            {
                EnsureInitialized();
                return defaultConnectionString;
            }
        }

        public static SqlConnection CreateDefaultConnection()
        {
            return new SqlConnection(DefaultConnectionString);
        }

        public static void EnsureInitialized()
        {
            if (initialized)
                return;

            List<string> candidates = GetConnectionCandidates();
            Exception lastException = null;

            foreach (string candidate in candidates)
            {
                try
                {
                    using (SqlConnection connection = new SqlConnection(candidate))
                    {
                        connection.Open();
                    }

                    masterConnectionString = candidate;
                    defaultConnectionString = BuildDatabaseConnection(candidate, DatabaseName);
                    initialized = true;
                    return;
                }
                catch (Exception ex)
                {
                    lastException = ex;
                }
            }

            throw new Exception(
                "Не удалось подключиться к SQL Server. Проверьте, что служба SQL Server запущена, " +
                "а в App.config указан правильный Data Source. Сейчас основной вариант настроен на localhost. " +
                "Также проверялись: ., .\\SQLEXPRESS, localhost\\SQLEXPRESS, (localdb)\\MSSQLLocalDB.",
                lastException);
        }

        private static List<string> GetConnectionCandidates()
        {
            List<string> result = new List<string>();

            AddConfigConnection(result, "MasterConnection");

            // Сначала проверяется тот сервер, который обычно открыт в SSMS как localhost.
            AddServerCandidate(result, @"localhost");
            AddServerCandidate(result, @".");
            AddServerCandidate(result, @".\SQLEXPRESS");
            AddServerCandidate(result, @"localhost\SQLEXPRESS");
            AddServerCandidate(result, @"(localdb)\MSSQLLocalDB");

            return result;
        }

        private static void AddConfigConnection(List<string> result, string name)
        {
            ConnectionStringSettings settings = ConfigurationManager.ConnectionStrings[name];
            if (settings == null || string.IsNullOrWhiteSpace(settings.ConnectionString))
                return;

            AddUnique(result, settings.ConnectionString);
        }

        private static void AddServerCandidate(List<string> result, string dataSource)
        {
            SqlConnectionStringBuilder builder = new SqlConnectionStringBuilder();
            builder.DataSource = dataSource;
            builder.InitialCatalog = "master";
            builder.IntegratedSecurity = true;
            builder.TrustServerCertificate = true;
            builder.ConnectTimeout = 3;
            AddUnique(result, builder.ConnectionString);
        }

        private static void AddUnique(List<string> result, string connectionString)
        {
            foreach (string item in result)
            {
                if (string.Equals(item, connectionString, StringComparison.OrdinalIgnoreCase))
                    return;
            }

            result.Add(connectionString);
        }

        private static string BuildDatabaseConnection(string baseConnectionString, string databaseName)
        {
            SqlConnectionStringBuilder builder = new SqlConnectionStringBuilder(baseConnectionString);
            builder.InitialCatalog = databaseName;
            builder.TrustServerCertificate = true;
            return builder.ConnectionString;
        }
    }
}
