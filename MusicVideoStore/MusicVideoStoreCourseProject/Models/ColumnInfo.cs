namespace MusicVideoStoreCourseProject.Models
{
    public class ColumnInfo
    {
        public string ColumnName { get; set; }
        public string DataType { get; set; }
        public bool IsNullable { get; set; }
        public bool IsIdentity { get; set; }
        public bool IsPrimaryKey { get; set; }

        internal string SafeName
        {
            get { return "[" + ColumnName.Replace("]", "]]") + "]"; }
        }
    }
}
