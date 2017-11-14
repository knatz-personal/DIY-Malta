using System;

namespace Common.Views
{
    /// <summary>
    ///  View Special Product Sale
    /// </summary>
    public class VwSpecialSale
    {
        public Guid ProductID { get; set; }

        public string Name { get; set; }

        public DateTime DateStarted { get; set; }

        public DateTime DateExpired { get; set; }

        public decimal Discount { get; set; }

        public Guid ID { get; set; }
    }
}