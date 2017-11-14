using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using BLL.Accounts;
using BLL.Catalogue;
using Common.Utilities;
using Common.Views;

namespace UI.accounts
{
    public partial class alt_login : System.Web.UI.Page
    {
        private bool isAlreadyLoggedIn;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                isAlreadyLoggedIn = Context.User.Identity.IsAuthenticated;
                if (isAlreadyLoggedIn)
                {
                    Response.Redirect("/default.aspx");
                }
            }
        }

        protected void BttnLogin_Click(object sender, EventArgs e)
        {
            Authenticate(TxtBxUsername.Text, TxtBxPassword.Text, ChkBxRemember.Checked);
        }


        public void Authenticate(string username, string password, bool isRemembered)
        {
            var hashedPassword = FormsAuthentication.HashPasswordForStoringInConfigFile(password, "SHA1");
            string encpassword = Encryption.EncryptTripleDES(hashedPassword);
            if (isAlreadyLoggedIn == false)
            {
                if (!String.IsNullOrEmpty(hashedPassword))
                {

                    if (new BlUsers().Authenticate(username, encpassword))
                    {

                        FormsAuthentication.RedirectFromLoginPage(username, isRemembered);

                        if (HttpContext.Current.Request.Cookies["shoppingCart"] != null)
                        {
                            HttpCookie cartCookie = HttpContext.Current.Request.Cookies["shoppingCart"];

                            string[] keys = cartCookie.Values.AllKeys;

                            foreach (var key in keys)
                            {
                                var k = new VwShoppingCart()
                                {
                                    ID = Guid.NewGuid(),
                                    ProductID = new Guid(key),
                                    Username = username,
                                    Quantity = Convert.ToInt32(cartCookie[key])
                                };
                                new BlProducts().AddToCart(k);
                            }
                            cartCookie.Expires = DateTime.Now.AddDays(-1d);
                            HttpContext.Current.Response.Cookies.Add(cartCookie);
                        }

                    }

                }
            }
        }
    }
}