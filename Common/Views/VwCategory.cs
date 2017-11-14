namespace Common.Views
{
    public class VwCategory
    {
        public string Description { get; set; }
        public int ID { get; set; }
        public string Name { get; set; }
        public int Quantity { get; set; }
        public int? RootParent { get; set; }
        public int? ParentID { get; set; }
    }
}