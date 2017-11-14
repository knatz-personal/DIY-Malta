using BLL.Accounts;
using BLL.Orders;
using Common.Views;
using System;
using System.Web;
using System.Web.UI;

namespace UI.members
{
    public partial class invoice : System.Web.UI.Page
    {
        #region Public Properties

        public VwOrder GetOrder { get; set; }

        public VwUser GetUser { get; set; }

        #endregion Public Properties

        #region Protected Methods

        protected void Page_Load(object sender, EventArgs e)
        {
            string qid = Request.QueryString["orderId"];
            if (qid != null)
            {
                var blo = new BlOrders();
                string username = HttpContext.Current.User.Identity.Name;
                GetOrder = blo.Read(new Guid(qid));
                if (GetOrder != null)
                {
                    GetOrder.Tax = blo.GetTax(GetOrder.ID);
                    GetOrder.SubTotal = blo.GetSubTotal(GetOrder.ID);
                    var grandTotal = blo.GetGrandTotal(GetOrder.SubTotal, GetOrder.Tax);
                    if (grandTotal != null)
                        GetOrder.GrandTotal = (decimal)grandTotal;
                }
                GetUser = new BlUsers().GetUser(username);
            }
            if (!Page.IsPostBack)
            {
                if (GetOrder != null)
                {
                    ItemList.DataSource = new BlOrders().GetItems(GetOrder.ID);
                }
                ItemList.DataBind();
            }
        }

        #endregion Protected Methods
    }
}