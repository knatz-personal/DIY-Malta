namespace Common.Views
{
    public class VwMenu
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public int? RootParent { get; set; }
        public int? ParentID { get; set; }
        public string Description { get; set; }
        public string Url { get; set; }
    }
}