using BLL.Accounts;
using BLL.Catalogue;
using Common.Utilities;
using Common.Views;
using System;
using System.Web;
using System.Web.Security;
using System.Web.Services;

namespace UI.accounts
{
    public partial class login : System.Web.UI.Page
    {
        private static bool _isAlreadyLoggedIn;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                _isAlreadyLoggedIn = Context.User.Identity.IsAuthenticated;
                if (_isAlreadyLoggedIn)
                {
                    Response.Redirect("/default.aspx");
                }
            }

        }

        [WebMethod]
        public static bool Authenticate(string username, string password, bool isRemembered)
        {
            bool isAuthenticated = false;
            //Hashing security issue
            var hashedPassword = FormsAuthentication.HashPasswordForStoringInConfigFile(password, "SHA1");
            string encpassword = Encryption.EncryptTripleDES(hashedPassword);
            if (_isAlreadyLoggedIn == false)
            {
                if (!String.IsNullOrEmpty(hashedPassword))
                {

                    if (new BlUsers().Authenticate(username, encpassword))
                    {
                        isAuthenticated = true;

                        FormsAuthentication.SetAuthCookie(username, isRemembered);

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
                            //delete the cookie
                            cartCookie.Expires = DateTime.Now.AddDays(-1d);
                            HttpContext.Current.Response.Cookies.Add(cartCookie);
                        }

                    }

                }
            }

            return isAuthenticated;
        }
    }
}