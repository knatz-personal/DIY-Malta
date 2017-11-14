using System.Collections.Generic;
using System.IO;
using BLL.Accounts;
using BLL.Orders;
using Common.Views;
using System;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Web.UI;
using iTextSharp.text;
using iTextSharp.text.html.simpleparser;
using iTextSharp.text.pdf;

namespace UI.members
{
    public partial class index : Page
    {
        #region Public Properties

        public VwOrder GetOrder { get; set; }

        public VwUser GetUser { get; set; }

        #endregion Public Properties

        #region Public Methods

        [WebMethod]
        public static object GetOrderStateOptions()
        {
            try
            {
                var records = new BlOrderStates().ListAll().Select(c => new { DisplayText = c.Name, Value = c.ID });
                return new { Result = "OK", Options = records };
            }
            catch (Exception ex)
            {
                return new { Result = "ERROR", ex.Message };
            }
        }

        [WebMethod]
        public static void DownloadInvoice(string html)
        {
            try
            {
                StringReader sr = new StringReader(html);
                Document pdfDoc = new Document(PageSize.A4, 10f, 10f, 100f, 0f);
                HTMLWorker htmlparser = new HTMLWorker(pdfDoc);
                PdfWriter.GetInstance(pdfDoc, HttpContext.Current.Response.OutputStream);
                pdfDoc.Open();
                htmlparser.Parse(sr);
                pdfDoc.Close();
                HttpContext.Current.Response.Write(pdfDoc);
                HttpContext.Current.Response.End();
            }
            catch (Exception)
            {
                throw new Exception("An error occurred while downloading the invoice.");
            }
        }



        [WebMethod]
        public static object ListMyOrders(string start, string end, int jtStartIndex, int jtPageSize,
            string jtSorting)
        {
            try
            {
                IQueryable<VwOrder> records = null;

                int count = 0;

                string username = HttpContext.Current.User.Identity.Name;

                records = new BlOrders().PagedList(username, start, end, 0, jtStartIndex, jtPageSize, jtSorting);
                count = records.Count();
                return new
                    {
                        Result = "OK",
                        Records = records,
                        TotalRecordCount = count
                    };
            }
            catch (Exception ex)
            {
                return new
                    {
                        Result = "ERROR",
                        ex.Message
                    };
            }
        }

        #endregion Public Methods

        #region Protected Methods

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                string username = HttpContext.Current.User.Identity.Name;
                var blo = new BlOrders();
                GetOrder = blo.GetLatestOrder(username);
                if (GetOrder != null)
                {
                    GetOrder.Tax = blo.GetTax(GetOrder.ID);
                    GetOrder.SubTotal = blo.GetSubTotal(GetOrder.ID);
                    decimal? grandTotal = blo.GetGrandTotal(GetOrder.SubTotal, GetOrder.Tax);
                    if (grandTotal != null)
                    {
                        GetOrder.GrandTotal = (decimal)grandTotal;
                    }
                }

                GetUser = new BlUsers().GetUser(username);
            }
            if (GetOrder != null)
            {
                ListViewOrder.DataSource = new BlOrders().GetItems(GetOrder.ID);
                ListViewOrder.DataBind();
            }
        }

        #endregion Protected Methods
    }
}