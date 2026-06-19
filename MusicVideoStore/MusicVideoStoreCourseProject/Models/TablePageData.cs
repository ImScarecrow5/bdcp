using System.Collections.Generic;
using System.Windows.Forms;

namespace MusicVideoStoreCourseProject.Models
{
    public class TablePageData
    {
        public TableInfo Table { get; set; }
        public List<ColumnInfo> Columns { get; set; }
        public DataGridView DataGrid { get; set; }
        public Dictionary<string, TextBox> Fields { get; set; }
        public string SelectedId { get; set; }
    }
}
