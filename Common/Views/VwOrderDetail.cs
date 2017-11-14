using System;

namespace Common.Views
{
    public class VwOrderDetail
    {
        #region Public Properties

        public decimal Discount { get; set; }

        public string Item { get; set; }
        public string Category { get; set; }

        public Guid OrderID { get; set; }

        public decimal? TotalPrice { get; set; }

        public Guid ProductID { get; set; }

        public int? Quantity { get; set; }

        public decimal? UnitPrice { get; set; }

        public string Username { get; set; }

        public decimal? VAT { get; set; }

        public int Stock { get; set; }

        #endregion Public Properties
    }
}