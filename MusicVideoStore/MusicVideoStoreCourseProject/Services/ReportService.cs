using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace MusicVideoStoreCourseProject.Services
{
    public class ReportService
    {
        private readonly string _connectionString = SqlServerCon.DefaultConnectionString;

        public void ShowCatalog(DataGridView dataGrid)
        {
            string sql = @"
SELECT p.ID_I AS [ID],
       p.Name AS [Название],
       p.Publisher AS [Издатель],
       g.Genre AS [Жанр],
       sg.Subgenre AS [Поджанр],
       mt.Carrier AS [Носитель],
       p.ReleaseYear AS [Год выпуска],
       s.Company AS [Поставщик],
       sh.Street AS [Магазин],
       s.Wholesale_price AS [Оптовая цена],
       p.Price AS [Розничная цена],
       p.Price - s.Wholesale_price AS [Наценка]
FROM dbo.Product_information p
INNER JOIN dbo.Type_of_genre g ON p.ID_G = g.ID_G
INNER JOIN dbo.Type_of_subgenre sg ON p.ID_Subgenre = sg.ID_Subgenre
INNER JOIN dbo.Media_type mt ON p.ID_C = mt.ID_C
INNER JOIN dbo.Supplier s ON p.ID_Supplier = s.ID_Supplier
INNER JOIN dbo.Shops sh ON s.ID_M = sh.ID_M
ORDER BY p.Name";
            Fill(dataGrid, sql, null);
        }

        public void ShowGenreGroup(DataGridView dataGrid)
        {
            string sql = @"
SELECT g.Genre AS [Жанр], COUNT(sg.ID_Subgenre) AS [Количество поджанров]
FROM dbo.Type_of_genre g
LEFT JOIN dbo.Type_of_subgenre sg ON g.ID_G = sg.ID_G
GROUP BY g.Genre
ORDER BY [Количество поджанров] DESC";
            Fill(dataGrid, sql, null);
        }

        public void ShowHavingReport(DataGridView dataGrid, int minCount)
        {
            string sql = @"
SELECT g.Genre AS [Жанр], COUNT(sg.ID_Subgenre) AS [Количество поджанров]
FROM dbo.Type_of_genre g
LEFT JOIN dbo.Type_of_subgenre sg ON g.ID_G = sg.ID_G
GROUP BY g.Genre
HAVING COUNT(sg.ID_Subgenre) > @MinCount
ORDER BY [Количество поджанров] DESC";
            Fill(dataGrid, sql, new[] { new SqlParameter("@MinCount", minCount) });
        }

        public void ShowPriceReport(DataGridView dataGrid)
        {
            string sql = @"
SELECT p.Name AS [Запись],
       s.Company AS [Поставщик],
       s.Wholesale_price AS [Оптовая цена],
       p.Price AS [Розничная цена],
       p.Price - s.Wholesale_price AS [Прибыль с экземпляра]
FROM dbo.Product_information p
INNER JOIN dbo.Supplier s ON p.ID_Supplier = s.ID_Supplier
ORDER BY [Прибыль с экземпляра] DESC";
            Fill(dataGrid, sql, null);
        }

        internal void ShowShopReport(DataGridView dataGrid)
        {
            string sql = @"
SELECT sh.Street AS [Магазин],
       sh.Number_of_people_per_day AS [Посетителей в день],
       COUNT(s.ID_Supplier) AS [Поставщиков],
       COUNT(p.ID_I) AS [Товарных позиций]
FROM dbo.Shops sh
LEFT JOIN dbo.Supplier s ON sh.ID_M = s.ID_M
LEFT JOIN dbo.Product_information p ON s.ID_Supplier = p.ID_Supplier
GROUP BY sh.Street, sh.Number_of_people_per_day
ORDER BY sh.Number_of_people_per_day DESC";
            Fill(dataGrid, sql, null);
        }

        private void Fill(DataGridView dataGrid, string sql, SqlParameter[] parameters)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            using (SqlCommand command = new SqlCommand(sql, connection))
            using (SqlDataAdapter adapter = new SqlDataAdapter(command))
            {
                if (parameters != null)
                    command.Parameters.AddRange(parameters);

                DataTable table = new DataTable();
                adapter.Fill(table);
                dataGrid.DataSource = table;
                dataGrid.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            }
        }
    }
}
