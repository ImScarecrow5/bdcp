namespace MusicVideoStoreCourseProject.Models
{
    public class TableInfo
    {
        public string SchemaName { get; set; }
        public string TableName { get; set; }

        public string FullName
        {
            get { return "[" + SchemaName.Replace("]", "]]") + "].[" + TableName.Replace("]", "]]") + "]"; }
        }

        public override string ToString()
        {
            return TableName;
        }
    }
}
