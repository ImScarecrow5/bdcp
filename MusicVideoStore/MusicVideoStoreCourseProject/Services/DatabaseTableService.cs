using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.Linq;
using System.Windows.Forms;
using MusicVideoStoreCourseProject.Models;

namespace MusicVideoStoreCourseProject.Services
{
    public class DatabaseTableService
    {
        private readonly string _connectionString = SqlServerCon.DefaultConnectionString;

        public List<TableInfo> GetTables()
        {
            List<TableInfo> result = new List<TableInfo>();
            string sql = @"
SELECT SCHEMA_NAME(t.schema_id) AS SchemaName, t.name AS TableName
FROM sys.tables t
WHERE t.is_ms_shipped = 0
ORDER BY t.name";

            using (SqlConnection connection = new SqlConnection(_connectionString))
            using (SqlCommand command = new SqlCommand(sql, connection))
            {
                connection.Open();
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        result.Add(new TableInfo
                        {
                            SchemaName = reader["SchemaName"].ToString(),
                            TableName = reader["TableName"].ToString()
                        });
                    }
                }
            }
            return result;
        }

        public List<ColumnInfo> GetColumns(TableInfo table)
        {
            List<ColumnInfo> result = new List<ColumnInfo>();
            string sql = @"
SELECT c.name AS ColumnName,
       TYPE_NAME(c.user_type_id) AS DataType,
       c.is_nullable AS IsNullable,
       c.is_identity AS IsIdentity,
       CASE WHEN i.is_primary_key = 1 THEN 1 ELSE 0 END AS IsPrimaryKey
FROM sys.columns c
LEFT JOIN sys.index_columns ic ON ic.object_id = c.object_id AND ic.column_id = c.column_id
LEFT JOIN sys.indexes i ON i.object_id = ic.object_id AND i.index_id = ic.index_id AND i.is_primary_key = 1
WHERE c.object_id = OBJECT_ID(@FullName)
ORDER BY c.column_id";

            using (SqlConnection connection = new SqlConnection(_connectionString))
            using (SqlCommand command = new SqlCommand(sql, connection))
            {
                command.Parameters.AddWithValue("@FullName", table.SchemaName + "." + table.TableName);
                connection.Open();
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        result.Add(new ColumnInfo
                        {
                            ColumnName = reader["ColumnName"].ToString(),
                            DataType = reader["DataType"].ToString().ToLowerInvariant(),
                            IsNullable = Convert.ToBoolean(reader["IsNullable"]),
                            IsIdentity = Convert.ToBoolean(reader["IsIdentity"]),
                            IsPrimaryKey = Convert.ToBoolean(reader["IsPrimaryKey"])
                        });
                    }
                }
            }
            return result;
        }

        internal ColumnInfo GetKeyColumn(TablePageData pageData)
        {
            ColumnInfo key = pageData.Columns.FirstOrDefault(c => c.IsPrimaryKey);
            if (key == null)
                throw new Exception("В таблице не найден первичный ключ.");
            return key;
        }

        internal void DisplayTableData(TableInfo table, DataGridView dataGrid)
        {
            DataTable dataTable = ExecuteTable("SELECT TOP 1000 * FROM " + table.FullName + " ORDER BY 1 DESC");
            dataGrid.DataSource = dataTable;
            dataGrid.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
        }

        private DataTable ExecuteTable(string sql)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            using (SqlDataAdapter adapter = new SqlDataAdapter(sql, connection))
            {
                DataTable table = new DataTable();
                adapter.Fill(table);
                return table;
            }
        }

        private void ExecuteNonQuery(string sql, IEnumerable<SqlParameter> parameters)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            using (SqlCommand command = new SqlCommand(sql, connection))
            {
                if (parameters != null)
                {
                    foreach (SqlParameter parameter in parameters)
                        command.Parameters.Add(parameter);
                }
                connection.Open();
                try
                {
                    command.ExecuteNonQuery();
                }
                catch (SqlException ex)
                {
                    throw CreateUserFriendlySqlException(ex);
                }
            }
        }

        private Exception CreateUserFriendlySqlException(SqlException ex)
        {
            if (ex.Number == 547)
            {
                return new Exception(
                    "Запись нельзя удалить или изменить, потому что она используется в другой таблице. " +
                    "Сначала удалите связанные записи или измените у них ссылку на другую запись.");
            }

            if (ex.Number == 2627 || ex.Number == 2601)
                return new Exception("Такая запись уже существует. Проверьте уникальные поля.");

            return new Exception(ex.Message);
        }

        public void AddRecord(TableInfo table, List<ColumnInfo> columns, Dictionary<string, TextBox> fields, DataGridView dataGrid)
        {
            List<ColumnInfo> editable = columns.Where(c => !c.IsIdentity && !c.IsPrimaryKey).ToList();
            List<string> columnNames = editable.Select(c => c.SafeName).ToList();
            List<string> parameterNames = editable.Select(c => "@" + c.ColumnName).ToList();
            List<SqlParameter> parameters = editable.Select(c => new SqlParameter("@" + c.ColumnName, GetValue(fields[c.ColumnName].Text, c))).ToList();

            string sql = "INSERT INTO " + table.FullName + " (" + string.Join(", ", columnNames) + ") VALUES (" + string.Join(", ", parameterNames) + ")";
            ExecuteNonQuery(sql, parameters);
            DisplayTableData(table, dataGrid);
        }

        public void ChangeRecord(TableInfo table, List<ColumnInfo> columns, Dictionary<string, TextBox> fields, string selectedId, DataGridView dataGrid)
        {
            if (string.IsNullOrWhiteSpace(selectedId))
                throw new Exception("Не выбрана запись для изменения.");

            ColumnInfo keyColumn = columns.First(c => c.IsPrimaryKey);
            List<ColumnInfo> editable = columns.Where(c => !c.IsIdentity && !c.IsPrimaryKey).ToList();
            List<string> sets = editable.Select(c => c.SafeName + " = @" + c.ColumnName).ToList();
            List<SqlParameter> parameters = editable.Select(c => new SqlParameter("@" + c.ColumnName, GetValue(fields[c.ColumnName].Text, c))).ToList();
            parameters.Add(new SqlParameter("@Id", selectedId));

            string sql = "UPDATE " + table.FullName + " SET " + string.Join(", ", sets) + " WHERE " + keyColumn.SafeName + " = @Id";
            ExecuteNonQuery(sql, parameters);
            DisplayTableData(table, dataGrid);
        }

        public void DeleteRecord(TableInfo table, List<ColumnInfo> columns, string selectedId, DataGridView dataGrid)
        {
            if (string.IsNullOrWhiteSpace(selectedId))
                throw new Exception("Не выбрана запись для удаления.");

            ColumnInfo keyColumn = columns.First(c => c.IsPrimaryKey);
            string sql = "DELETE FROM " + table.FullName + " WHERE " + keyColumn.SafeName + " = @Id";
            ExecuteNonQuery(sql, new[] { new SqlParameter("@Id", selectedId) });
            DisplayTableData(table, dataGrid);
        }

        private object GetValue(string text, ColumnInfo column)
        {
            if (string.IsNullOrWhiteSpace(text))
            {
                if (column.IsNullable)
                    return DBNull.Value;
                throw new Exception("Поле " + column.ColumnName + " обязательно для заполнения.");
            }

            string type = column.DataType.ToLowerInvariant();
            string value = text.Trim();

            if (type.Contains("int"))
                return int.Parse(value, CultureInfo.InvariantCulture);
            if (type == "money" || type == "decimal" || type == "numeric")
                return decimal.Parse(value.Replace(',', '.'), CultureInfo.InvariantCulture);
            if (type == "date" || type == "datetime" || type == "datetime2")
                return DateTime.Parse(value, CultureInfo.CurrentCulture);
            if (type == "bit")
                return value == "1" || value.Equals("true", StringComparison.OrdinalIgnoreCase) || value.Equals("да", StringComparison.OrdinalIgnoreCase);

            return value;
        }
    }
}
