using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using MusicVideoStoreCourseProject.Models;
using MusicVideoStoreCourseProject.Services;

namespace MusicVideoStoreCourseProject
{
    public partial class MainForm : Form
    {
        private readonly DatabaseTableService tableService = new DatabaseTableService();
        private readonly ReportService reportService = new ReportService();
        private readonly ExportService exportService = new ExportService();
        private readonly Dictionary<TabPage, TablePageData> pages = new Dictionary<TabPage, TablePageData>();
        public MainForm(string userRole)
        {
            InitializeComponent();

            Text = "Информационная система МУЗЫКАЛЬНЫЙ (ВИДЕО-) МАГАЗИН" + " - " + userRole;
            roleLabel.Text = "Роль: " + userRole;

            if (userRole == "Admin")
            {
                LoadTablesFromDatabase();
            }
            else
            {
                tabControl.TabPages.Clear();
            }

            tabControl.TabPages.Add(reportTabPage);
            reportService.ShowCatalog(reportGrid);
        }

        private void LoadTablesFromDatabase()
        {
            tabControl.TabPages.Clear();
            pages.Clear();

            foreach (TableInfo table in tableService.GetTables())
            {
                List<ColumnInfo> columns = tableService.GetColumns(table);
                CreateTablePage(table, columns);
            }
        }

        private void CreateTablePage(TableInfo table, List<ColumnInfo> columns)
        {
            TabPage page = new TabPage(table.TableName);
            page.UseVisualStyleBackColor = true;
            page.Padding = new Padding(0);

            Panel inputPanel = new Panel();
            inputPanel.Dock = DockStyle.Left;
            inputPanel.Width = 300;
            inputPanel.AutoScroll = true;
            inputPanel.BackColor = SystemColors.Control;

            DataGridView dataGrid = new DataGridView();
            dataGrid.Dock = DockStyle.Fill;
            dataGrid.AllowUserToAddRows = false;
            dataGrid.AllowUserToDeleteRows = false;
            dataGrid.ReadOnly = true;
            dataGrid.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dataGrid.MultiSelect = false;
            dataGrid.RowHeadersWidth = 28;
            dataGrid.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dataGrid.BackgroundColor = Color.White;
            dataGrid.BorderStyle = BorderStyle.FixedSingle;
            dataGrid.ScrollBars = ScrollBars.Both;
            dataGrid.CellClick += DataGrid_CellClick;
            page.Controls.Add(dataGrid);
            page.Controls.Add(inputPanel);

            Dictionary<string, TextBox> fields = new Dictionary<string, TextBox>();
            int y = 12;

            foreach (ColumnInfo column in columns)
            {
                if (!column.IsIdentity && !column.IsPrimaryKey)
                {
                    Label label = new Label();
                    label.Text = GetColumnCaption(column.ColumnName);
                    label.Location = new Point(10, y + 3);
                    label.Size = new Size(125, 23);
                    inputPanel.Controls.Add(label);

                    TextBox textBox = new TextBox();
                    textBox.Location = new Point(140, y);
                    textBox.Size = new Size(145, 23);
                    inputPanel.Controls.Add(textBox);
                    fields.Add(column.ColumnName, textBox);
                    y += 30;
                }
            }

            Button addButton = CreateButton("Добавить", 10, y + 10, AddButton_Click);
            Button changeButton = CreateButton("Изменить", 105, y + 10, ChangeButton_Click);
            Button deleteButton = CreateButton("Удалить", 200, y + 10, DeleteButton_Click);
            inputPanel.Controls.Add(addButton);
            inputPanel.Controls.Add(changeButton);
            inputPanel.Controls.Add(deleteButton);

            TablePageData pageData = new TablePageData
            {
                Table = table,
                Columns = columns,
                DataGrid = dataGrid,
                Fields = fields,
                SelectedId = ""
            };

            pages.Add(page, pageData);
            tabControl.TabPages.Add(page);
            tableService.DisplayTableData(table, dataGrid);
        }

        private Button CreateButton(string text, int x, int y, EventHandler handler)
        {
            Button button = new Button();
            button.Text = text;
            button.Location = new Point(x, y);
            button.Size = new Size(88, 30);
            button.Click += handler;
            return button;
        }

        private TablePageData GetCurrentPageData(Control control)
        {
            Control current = control;
            while (current != null && !(current is TabPage))
                current = current.Parent;

            TabPage page = current as TabPage;
            if (page == null || !pages.ContainsKey(page))
                return null;

            return pages[page];
        }

        private void CleanData(TablePageData pageData)
        {
            foreach (TextBox textBox in pageData.Fields.Values)
                textBox.Text = "";
            pageData.SelectedId = "";
        }

        private void AddButton_Click(object sender, EventArgs e)
        {
            ExecuteSafe(() =>
            {
                TablePageData pageData = GetCurrentPageData(sender as Control);
                if (pageData == null)
                    return;

                tableService.AddRecord(pageData.Table, pageData.Columns, pageData.Fields, pageData.DataGrid);
                CleanData(pageData);
            });
        }

        private void ChangeButton_Click(object sender, EventArgs e)
        {
            ExecuteSafe(() =>
            {
                TablePageData pageData = GetCurrentPageData(sender as Control);
                if (pageData == null)
                    return;

                tableService.ChangeRecord(pageData.Table, pageData.Columns, pageData.Fields, pageData.SelectedId, pageData.DataGrid);
                CleanData(pageData);
            });
        }

        private void DeleteButton_Click(object sender, EventArgs e)
        {
            ExecuteSafe(() =>
            {
                TablePageData pageData = GetCurrentPageData(sender as Control);
                if (pageData == null)
                    return;

                if (MessageBox.Show("Удалить выбранную запись?", "Подтверждение", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    tableService.DeleteRecord(pageData.Table, pageData.Columns, pageData.SelectedId, pageData.DataGrid);
                    CleanData(pageData);
                }
            });
        }

        private void DataGrid_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0)
                return;

            DataGridView grid = sender as DataGridView;
            TablePageData pageData = GetCurrentPageData(grid);
            if (pageData == null)
                return;

            ColumnInfo key = tableService.GetKeyColumn(pageData);
            pageData.SelectedId = Convert.ToString(grid.Rows[e.RowIndex].Cells[key.ColumnName].Value);

            foreach (var pair in pageData.Fields)
            {
                if (grid.Columns.Contains(pair.Key))
                    pair.Value.Text = Convert.ToString(grid.Rows[e.RowIndex].Cells[pair.Key].Value);
            }
        }

        private void CatalogButton_Click(object sender, EventArgs e) { ExecuteSafe(() => reportService.ShowCatalog(reportGrid)); }
        private void GroupButton_Click(object sender, EventArgs e) { ExecuteSafe(() => reportService.ShowGenreGroup(reportGrid)); }
        private void ProfitButton_Click(object sender, EventArgs e) { ExecuteSafe(() => reportService.ShowPriceReport(reportGrid)); }
        private void OrdersButton_Click(object sender, EventArgs e) { ExecuteSafe(() => reportService.ShowShopReport(reportGrid)); }
        private void ExcelButton_Click(object sender, EventArgs e) { ExecuteSafe(() => exportService.ExportToExcel(reportGrid)); }
        private void WordButton_Click(object sender, EventArgs e) { ExecuteSafe(() => exportService.ExportToWord(reportGrid)); }

        private void HavingButton_Click(object sender, EventArgs e)
        {
            ExecuteSafe(() =>
            {
                int count;
                if (!int.TryParse(havingCountField.Text, out count))
                    throw new Exception("Введите целое число в поле HAVING.");
                reportService.ShowHavingReport(reportGrid, count);
            });
        }

        private void ExecuteSafe(Action action)
        {
            try
            {
                action();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private string GetColumnCaption(string columnName)
        {
            Dictionary<string, string> captions = new Dictionary<string, string>
            {
                { "Name", "Название" },
                { "Publisher", "Издатель" },
                { "ReleaseYear", "Год выпуска" },
                { "Price", "Цена" },
                { "ID_C", "ID носителя" },
                { "ID_G", "ID жанра" },
                { "ID_Subgenre", "ID поджанра" },
                { "ID_Supplier", "ID поставщика" },
                { "Carrier", "Носитель" },
                { "Genre", "Жанр" },
                { "Subgenre", "Поджанр" },
                { "Company", "Компания" },
                { "Wholesale_price", "Опт. цена" },
                { "Street", "Улица" },
                { "Number_of_people_per_day", "Посетителей/день" }
            };

            return captions.ContainsKey(columnName) ? captions[columnName] : columnName;
        }
    }
}
