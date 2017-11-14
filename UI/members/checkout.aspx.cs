using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net.Mail;
using System.Net.Mime;
using System.Text;
using System.Web;
using System.Web.Script.Serialization;
using System.Web.Services;
using System.Web.UI;
using BLL.Accounts;
using BLL.Orders;
using BLL.ShoppingCarts;
using Common.Utilities;
using Common.Views;

namespace UI.members
{
    public partial class checkout : Page
    {
        #region Public Methods

        [WebMethod]
        public static string GetCheckoutSummary()
        {
            string username = HttpContext.Current.User.Identity.Name;
            var temp = new CheckoutSummary
            {
                SubTotal = new BlOrders().GetSubTotal(username),
                Tax = new BlOrders().GetTax(username)
            };

            temp.GrandTotal = new BlOrders().GetGrandTotal(temp.SubTotal,
                temp.Tax);

            string json = new JavaScriptSerializer().Serialize(temp);

            return json;
        }

        [WebMethod]
        public static void PlaceOrder()
        {
            try
            {
                string username = HttpContext.Current.User.Identity.Name;
                IQueryable<VwShoppingCart> carts = new BlShoppingCarts().ListAll(username);
                var user = new BlUsers().GetUser(username);

                var blo = new BlOrders();
                var newOrder = new VwOrder
                {
                    DatePlaced = DateTime.Now,
                    State = new BlOrderStates().GetStateID("Processing"),
                    ID = Guid.NewGuid()
                };
                blo.Create(newOrder);



                var order = blo.Read(newOrder.ID);
                
                if (carts != null && user != null && order != null)
                {
                    var html = new StringBuilder();
                    html.Append("<table width='650' cellspacing='0' cellpadding='0' border='0' style='border:1px solid #eaeaea'>");
                    html.Append("<thead><tr><th bgcolor='#EAEAEA' align='left' style='font-size:13px;padding:3px 9px'>Item</th>" +
                                "<th bgcolor='#EAEAEA' align='left' style='font-size:13px;padding:3px 9px'>Unit Price</th>" +
                                "<th bgcolor='#EAEAEA' align='center' style='font-size:13px;padding:3px 9px'>Quantity</th>" +
                                "<th bgcolor='#EAEAEA' align='right' style='font-size:13px;padding:3px 9px'>Total</th></tr></thead>");
                    foreach (VwShoppingCart item in carts)
                    {
                        new BlOrderDetails().Create(new VwOrderDetail
                        {
                            Item = item.Name,
                            OrderID = order.ID,
                            ProductID = item.ProductID,
                            Quantity = item.Quantity,
                            UnitPrice = item.DiscountPrice == 0 ? item.UnitPrice : item.DiscountPrice,
                            Username = item.Username,
                            Stock = item.Stock,
                            TotalPrice = item.TotalPrice,
                            VAT = item.Tax
                        });
                        html.Append("<tbody bgcolor='#F6F6F6'><tr>");
                        html.Append("<td valign='top' align='left' style='font-size:11px;padding:3px 9px;border-bottom:1px dotted #cccccc'>");
                        html.Append("<strong style='font-size:11px'>" + item.Name + "</strong></td>");
                        html.Append("<td valign='top' align='left' style='font-size:11px;padding:3px 9px;border-bottom:1px dotted #cccccc'>");
                        html.Append(string.Format(new CultureInfo("MT"), "{0:C}", item.DiscountPrice == 0 ? item.UnitPrice : item.DiscountPrice) + "</td>");
                        html.Append("<td valign='top' align='center' style='font-size:11px;padding:3px 9px;border-bottom:1px dotted #cccccc'>");
                        html.Append(item.Quantity + "</td>");
                        html.Append("<td valign='top' align='right' style='font-size:11px;padding:3px 9px;border-bottom:1px dotted #cccccc'>");
                        html.Append("<span>Excl. VAT:</span>");
                        html.Append("<span>" + string.Format(new CultureInfo("MT"), "{0:C}", item.TotalPrice) + "</span></td>");
                        html.Append("</tr></tbody>");
                        new BlShoppingCarts().Delete(item.ID);
                    }

                    var subTotal = new BlOrders().GetSubTotal(order.ID);
                    var tax = new BlOrders().GetTax(order.ID);
                    var grandTotal = new BlOrders().GetGrandTotal(subTotal, tax);

                    html.Append("<tbody><tr><td colspan='4' style='background:#eaeaea;text-align:center;padding:5px;' >&nbsp;</td></tr></tbody>");
                    html.Append("<tbody><tr><td align='right' style='padding:3px 9px' colspan='3'>Subtotal (Excl.VAT)</td>");
                    html.Append("<td align='right' style='padding:3px 9px'><span>" + string.Format(new CultureInfo("MT"), "{0:C}", subTotal) + "</span></td></tr>");
                    html.Append("<tr><td align='right' style='padding:3px 9px' colspan='3'>Delivery (Excl.VAT)</td>");
                    html.Append("<td align='right' style='padding:3px 9px'><span>" + string.Format(new CultureInfo("MT"), "{0:C}", 0) + "</span></td></tr>");
                    html.Append("<tr><td align='right' style='padding:3px 9px' colspan='3'> VAT</td>");
                    html.Append("<td align='right' style='padding:3px 9px'><span>" + string.Format(new CultureInfo("MT"), "{0:C}", tax) + "</span></td></tr>");
                    html.Append("<tr><td align='right' style='padding:3px 9px' colspan='3'><strong>Grand Total (Incl.VAT)</strong></td>");
                    html.Append("<td align='right' style='padding:3px 9px'><strong><span>" + string.Format(new CultureInfo("MT"), "{0:C}", grandTotal) + "</span></strong></td></tr>");
                    html.Append("</tbody></table>");

                    SendOrderInvoice(user, order, html);
                }
            }
            catch (Exception)
            {
                throw new Exception("Error occurred while placing order.");
            }
        }

        private static void SendOrderInvoice(VwUser user, VwOrder order, StringBuilder orderHtml)
        {
            var html = new StringBuilder();
            html.Append("<html><head><style type='text/css'>");
            html.Append("body,td {font-family: arial, sans-serif; font-size: 13px");
            html.Append("a:link,a:active {color: #1155CC;text-decoration: none}");
            html.Append("a:hover {text-decoration: underline;cursor: pointer}");
            html.Append(" a:visited {color: #6611CC}");
            html.Append("img {border: 0px}");
            html.Append("pre {white-space: pre;white-space: -moz-pre-wrap;white-space: -o-pre-wrap;" +
                        "white-space: pre-wrap;word-wrap: break-word;max-width: 800px;" +
                        "overflow: auto;} </style></head>");
            html.Append("<body><table border='0' width='100%' cellpadding='0' cellspacing='0'>");
            html.Append("<tbody><tr> <td colspan='2'><table border='0' width='100%' cellpadding='12' cellspacing='0'>");
            html.Append("<tbody><tr><td><div style='overflow: hidden;'><font size='-1'>");
            html.Append(
                "<div style='margin-top:0;margin-bottom:0;margin-left:0;margin-right:0;padding-top:0;padding-bottom:0;padding-left:0;padding-right:0;min-width:100%;background-color:#f6f9fb'>");
            html.Append(
                "<table style='border-collapse:collapse;border-spacing:0;display:table;table-layout:fixed;width:100%;min-width:620px;background-color:#f6f9fb'>");
            html.Append(
                "<tbody><tr><td style='padding-top:0;padding-bottom:0;padding-left:0;padding-right:0;vertical-align:top'><center>");
            html.Append("<table style='border-collapse:collapse;border-spacing:0;width:100%'><tbody><tr>");
            html.Append(
                "<td style='padding-top:8px;padding-bottom:8px;padding-left:32px;padding-right:32px;vertical-align:top;font-size:11px;font-weight:400;letter-spacing:0.01em;line-height:17px;width:50%;color:#b3b3b3;text-align:left;font-family:sans-serif'></td>");
            html.Append(
                "<td style='padding-top:8px;padding-bottom:8px;padding-left:32px;padding-right:32px;vertical-align:top;font-size:11px;font-weight:400;letter-spacing:0.01em;line-height:17px;width:50%;color:#b3b3b3;text-align:left;font-family:sans-serif'></td>");
            html.Append(
                "<div>No Images? <a style='font-weight:700;letter-spacing:0.03em;text-decoration:none;color:#b3b3b3'>Click here</a></div></td></tr>");
            html.Append(
                "</tbody></table><table style='border-collapse:collapse;border-spacing:0;Margin-left:auto;Margin-right:auto;width:600px'>");
            html.Append(
                "<tbody><tr><td style='padding-top:16px;padding-bottom:32px;padding-left:0;padding-right:0;vertical-align:top;font-size:24px;line-height:32px;letter-spacing:-0.01em;color:#2e3b4e;font-family:Cabin,Avenir,sans-serif!important' align='center'>");
            html.Append(
                "<center><div><img style='border-left-width:0;border-top-width:0;border-bottom-width:0;border-right-width:0;display:block;Margin-left:auto;Margin-right:auto;max-width:225px' src='cid:logo' alt='' height='134' width='150'></div></center></td>");
            html.Append(
                "</tr></tbody></table><table style='border-collapse:collapse;border-spacing:0;width:100%'><tbody><tr>");
            html.Append(
                "<td style='padding-top:0;padding-bottom:0;padding-left:0;padding-right:0;vertical-align:top' align='center'>");
            html.Append(
                "<table style='border-collapse:collapse;border-spacing:0;Margin-left:auto;Margin-right:auto;width:600px'>");
            html.Append(
                "<tbody><tr><td style='padding-top:0;padding-bottom:0;padding-left:0;padding-right:0;vertical-align:top;text-align:left'><div><div style='font-size:52px;line-height:52px'>&nbsp;</div></div>");
            html.Append(
                "<table style='border-collapse:collapse;border-spacing:0;width:100%'><tbody><tr><td style='padding-top:0;padding-bottom:0;padding-left:90px;padding-right:90px;vertical-align:top'>");
            //Start
            html.Append(
                "<tr><h3 style='Margin-top:0;color:#2e3b4e;font-size:16px;Margin-bottom:16px;font-family:Cabin,Avenir,sans-serif!important;line-height:24px'>Hello, " +
                user.FirstName + " " + user.LastName + "</h3>");
            html.Append("<p style='font-size:12px;line-height:16px;margin:0'>Thank you for your order from DIY-Malta. You can check the status of your order by");
            html.Append("<a target='_blank' style='color:#1e7ec8' href='#'> logging into your account</a>.<br/> If you have any questions about your order please contact us on ");
            html.Append("<a target='_blank' style='color:#1e7ec8' href='mailto:esales@diymalta.com'>esales@diymalta.com</a> or call us on<span><a target='_blank' value='+35622251234' href='tel:%2B356%2022251234'> +356 22251234</a></span><p>Monday - Friday, 9am - 6pm.</p>");
            html.Append("</p><p style='font-size:12px;line-height:16px;margin:0'>Your order confirmation is below.</p><br/>");
            html.Append("<h2 style='font-size:15px;font-weight:600;margin:0'>");
            html.Append("Your Order code " + order.ID + " <em> <br/>(placed on " + order.DatePlaced + ")</em></h2><br/>");
            //render order table
            html.Append(orderHtml);
            //End
            html.Append(
                "<br/><div style='background:#eaeaea;text-align:center;padding:5px;'><p style='font-size:12px;margin:0; text-align:center;'>Thank you, <strong>DIY-Malta</strong></p></div>");
            html.Append("<div><h3>You can check the status of your order by:</h3><ul>");
            html.Append(
                "<li><b>Checking your email:</b> When the status of your order changes we will send you an email like this one.</li>");
            html.Append(
                "<li><b>Visiting your dashboard in the diymalta.com website:</b>You will be able to view your order history and the status of your most recent order.</li><li><b>Contacting us:</b> If you still have questions about your order, please <a href='#'>contact us</a>.</li></ul>");
            html.Append(
                "</div><p>This is an automatic e-mail message generated by the DIY-Malta system. Please do not reply to this e-mail.</p>");

            html.Append(
                "</td></tr></tbody></table><div style='font-size:26px;line-height:26px'>&nbsp;</div></td></tr></tbody></table>");
            html.Append(
                "</td></tr></tbody></table><table style='border-collapse:collapse;border-spacing:0;width:100%;background-color:#f6f9fb'><tbody><tr>");
            html.Append(
                "<td style='padding-top:60px;padding-bottom:55px;padding-left:0;padding-right:0;vertical-align:top' align='center'><table style='border-collapse:collapse;border-spacing:0;width:600px'><tbody><tr>");
            html.Append(
                "<td style='padding-top:0;padding-bottom:22px;padding-left:0;padding-right:5px;vertical-align:top;font-size:11px;font-weight:400;letter-spacing:0.01em;line-height:17px;text-align:left;width:55%;color:#b3b3b3;font-family:sans-serif'><table style='border-collapse:collapse;border-spacing:0'>");
            html.Append(
                "<tbody><tr><td style='padding-top:0;padding-bottom:20px;padding-left:0;padding-right:15px;vertical-align:top;text-transform:uppercase;line-height:21px;font-size:11px'>");
            html.Append(
                "<a style='font-weight:700;letter-spacing:0.03em;text-decoration:none;color:#b3b3b3;display:block;font-family:sans-serif'><img style='border-left-width:0;border-top-width:0;border-bottom-width:0;border-right-width:0' src='cid:twitter' align='top' height='20' width='25'>Tweet</a></td>");
            html.Append(
                "<td style='padding-top:0;padding-bottom:20px;padding-left:0;padding-right:15px;vertical-align:top;text-transform:uppercase;line-height:21px;font-size:11px'><a style='font-weight:700;letter-spacing:0.03em;text-decoration:none;color:#b3b3b3;display:block;font-family:sans-serif' rel='cs_facebox'><img style='border-left-width:0;border-top-width:0;border-bottom-width:0;border-right-width:0' src='cid:facebook' align='top' height='20' width='25'>Like</a></td>");
            html.Append(
                "<td style='padding-top:0;padding-bottom:20px;padding-left:0;padding-right:15px;vertical-align:top;text-transform:uppercase;line-height:21px;font-size:11px'><a style='font-weight:700;letter-spacing:0.03em;text-decoration:none;color:#b3b3b3;display:block;font-family:sans-serif'><img style='border-left-width:0;border-top-width:0;border-bottom-width:0;border-right-width:0' src='cid:arrow' align='top' height='20' width='25'>Forward</a></td>");
            html.Append("</tr></tbody></table></td>");
            html.Append(
                "<td style='padding-top:0;padding-bottom:22px;padding-left:5px;padding-right:0;vertical-align:top;font-size:11px;font-weight:400;letter-spacing:0.01em;line-height:17px;text-align:right;width:45%;color:#b3b3b3;font-family:sans-serif'><div style='font-size:1px;line-height:20px;width:100%'>&nbsp;</div>");
            html.Append(
                "<div><span><span><a style='font-weight:700;letter-spacing:0.03em;text-decoration:none;color:#b3b3b3'>Preferences</a><span> &nbsp;|&nbsp; </span></span></span><span><a style='font-weight:700;letter-spacing:0.03em;text-decoration:none;color:#b3b3b3'>Unsubscribe</a></span></div></td></tr></tbody></table></td></tr></tbody></table></center></td></tr></tbody></table>");
            html.Append("</div></font></div></td></tr></tbody></table></td></tr></tbody></table></body></html>");

            AlternateView avHtml = AlternateView.CreateAlternateViewFromString
                (html.ToString(), null, MediaTypeNames.Text.Html);

            var logo = new LinkedResource(HttpContext.Current.Server.MapPath("~/img/skeleton/logo.png"), "image/png")
            {
                ContentId = "logo"
            };
            avHtml.LinkedResources.Add(logo);

            var arrow = new LinkedResource(HttpContext.Current.Server.MapPath("~/img/skeleton/arrow.png"), "image/png")
            {
                ContentId = "arrow"
            };
            avHtml.LinkedResources.Add(arrow);

            var facebook = new LinkedResource(HttpContext.Current.Server.MapPath("~/img/skeleton/facebook.png"),
                "image/png")
            {
                ContentId = "facebook"
            };
            avHtml.LinkedResources.Add(facebook);

            var twitter = new LinkedResource(HttpContext.Current.Server.MapPath("~/img/skeleton/twitter.png"),
                "image/png")
            {
                ContentId = "twitter"
            };
            avHtml.LinkedResources.Add(twitter);

            new Communication().SendEmail(user.Email, "DIY-Malta Order Confirmation", avHtml);
        }

        #endregion Public Methods

        #region Protected Methods

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                ListViewCheckout.DataSource = new BlShoppingCarts().ListAll(Context.User.Identity.Name);
                ListViewCheckout.DataBind();
            }
        }

        #endregion Protected Methods
    }

    public class CheckoutSummary
    {
        #region Public Properties

        public decimal? GrandTotal { get; set; }

        public decimal SubTotal { get; set; }

        public decimal Discount { get; set; }

        public decimal Tax { get; set; }

        #endregion Public Properties
    }
}