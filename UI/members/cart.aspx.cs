using System;
using System.Web;
using System.Web.Services;
using System.Web.UI;
using BLL.ShoppingCarts;
using Common.Views;

namespace UI.members
{
    public partial class cart : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                ListViewCart.DataSource = new BlShoppingCarts().ListAll(Context.User.Identity.Name);
                ListViewCart.DataBind();
            }
        }

        [WebMethod]
        public static bool RemoveItem(Guid item)
        {
            bool result = false;

            try
            {
                new BlShoppingCarts().Delete(item);
                result = true;
            }
            catch (Exception)
            {
                result = false;
            }

            return result;
        }
        
        [WebMethod]
        public static bool UpdateItem(Guid item, int quantity)
        {
            bool result = false;

            try
            {
                new BlShoppingCarts().Update(new VwShoppingCart
                {
                    ID = item,
                    Username = HttpContext.Current.User.Identity.Name,
                    Quantity = quantity
                });
                result = true;
            }
            catch (Exception)
            {
                result = false;
            }

            return result;
        }
    }
}