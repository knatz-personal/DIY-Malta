using System;

namespace Common.Views
{
    public class VwProduct
    {
        #region Public Properties

        public int CategoryID { get; set; }

        public string Description { get; set; }

        public Guid ID { get; set; }

        public string Image { get; set; }

        public bool IsActive { get; set; }

        public string Name { get; set; }

        public Guid? SaleID { get; set; }

        public int Stock { get; set; }

        public decimal UnitPrice { get; set; }

        public decimal VAT { get; set; }
        public decimal? Discount { get; set; }

        #endregion Public Properties
    }
}