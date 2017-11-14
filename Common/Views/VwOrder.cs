using System;

namespace Common.Views
{
    public class VwOrder
    {
        private string username;
        public Guid ID { get; set; }
        public DateTime DatePlaced { get; set; }
        public int? State { get; set; }

        public string Username
        {
            get { return username; }
            set
            {
                username = string.IsNullOrEmpty(value)
                    ? "<span class='ui-state-error'>Error! Orphan Record! </span>"
                    : value;
            }
        }

        public string Status { get; set; }
        public decimal? SubTotal { get; set; }
        public decimal? Tax { get; set; }
        public decimal GrandTotal { get; set; }
    }
}