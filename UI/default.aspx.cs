using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Script.Serialization;
using System.Web.Services;
using System.Web.UI;
using BLL.Accounts;
using BLL.Catalogue;
using Common.Views;

namespace UI
{
    public partial class _default : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
        }

        [WebMethod]
        public static IQueryable<string> FindProduct(string query)
        {
            IQueryable<string> result = new BlProducts().FindProduct(query).OrderBy(p => p);
            return result;
        }

        [WebMethod]
        public static object LoadCatalog(string query, int category, int startIndex, int pageSize)
        {
            try
            {
                int count = 0;

                var html = new StringBuilder();
                string username = HttpContext.Current.User.Identity.Name;
                int? userType = string.IsNullOrEmpty(username)
                    ? new BlUsers().GetTypeID("Customer")
                    : new BlUsers().GetUser(username).UserType;
                if (userType != null)
                {
                    IEnumerable<VwProduct> list = new BlProducts().PagedList(query, category, (int)userType);
                    IList<VwProduct> vwProducts = list as IList<VwProduct> ?? list.ToList();
                    count = vwProducts.Count();
                    list = vwProducts.OrderBy(p => p.Name).Skip((startIndex - 1) * pageSize).Take(pageSize);
                    CreateCatalogHtml(list, count, html);
                }

                return new { Result = "OK", Records = html.ToString(), TotalRecordCount = count };
            }
            catch (Exception)
            {
                return
                    new
                    {
                        Result = "ERROR",
                        Message = "An error occurred while loading records. Sorry for the inconvenience."
                    };
            }
        }

        private static void CreateCatalogHtml(IEnumerable<VwProduct> list, int count, StringBuilder html)
        {
            if (list != null && count > 0)
            {
                foreach (VwProduct item in list)
                {
                    html.Append("<div class='product-box'>");
                    if (item.Discount > 0)
                    {
                        html.Append("<div style='position:relative;'><span class='SaleMarker'>Sale</span></div>"); 
                    }
                    html.Append("<a class='image featured' href='/detail.aspx?productId=" + item.ID + "'>");
                    html.Append("<img alt='Image of " + item.Name + "' src='" + item.Image + "'></a>");
                    html.Append("<div class='box'>");
                    html.Append("<p><a href='/detail.aspx?productId=" + item.ID + "'>" + item.Name + "</a></p>");
                    html.Append("<p class='stockDisplay'>");
                    html.Append("<span class='incartmessage'>Stock [ " + item.Stock + " ]</span><br />");
                   
                    if (item.Discount > 0)
                    {
                        var price = (item.UnitPrice*(100 - item.Discount)/100);
                        html.Append("<span>VAT [ " + String.Format(new CultureInfo("MT"), "{0:C}", price*(item.VAT/ 100) ) + "]</span><br />");
                        html.Append(" <span style='text-decoration: line-through;width:40%;float:left;color:red;font-size:small;text-align:left;'>" + String.Format(new CultureInfo("MT"), "{0:C}", item.UnitPrice) +
                                "</span><br />");
                        html.Append(" <span class='price'>" + String.Format(new CultureInfo("MT"), "{0:C}", price) +
                                "</span><br />");
                    }
                    else
                    {
                        html.Append("<span>VAT [ " + String.Format(new CultureInfo("MT"), "{0:C}", (item.UnitPrice * item.VAT) / 100) + "]</span><br />");
                        html.Append(" <span class='price'>" + String.Format(new CultureInfo("MT"), "{0:C}", item.UnitPrice) +
                                "</span><br />");
                    }

                    html.Append("</p>");
                    html.Append("<a href='#' id='item-id-" + item.ID +
                                "' class='product-bttn'><span class='icon icon-shopping-cart'></span>&nbsp;Add To Cart</a>");
                    html.Append("<input name='quatity' class='productQuantity' type='text' />");
                    html.Append("</div></div>");
                }
            }
            else
            {
                html.Append(
                    "<div style=\"background: #fff; padding: 20px; text-align: center;height:600px;\">No records found</div>");
            }
        }

        [WebMethod]
        public static string GetCartSummary()
        {
            var temp = new CartSummary();
            if (HttpContext.Current.User.Identity.IsAuthenticated)
            {
                string username = HttpContext.Current.User.Identity.Name;
                int count = 0;
                decimal total = 0;

                if (!string.IsNullOrEmpty(username))
                {
                    count = new BlProducts().GetCartItemsCount(username);
                    total = new BlProducts().GetCartTotalPrice(username);
                }

                temp.ItemCount = count;
                temp.CartTotal = total;
            }
            else
            {
                int num = 0;
                HttpCookie cartCookie = HttpContext.Current.Request.Cookies["shoppingCart"];
                if (cartCookie != null) temp.ItemCount = cartCookie.Values.Count;
                temp.CartTotal = 0;
            }

            string json = new JavaScriptSerializer().Serialize(temp);

            return json;
        }

        [WebMethod]
        public static bool AddItemToCart(Guid productId, int quantity)
        {
            bool result = false;

            try
            {
                VwProduct p = new BlProducts().Read(productId);
                if (p != null && p.Stock > 0)
                {
                    if (HttpContext.Current.User.Identity.IsAuthenticated)
                    {
                        result = new BlProducts().AddToCart(new VwShoppingCart
                        {
                            ProductID = productId,
                            Username = HttpContext.Current.User.Identity.Name,
                            Quantity = quantity
                        });
                    }
                    else
                    {
                        HttpCookie cartCookie;

                        if (HttpContext.Current.Request.Cookies["shoppingCart"] == null)
                        {
                            cartCookie = new HttpCookie("shoppingCart");
                        }
                        else
                        {
                            cartCookie = HttpContext.Current.Request.Cookies["shoppingCart"];
                        }

                        cartCookie.Expires = DateTime.Now.AddMinutes(30);

                        if (cartCookie[productId.ToString()] != null)
                        {
                            int oldQty = Convert.ToInt32(cartCookie[productId.ToString()]);
                            cartCookie[productId.ToString()] = quantity.ToString(CultureInfo.InvariantCulture);
                        }
                        else
                        {
                            cartCookie[productId.ToString()] = quantity.ToString(CultureInfo.InvariantCulture);
                        }
                        HttpContext.Current.Response.Cookies.Add(cartCookie);
                        result = true;
                    }
                }
            }
            catch (Exception)
            {
                throw new Exception("An error occurred while adding item to your cart. Sorry for the inconvenience.");
            }
            return result;
        }
    }

    public class CartSummary
    {
        public int ItemCount { get; set; }

        public decimal CartTotal { get; set; }
    }
}