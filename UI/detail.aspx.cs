using System;

namespace UI
{
    public partial class detail : System.Web.UI.Page
    {
        public Guid GetProductID { get; set; }

        protected void Page_Load(object sender, EventArgs e)
        {
            string qid = Request.QueryString["productId"];
            if (qid != null)
            {
                GetProductID = new Guid(qid);
            }
        }
    }
}