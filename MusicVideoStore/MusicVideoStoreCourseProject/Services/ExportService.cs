using System;
using System.IO;
using System.Text;
using System.Windows.Forms;
using Word = Microsoft.Office.Interop.Word;

namespace MusicVideoStoreCourseProject.Services
{
    public class ExportService
    {
        public void ExportToExcel(DataGridView dataGrid)
        {
            if (dataGrid.DataSource == null)
                throw new Exception("Нет данных для экспорта.");

            Type excelType = Type.GetTypeFromProgID("Excel.Application");
            if (excelType == null)
            {
                SaveCsv(dataGrid);
                return;
            }

            dynamic excel = Activator.CreateInstance(excelType);
            excel.Visible = true;
            dynamic workbook = excel.Workbooks.Add();
            dynamic sheet = workbook.ActiveSheet;
            sheet.Name = "Отчет";

            for (int column = 0; column < dataGrid.Columns.Count; column++)
            {
                sheet.Cells[1, column + 1] = dataGrid.Columns[column].HeaderText;
                sheet.Cells[1, column + 1].Font.Bold = true;
            }

            for (int row = 0; row < dataGrid.Rows.Count; row++)
            {
                for (int column = 0; column < dataGrid.Columns.Count; column++)
                    sheet.Cells[row + 2, column + 1] = Convert.ToString(dataGrid.Rows[row].Cells[column].Value);
            }

            sheet.Columns.AutoFit();
        }

        public void ExportToWord(DataGridView dataGrid)
        {
            if (dataGrid.DataSource == null || dataGrid.Columns.Count == 0 || dataGrid.Rows.Count == 0)
                throw new Exception("Нет данных для экспорта.");

            int rowCount = 0;
            foreach (DataGridViewRow row in dataGrid.Rows)
            {
                if (!row.IsNewRow)
                    rowCount++;
            }

            int columnCount = dataGrid.Columns.Count;
            if (rowCount == 0)
                throw new Exception("Нет строк для экспорта.");

            object[,] dataArray = new object[rowCount, columnCount];
            int rowIndex = 0;
            foreach (DataGridViewRow row in dataGrid.Rows)
            {
                if (row.IsNewRow)
                    continue;

                for (int column = 0; column < columnCount; column++)
                    dataArray[rowIndex, column] = row.Cells[column].Value;

                rowIndex++;
            }

            Word.Application wordApplication = new Word.Application();
            Word.Document document = wordApplication.Documents.Add();
            wordApplication.Visible = true;
            document.PageSetup.Orientation = Word.WdOrientation.wdOrientLandscape;

            Word.Range range = document.Content.Application.Selection.Range;
            StringBuilder tableText = new StringBuilder();

            for (int row = 0; row < rowCount; row++)
            {
                for (int column = 0; column < columnCount; column++)
                {
                    if (column > 0)
                        tableText.Append("\t");

                    tableText.Append(Convert.ToString(dataArray[row, column]));
                }

                tableText.AppendLine();
            }

            range.Text = tableText.ToString();

            object separator = Word.WdTableFieldSeparator.wdSeparateByTabs;
            object rows = rowCount;
            object columns = columnCount;
            object applyBorders = true;
            object autoFit = true;
            object autoFitBehavior = Word.WdAutoFitBehavior.wdAutoFitContent;
            object missing = Type.Missing;

            range.ConvertToTable(
                ref separator,
                ref rows,
                ref columns,
                ref missing,
                ref missing,
                ref applyBorders,
                ref missing,
                ref missing,
                ref missing,
                ref missing,
                ref missing,
                ref missing,
                ref missing,
                ref autoFit,
                ref autoFitBehavior,
                ref missing);

            range.Select();
            wordApplication.Selection.Tables[1].Select();
            wordApplication.Selection.Tables[1].Rows.AllowBreakAcrossPages = 0;
            wordApplication.Selection.Tables[1].Rows.Alignment = 0;
            wordApplication.Selection.Tables[1].Rows[1].Select();
            wordApplication.Selection.InsertRowsAbove(1);
            wordApplication.Selection.Tables[1].Rows[1].Select();
            wordApplication.Selection.Tables[1].Rows[1].Range.Bold = 1;
            wordApplication.Selection.Tables[1].Rows[1].Range.Font.Name = "Tahoma";
            wordApplication.Selection.Tables[1].Rows[1].Range.Font.Size = 14;

            for (int column = 0; column < columnCount; column++)
                wordApplication.Selection.Tables[1].Cell(1, column + 1).Range.Text = dataGrid.Columns[column].HeaderText;

            foreach (Word.Section section in wordApplication.ActiveDocument.Sections)
            {
                Word.Range headerRange = section.Headers[Word.WdHeaderFooterIndex.wdHeaderFooterPrimary].Range;
                headerRange.Fields.Add(headerRange, Word.WdFieldType.wdFieldPage);
                headerRange.Text = "Отчет";
                headerRange.Font.Size = 16;
                headerRange.ParagraphFormat.Alignment = Word.WdParagraphAlignment.wdAlignParagraphCenter;
            }
        }

        private void SaveCsv(DataGridView dataGrid)
        {
            using (SaveFileDialog dialog = new SaveFileDialog())
            {
                dialog.Filter = "CSV-файл (*.csv)|*.csv";
                dialog.FileName = "report.csv";
                if (dialog.ShowDialog() != DialogResult.OK)
                    return;

                StringBuilder builder = new StringBuilder();
                for (int i = 0; i < dataGrid.Columns.Count; i++)
                {
                    if (i > 0) builder.Append(';');
                    builder.Append(Escape(dataGrid.Columns[i].HeaderText));
                }
                builder.AppendLine();

                foreach (DataGridViewRow row in dataGrid.Rows)
                {
                    for (int i = 0; i < dataGrid.Columns.Count; i++)
                    {
                        if (i > 0) builder.Append(';');
                        builder.Append(Escape(Convert.ToString(row.Cells[i].Value)));
                    }
                    builder.AppendLine();
                }

                File.WriteAllText(dialog.FileName, builder.ToString(), Encoding.UTF8);
                MessageBox.Show("Microsoft Excel не найден, поэтому данные сохранены в CSV.", "Экспорт", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void SaveHtmlWord(DataGridView dataGrid)
        {
            using (SaveFileDialog dialog = new SaveFileDialog())
            {
                dialog.Filter = "Документ Word (*.doc)|*.doc";
                dialog.FileName = "report.doc";
                if (dialog.ShowDialog() != DialogResult.OK)
                    return;

                StringBuilder builder = new StringBuilder();
                builder.AppendLine("<html><head><meta charset='utf-8'><style>body{font-family:Times New Roman;font-size:14pt;}table{border-collapse:collapse;}th,td{border:1px solid #000;padding:4px;}th{font-weight:bold;}</style></head><body>");
                builder.AppendLine("<h2>Отчет информационной системы МУЗЫКАЛЬНЫЙ (ВИДЕО-) МАГАЗИН</h2>");
                builder.AppendLine("<table border='1' cellspacing='0' cellpadding='4'>");
                builder.AppendLine("<tr>");
                foreach (DataGridViewColumn column in dataGrid.Columns)
                    builder.Append("<th>").Append(Html(column.HeaderText)).AppendLine("</th>");
                builder.AppendLine("</tr>");

                foreach (DataGridViewRow row in dataGrid.Rows)
                {
                    builder.AppendLine("<tr>");
                    foreach (DataGridViewCell cell in row.Cells)
                        builder.Append("<td>").Append(Html(Convert.ToString(cell.Value))).AppendLine("</td>");
                    builder.AppendLine("</tr>");
                }

                builder.AppendLine("</table></body></html>");
                File.WriteAllText(dialog.FileName, builder.ToString(), Encoding.UTF8);
                MessageBox.Show("Отчет сохранен в формате Word.", "Экспорт", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private string Escape(string text)
        {
            if (text == null) return "";
            return '"' + text.Replace("\"", "\"\"") + '"';
        }

        private string Html(string text)
        {
            if (text == null) return "";
            return text.Replace("&", "&amp;").Replace("<", "&lt;").Replace(">", "&gt;");
        }
    }
}
