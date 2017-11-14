using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common.EntityModel;

namespace Common.Views
{
    public class VwOrdersDisplay
    {
        public string Username { get; set; }
        public List<VwOrder> Orders { get; set; }
    }
}
