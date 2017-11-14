using System;
using System.Globalization;
using System.Linq;
using System.Net.Mail;
using System.Net.Mime;
using System.Text;
using System.Web;
using System.Web.Security;
using System.Web.Services;
using System.Web.UI;
using BLL.Accounts;
using BLL.Catalogue;
using BLL.Navigation;
using BLL.Orders;
using BLL.Sales;
using BLL.ShoppingCarts;
using Common.Utilities;
using Common.Views;

namespace UI.members.administrators
{
    public partial class index : Page
    {

        #region Public Methods

        [WebMethod]
        public static object AddressList(string Username)
        {
            object record = new BlAddresses().GetAddressDetails(Username);
            return new { Result = "OK", Records = record };
        }

        [WebMethod]
        public static object AddressTypeList(int jtStartIndex, int jtPageSize, string jtSorting)
        {
            int townCount = new BlAddressTypes().GetCount();
            IQueryable<VwAddressType> towns = new BlAddressTypes().PagedList(jtStartIndex, jtPageSize, jtSorting);
            return new { Result = "OK", Records = towns, TotalRecordCount = townCount };
        }

        [WebMethod]
        public static object AllocateRole(VwUserRoles record)
        {
            try
            {
                VwUserRoles newRecord = new BlRoles().AllocateRole(record.Username, record.RoleID);

                return new { Result = "OK", Record = newRecord };
            }
            catch (Exception ex)
            {
                return new { Result = "ERROR", ex.Message };
            }
        }

        [WebMethod]
        public static object CategoryList(int jtStartIndex, int jtPageSize, string jtSorting)
        {
            int categoryCount = new BlCategory().GetCount();
            IQueryable<VwCategory> Categorys = new BlCategory().PagedList(jtStartIndex, jtPageSize, jtSorting);
            return new { Result = "OK", Records = Categorys, TotalRecordCount = categoryCount };
        }

        [WebMethod]
        public static object ContactList(string Username)
        {
            IQueryable<VwContact> record = new BlContacts().GetContactDetails(Username);
            return new { Result = "OK", Records = record };
        }

        [WebMethod]
        public static object ContactTypeList(int jtStartIndex, int jtPageSize, string jtSorting)
        {
            int townCount = new BlContactTypes().GetCount();
            IQueryable<VwContactType> towns = new BlContactTypes().PagedList(jtStartIndex, jtPageSize, jtSorting);
            return new { Result = "OK", Records = towns, TotalRecordCount = townCount };
        }

        [WebMethod]
        public static object CreateAddressType(VwAddressType record)
        {
            VwAddressType newRecord = new BlAddressTypes().Create(record.Name);
            return new { Result = "OK", Record = newRecord };
        }

        [WebMethod]
        public static object CreateCategory(VwCategory record)
        {
            try
            {
                if (record.RootParent == -1 || record.ParentID == -1)
                {
                    record.RootParent = null;
                    record.ParentID = null;
                    record.ID = GenerateCategoryKey(true, null);
                }
                else
                {
                    record.ID = GenerateCategoryKey(false, record.RootParent);
                    record.ParentID = record.RootParent;
                }
                VwCategory newRecord = new BlCategory().Create(record);
                return new { Result = "OK", Record = newRecord };
            }
            catch (Exception ex)
            {
                return new { Result = "ERROR", ex.Message };
            }
        }

        [WebMethod]
        public static object CreateContactType(VwContactType record)
        {
            VwContactType newRecord = new BlContactTypes().Create(record.Name);
            return new { Result = "OK", Record = newRecord };
        }

        [WebMethod]
        public static object CreateGender(VwGender record)
        {
            try
            {
                VwGender newRecord = new BlGenders().Create(record.Name);
                return new { Result = "OK", Record = newRecord };
            }
            catch (Exception ex)
            {
                return new { Result = "ERROR", ex.Message };
            }
        }

        [WebMethod]
        public static object CreateMenu(VwMenu record)
        {
            try
            {
                if (record.RootParent == -1 || record.ParentID == -1)
                {
                    record.RootParent = null;
                    record.ParentID = null;
                    record.ID = GenerateMenuKey(true, null);
                }
                else
                {
                    record.ID = GenerateMenuKey(false, record.RootParent);
                    record.ParentID = record.RootParent;
                }

                VwMenu newRecord = new BlMenus().Create(record);
                return new { Result = "OK", Record = newRecord };
            }
            catch (Exception ex)
            {
                return new { Result = "ERROR", ex.Message };
            }
        }

        [WebMethod]
        public static object CreateMenuRole(VwMenuRole record)
        {
            try
            {
                VwMenuRole newRecord = new BlMenus().CreateMenuRole(record);
                return new { Result = "OK", Record = newRecord };
            }
            catch (Exception ex)
            {
                return new { Result = "ERROR", ex.Message };
            }
        }

        [WebMethod]
        public static object CreateOrderState(VwOrderState record)
        {
            VwOrderState newRecord = new BlOrderStates().Create(record.Name);
            return new { Result = "OK", Record = newRecord };
        }

        [WebMethod]
        public static object CreatePriceType(VwPriceType record)
        {
            VwPriceType newRecord = new BlPriceTypes().Create(record);
            return new { Result = "OK", Record = newRecord };
        }

        [WebMethod]
        public static object CreateProduct(VwProduct record)
        {
            try
            {
                record.Image = (string)HttpContext.Current.Session["CurrentImagePath"];
                if (string.IsNullOrEmpty(record.Image))
                {
                    record.Image = "/img/catalogue/product0.jpg";
                }
                VwProduct newRecord = new BlProducts().Create(record);
                return new { Result = "OK", Record = newRecord };
            }
            catch (Exception ex)
            {
                return new { Result = "ERROR", ex.Message };
            }
        }

        [WebMethod]
        public static object CreateRole(VwRole record)
        {
            try
            {
                VwRole newRecord = new BlRoles().Create(record.Name);
                return new { Result = "OK", Record = newRecord };
            }
            catch (Exception ex)
            {
                return new { Result = "ERROR", ex.Message };
            }
        }

        [WebMethod]
        public static object CreateSale(VwSpecialSale record)
        {
            try
            {
                VwSpecialSale newRecord = new BlSpecialSales().Create(record);
                return new { Result = "OK", Record = newRecord };
            }
            catch (Exception ex)
            {
                return new { Result = "ERROR", ex.Message };
            }
        }

        [WebMethod]
        public static object CreateTown(VwTown record)
        {
            try
            {
                VwTown newRecord = new BlTowns().Create(record.Name);
                return new { Result = "OK", Record = newRecord };
            }
            catch (Exception ex)
            {
                return new { Result = "ERROR", ex.Message };
            }
        }

        [WebMethod]
        public static object CreateUserType(VwUserType record)
        {
            VwUserType newRecord = new BlUserTypes().Create(record.Name);
            return new { Result = "OK", Record = newRecord };
        }

        [WebMethod]
        public static object DeAllocateRole(string Username, int RoleID)
        {
            try
            {
                new BlRoles().DeallocateRole(Username, RoleID);
                return new { Result = "OK" };
            }
            catch (Exception ex)
            {
                return new { Result = "ERROR", ex.Message };
            }
        }

        [WebMethod]
        public static object DeleteAddressType(int ID)
        {
            new BlAddressTypes().Delete(ID);
            return new { Result = "OK" };
        }

        [WebMethod]
        public static object DeleteCategory(int ID)
        {
            new BlCategory().Delete(ID);
            return new { Result = "OK" };
        }

        [WebMethod]
        public static object DeleteContactType(int ID)
        {
            new BlContactTypes().Delete(ID);
            return new { Result = "OK" };
        }

        [WebMethod]
        public static object DeleteGender(int ID)
        {
            new BlGenders().Delete(ID);
            return new { Result = "OK" };
        }

        [WebMethod]
        public static object DeleteMenu(int ID)
        {
            try
            {
                new BlMenus().Delete(ID);
                return new { Result = "OK" };
            }
            catch (Exception ex)
            {
                return new { Result = "ERROR", ex.Message };
            }
        }

        [WebMethod]
        public static object DeleteMenuRole(int menuId, int roleId)
        {
            try
            {
                new BlMenus().DeleteMenuRole(menuId, roleId);
                return new { Result = "OK" };
            }
            catch (Exception ex)
            {
                return new { Result = "ERROR", ex.Message };
            }
        }

        [WebMethod]
        public static object DeleteOrder(Guid ID)
        {
            try
            {
                new BlOrders().Delete(ID);
                return new { Result = "OK" };
            }
            catch (Exception ex)
            {
                return new { Result = "ERROR", ex.Message };
            }
        }

        [WebMethod]
        public static object DeleteOrderDetail(Guid ProductID, string Username, Guid OrderID)
        {
            try
            {
                new BlOrderDetails().Delete(OrderID, ProductID, Username);
                return new { Result = "OK" };
            }
            catch (Exception ex)
            {
                return new { Result = "ERROR", ex.Message };
            }
        }

        [WebMethod]
        public static object DeleteOrderState(int ID)
        {
            try
            {
                new BlOrderStates().Delete(ID);
                return new { Result = "OK" };
            }
            catch (Exception ex)
            {
                return new { Result = "ERROR", ex.Message };
            }
        }

        [WebMethod]
        public static object DeletePriceType(Guid ProductID, int UserType)
        {
            try
            {
                new BlPriceTypes().Delete(ProductID, UserType);
                return new { Result = "OK" };
            }
            catch (Exception ex)
            {
                return new { Result = "ERROR", ex.Message };
            }
        }

        [WebMethod]
        public static object DeleteProduct(Guid ID)
        {
            try
            {
                new BlProducts().Delete(ID);
                return new { Result = "OK" };
            }
            catch (Exception ex)
            {
                return new { Result = "ERROR", ex.Message };
            }
        }

        [WebMethod]
        public static object DeleteRole(int ID)
        {
            try
            {
                new BlRoles().Delete(ID);
                return new { Result = "OK" };
            }
            catch (Exception ex)
            {
                return new { Result = "ERROR", ex.Message };
            }
        }

        [WebMethod]
        public static object DeleteSale(Guid ID)
        {
            try
            {
                new BlSpecialSales().Delete(ID);
                return new { Result = "OK" };
            }
            catch (Exception ex)
            {
                return new { Result = "ERROR", ex.Message };
            }
        }

        [WebMethod]
        public static object DeleteTown(int ID)
        {
            new BlTowns().Delete(ID);
            return new { Result = "OK" };
        }

        [WebMethod]
        public static object DeleteUser(VwUser record)
        {
            try
            {
                new BlUsers().Delete(record);
                return new { Result = "OK" };
            }
            catch (Exception ex)
            {
                return new { Result = "ERROR", ex.Message };
            }
        }

        [WebMethod]
        public static object DeleteUserType(int ID)
        {
            new BlUserTypes().Delete(ID);
            return new { Result = "OK" };
        }

        [WebMethod]
        public static IQueryable<string> FindUsername(string query)
        {
            IQueryable<string> result = new BlUsers().GetUsernames(query);
            return result;
        }

        [WebMethod]
        public static object GenderList(int jtStartIndex, int jtPageSize, string jtSorting)
        {
            int townCount = new BlGenders().GetCount();
            IQueryable<VwGender> towns = new BlGenders().PagedList(jtStartIndex, jtPageSize, jtSorting);
            return new { Result = "OK", Records = towns, TotalRecordCount = townCount };
        }

        [WebMethod]
        public static object GetAddressTypeOptions()
        {
            try
            {
                var records = new BlAddressTypes().ListAll().Select(c => new { DisplayText = c.Name, Value = c.ID });
                return new { Result = "OK", Options = records };
            }
            catch (Exception ex)
            {
                return new { Result = "ERROR", ex.Message };
            }
        }

        [WebMethod]
        public static object GetCategoryOptions()
        {
            try
            {
                var records = new BlCategory().ListAll().Select(c => new { DisplayText = c.Name, Value = c.ID });
                return new { Result = "OK", Options = records };
            }
            catch (Exception ex)
            {
                return new { Result = "ERROR", ex.Message };
            }
        }

        [WebMethod]
        public static object GetContactTypeOptions()
        {
            try
            {
                var records = new BlContactTypes().ListAll().Select(c => new { DisplayText = c.Name, Value = c.ID });
                return new { Result = "OK", Options = records };
            }
            catch (Exception ex)
            {
                return new { Result = "ERROR", ex.Message };
            }
        }

        [WebMethod]
        public static object GetGenders()
        {
            try
            {
                var genders = new BlGenders().ListAll().Select(c => new { DisplayText = c.Name, Value = c.ID });
                return new { Result = "OK", Options = genders };
            }
            catch (Exception ex)
            {
                return new { Result = "ERROR", ex.Message };
            }
        }

        [WebMethod]
        public static object GetMenuRoles(int menuId)
        {
            try
            {
                IQueryable<VwMenuRole> record = new BlMenus().GetMenuRoles(menuId);
                return new { Result = "OK", Records = record };
            }
            catch (Exception ex)
            {
                return new { Result = "ERROR", ex.Message };
            }
        }

        [WebMethod]
        public static object GetOrderDetails(Guid id)
        {
            try
            {
                IQueryable<VwOrderDetail> record = new BlOrderDetails().GetOrderDetails(id);
                return new { Result = "OK", Records = record };
            }
            catch (Exception ex)
            {
                return new { Result = "ERROR", ex.Message };
            }
        }

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
        public static object GetPriceTypes(Guid ProductID)
        {
            try
            {
                IQueryable<VwPriceType> record = new BlPriceTypes().GetPriceTypeDetails(ProductID);
                return new { Result = "OK", Records = record };
            }
            catch (Exception ex)
            {
                return new { Result = "ERROR", ex.Message };
            }
        }

        [WebMethod]
        public static object GetRoleOptions()
        {
            try
            {
                var records = new BlRoles().ListAll().Select(c => new { DisplayText = c.Name, Value = c.ID });
                return new { Result = "OK", Options = records };
            }
            catch (Exception ex)
            {
                return new { Result = "ERROR", ex.Message };
            }
        }

        [WebMethod]
        public static object GetRoles(string Username)
        {
            try
            {
                IQueryable<VwUserRoles> record = new BlRoles().GetRoleDetails(Username);
                return new { Result = "OK", Records = record };
            }
            catch (Exception ex)
            {
                return new { Result = "ERROR", ex.Message };
            }
        }

        [WebMethod]
        public static object GetRootCategoryOptions()
        {
            try
            {
                var records = new BlCategory().ListRootCategories()
                    .Select(c => new { DisplayText = c.Name, Value = c.ID });

                return new { Result = "OK", Options = records };
            }
            catch (Exception ex)
            {
                return new { Result = "ERROR", ex.Message };
            }
        }

        [WebMethod]
        public static object GetRootMenuOptions()
        {
            try
            {
                var records = new BlMenus().ListRootMenus().Select(c => new { DisplayText = c.Name, Value = c.ID });

                return new { Result = "OK", Options = records };
            }
            catch (Exception ex)
            {
                return new { Result = "ERROR", ex.Message };
            }
        }

        [WebMethod]
        public static object GetSalesOptions()
        {
            try
            {
                var records = new BlSpecialSales().ListAll().Select(c => new { DisplayText = c.Name, Value = c.ID });
                return new { Result = "OK", Options = records };
            }
            catch (Exception ex)
            {
                return new { Result = "ERROR", ex.Message };
            }
        }

        [WebMethod]
        public static object GetSubCategoryOptions(int? parentID)
        {
            try
            {
                var records =
                    new BlCategory().ListSubCategories(parentID).Select(c => new { DisplayText = c.Name, Value = c.ID });
                return new { Result = "OK", Options = records };
            }
            catch (Exception ex)
            {
                return new { Result = "ERROR", ex.Message };
            }
        }
        [WebMethod]
        public static object GetSubMenuOptions(int? parentID)
        {
            try
            {
                var records = new BlMenus().ListSubMenus(parentID).Select(c => new { DisplayText = c.Name, Value = c.ID });

                return new { Result = "OK", Options = records };
            }
            catch (Exception ex)
            {
                return new { Result = "ERROR", ex.Message };
            }
        }

        [WebMethod]
        public static object GetTownOptions()
        {
            try
            {
                var records = new BlTowns().ListAll().Select(c => new { DisplayText = c.Name, Value = c.ID });
                return new { Result = "OK", Options = records };
            }
            catch (Exception ex)
            {
                return new { Result = "ERROR", ex.Message };
            }
        }

        [WebMethod]
        public static object GetUserTypes()
        {
            try
            {
                var userType = new BlUserTypes().ListAll().Select(c => new { DisplayText = c.Name, Value = c.ID });
                return new { Result = "OK", Options = userType };
            }
            catch (Exception ex)
            {
                return new { Result = "ERROR", ex.Message };
            }
        }

        [WebMethod]
        public static object ListOrderByFiter(string query, string start, string end, int state, int jtStartIndex,
            int jtPageSize,
            string jtSorting)
        {
            try
            {
                IQueryable<VwOrder> records = null;
                int count = 0;
                records = new BlOrders().PagedList(query, start, end, state, jtStartIndex, jtPageSize, jtSorting);
                count = records.Count();

                return new { Result = "OK", Records = records, TotalRecordCount = count };
            }
            catch (Exception ex)
            {
                return new { Result = "ERROR", ex.Message };
            }
        }

        [WebMethod]
        public static object ListProductByFilter(string query, int jtStartIndex, int jtPageSize, string jtSorting)
        {
            try
            {
                int productCount = new BlProducts().GetCount();
                IQueryable<VwProduct> products = new BlProducts().PagedList(query, jtStartIndex, jtPageSize, jtSorting);
                return new { Result = "OK", Records = products, TotalRecordCount = productCount };
            }
            catch (Exception ex)
            {
                return new { Result = "ERROR", ex.Message };
            }
        }

        [WebMethod]
        public static object ListSaleByFilter(string query, string start, string end, int jtStartIndex, int jtPageSize,
            string jtSorting)
        {
            try
            {
                IQueryable<VwSpecialSale> records = null;
                int count = 0;
                records = new BlSpecialSales().PagedList(query, start, end, jtStartIndex, jtPageSize, jtSorting);
                count = records.Count();

                return new { Result = "OK", Records = records, TotalRecordCount = count };
            }
            catch (Exception ex)
            {
                return new { Result = "ERROR", ex.Message };
            }
        }

        [WebMethod]
        public static object ListUserByFilter(string username, int jtStartIndex, int jtPageSize, string jtSorting)
        {
            try
            {
                IQueryable<VwUser> records = null;
                int count = 0;

                count = new BlUsers().GetCountByFilter(username);
                records = new BlUsers().PagedList(username, jtStartIndex, jtPageSize, jtSorting);

                return new { Result = "OK", Records = records, TotalRecordCount = count };
            }
            catch (Exception ex)
            {
                return new { Result = "ERROR", ex.Message };
            }
        }

        [WebMethod]
        public static object MenuList(int jtStartIndex, int jtPageSize, string jtSorting)
        {
            try
            {
                int MenuCount = new BlMenus().GetCount();
                IQueryable<VwMenu> Menus = new BlMenus().PagedList(jtStartIndex, jtPageSize, jtSorting);
                return new { Result = "OK", Records = Menus, TotalRecordCount = MenuCount };
            }
            catch (Exception ex)
            {
                return new { Result = "ERROR", ex.Message };
            }
        }

        [WebMethod]
        public static object OrderStateList(int jtStartIndex, int jtPageSize, string jtSorting)
        {
            int OrderStateCount = new BlOrderStates().GetCount();
            IQueryable<VwOrderState> orderStates = new BlOrderStates().PagedList(jtStartIndex, jtPageSize, jtSorting);
            return new { Result = "OK", Records = orderStates, TotalRecordCount = OrderStateCount };
        }

        [WebMethod]
        public static object RoleList(int jtStartIndex, int jtPageSize, string jtSorting)
        {
            int townCount = new BlRoles().GetCount();
            IQueryable<VwRole> towns = new BlRoles().PagedList(jtStartIndex, jtPageSize, jtSorting);
            return new { Result = "OK", Records = towns, TotalRecordCount = townCount };
        }

        [WebMethod]
        public static object TownList(int jtStartIndex, int jtPageSize, string jtSorting)
        {
            int townCount = new BlTowns().GetCount();
            IQueryable<VwTown> towns = new BlTowns().PagedList(jtStartIndex, jtPageSize, jtSorting);
            return new { Result = "OK", Records = towns, TotalRecordCount = townCount };
        }

        [WebMethod]
        public static object UpdateAddressType(VwAddressType record)
        {
            new BlAddressTypes().Update(record);
            return new { Result = "OK" };
        }

        [WebMethod]
        public static object UpdateCategory(VwCategory record)
        {
            try
            {
                if (record.RootParent == -1)
                {
                    record.RootParent = null;
                    record.ParentID = null;
                }
                else
                {
                    record.ParentID = record.RootParent;
                }
                record = new BlCategory().Update(record);

                return new { Result = "OK", record };
            }
            catch (Exception ex)
            {
                return new { Result = "ERROR", ex.Message };
            }
        }

        [WebMethod]
        public static object UpdateContactType(VwContactType record)
        {
            new BlContactTypes().Update(record);
            return new { Result = "OK" };
        }

        [WebMethod]
        public static object UpdateGender(VwGender record)
        {
            try
            {
                new BlGenders().Update(record);
                return new { Result = "OK", record };
            }
            catch (Exception ex)
            {
                return new { Result = "ERROR", ex.Message };
            }
        }

        [WebMethod]
        public static object UpdateMenu(VwMenu record)
        {
            try
            {
                if (record.RootParent == -1)
                {
                    record.RootParent = null;
                    record.ParentID = null;
                }
                else
                {
                    record.ParentID = record.RootParent;
                }

                record = new BlMenus().Update(record);
                return new { Result = "OK", record };
            }
            catch (Exception ex)
            {
                return new { Result = "ERROR", ex.Message };
            }
        }

        [WebMethod]
        public static object UpdateOrder(VwOrder record)
        {
            try
            {
                var blo = new BlOrders();
                VwOrder order = blo.Update(record);
                VwUser user = new BlUsers().GetUser(order.Username);
                SendStatusUpdate(user, order);
                return new { Result = "OK" };
            }
            catch (Exception ex)
            {
                return new { Result = "ERROR", ex.Message };
            }
        }

        [WebMethod]
        public static object UpdateOrderDetail(VwOrderDetail record)
        {
            try
            {
                //record.Stock = new BlProducts().Read(record.ProductID).Stock;
                record = new BlOrderDetails().Update(record);

                var order = new BlOrders().Read(record.OrderID);
                var user = new BlUsers().GetUser(record.Username);
                var odList = new BlOrders().GetItems(record.OrderID);

                var html = new StringBuilder();
                html.Append("<table width='650' cellspacing='0' cellpadding='0' border='0' style='border:1px solid #eaeaea'>");
                html.Append("<thead><tr><th bgcolor='#EAEAEA' align='left' style='font-size:13px;padding:3px 9px'>Item</th>" +
                            "<th bgcolor='#EAEAEA' align='left' style='font-size:13px;padding:3px 9px'>Unit Price</th>" +
                            "<th bgcolor='#EAEAEA' align='center' style='font-size:13px;padding:3px 9px'>Quantity</th>" +
                            "<th bgcolor='#EAEAEA' align='right' style='font-size:13px;padding:3px 9px'>Total</th></tr></thead>");
                foreach (VwOrderDetail item in odList)
                {
                    html.Append("<tbody bgcolor='#F6F6F6'><tr>");
                    html.Append("<td valign='top' align='left' style='font-size:11px;padding:3px 9px;border-bottom:1px dotted #cccccc'>");
                    html.Append("<strong style='font-size:11px'>" + item.Item + "</strong></td>");
                    html.Append("<td valign='top' align='left' style='font-size:11px;padding:3px 9px;border-bottom:1px dotted #cccccc'>");
                    html.Append(string.Format(new CultureInfo("MT"), "{0:C}", item.UnitPrice) + "</td>");
                    html.Append("<td valign='top' align='center' style='font-size:11px;padding:3px 9px;border-bottom:1px dotted #cccccc'>");
                    html.Append(item.Quantity + "</td>");
                    html.Append("<td valign='top' align='right' style='font-size:11px;padding:3px 9px;border-bottom:1px dotted #cccccc'>");
                    html.Append("<span>Excl. VAT:</span>");
                    html.Append("<span>" + string.Format(new CultureInfo("MT"), "{0:C}", item.TotalPrice) + "</span></td>");
                    html.Append("</tr></tbody>");
                }
                html.Append("<tbody><tr><td colspan='4' style='background:#eaeaea;text-align:center;padding:5px;' >&nbsp;</td></tr></tbody>");
                html.Append("<tbody><tr><td align='right' style='padding:3px 9px' colspan='3'>Subtotal (Excl.VAT)</td>");
                html.Append("<td align='right' style='padding:3px 9px'><span>" + string.Format(new CultureInfo("MT"), "{0:C}", order.SubTotal) + "</span></td></tr>");
                html.Append("<tr><td align='right' style='padding:3px 9px' colspan='3'>Delivery (Excl.VAT)</td>");
                html.Append("<td align='right' style='padding:3px 9px'><span>" + string.Format(new CultureInfo("MT"), "{0:C}", 0) + "</span></td></tr>");
                html.Append("<tr><td align='right' style='padding:3px 9px' colspan='3'>Discount </td>");
                html.Append("<td align='right' style='padding:3px 9px'><span>" + string.Format(new CultureInfo("MT"), "{0:C}", 0) + "</span></td></tr>");
                html.Append("<tr><td align='right' style='padding:3px 9px' colspan='3'> VAT</td>");
                html.Append("<td align='right' style='padding:3px 9px'><span>" + string.Format(new CultureInfo("MT"), "{0:C}", order.Tax) + "</span></td></tr>");
                html.Append("<tr><td align='right' style='padding:3px 9px' colspan='3'><strong>Grand Total (Incl.VAT)</strong></td>");
                html.Append("<td align='right' style='padding:3px 9px'><strong><span>" + string.Format(new CultureInfo("MT"), "{0:C}", order.GrandTotal) + "</span></strong></td></tr>");
                html.Append("</tbody></table>");


                SendOrderDetailUpdate(user, order, html);
                return new { Result = "OK", Record = record };
            }
            catch (Exception ex)
            {
                return new { Result = "ERROR", ex.Message };
            }
        }

        [WebMethod]
        public static object UpdateOrderState(VwOrderState record)
        {
            try
            {
                new BlOrderStates().Update(record);
                return new { Result = "OK" };
            }
            catch (Exception ex)
            {
                return new { Result = "ERROR", ex.Message };
            }
        }

        [WebMethod]
        public static object UpdatePriceType(VwPriceType record)
        {
            try
            {
                new BlPriceTypes().Update(record);
                return new { Result = "OK", Record = record };
            }
            catch (Exception ex)
            {
                return new { Result = "ERROR", ex.Message };
            }
        }

        [WebMethod]
        public static object UpdateProduct(VwProduct record)
        {
            try
            {
                record.Image = (string)HttpContext.Current.Session["CurrentImagePath"];
                if (string.IsNullOrEmpty(record.Image))
                {
                    record.Image = "/img/catalogue/product0.jpg";
                }
                new BlProducts().Update(record);
                return new { Result = "OK", Record = record };
            }
            catch (Exception ex)
            {
                return new { Result = "ERROR", ex.Message };
            }
        }

        [WebMethod]
        public static object UpdateRole(VwRole record)
        {
            try
            {
                new BlRoles().Update(record);
                return new { Result = "OK", Record = record };
            }
            catch (Exception ex)
            {
                return new { Result = "ERROR", ex.Message };
            }
        }

        [WebMethod]
        public static object UpdateSale(VwSpecialSale record)
        {
            try
            {
                new BlSpecialSales().Update(record);
                return new { Result = "OK", Record = record };
            }
            catch (Exception ex)
            {
                return new { Result = "ERROR", ex.Message };
            }
        }

        [WebMethod]
        public static object UpdateTown(VwTown record)
        {
            try
            {
                new BlTowns().Update(record);
                return new { Result = "OK", Record = record };
            }
            catch (Exception ex)
            {
                return new { Result = "ERROR", ex.Message };
            }
        }

        [WebMethod]
        public static object UpdateUser(VwUser record)
        {
            try
            {
                string hashedPassword = FormsAuthentication.HashPasswordForStoringInConfigFile(
                    record.Password, "SHA1");
                string encPassword = Encryption.EncryptTripleDES(hashedPassword);
                record.Password = encPassword;
                new BlUsers().Update(record);
                return new { Result = "OK" };
            }
            catch (Exception ex)
            {
                return new { Result = "ERROR", ex.Message };
            }
        }

        [WebMethod]
        public static object UpdateUserType(VwUserType record)
        {
            new BlUserTypes().Update(record);
            return new { Result = "OK" };
        }

        [WebMethod]
        public static object UserTypeList(int jtStartIndex, int jtPageSize, string jtSorting)
        {
            int townCount = new BlUserTypes().GetCount();
            IQueryable<VwUserType> towns = new BlUserTypes().PagedList(jtStartIndex, jtPageSize, jtSorting);
            return new { Result = "OK", Records = towns, TotalRecordCount = townCount };
        }

        #endregion Public Methods

        #region Protected Methods

        protected void Page_Load(object sender, EventArgs e)
        {
            //Deadline 5th of january 2015
            //client-side
            //javascript and vbscipt

            //server-side
            //asp.net
            //php

            //Secure login
            //role based
            //folder based web.config
            //hash paswords in the database
            //login attempts
        }

        #endregion Protected Methods

        #region Private Methods

        private static int GenerateCategoryKey(bool isRoot, int? parentId)
        {
            int id = 0;

            if (isRoot)
            {
                id = new BlCategory().GetLastRootID() + 1000;
            }
            else
            {
                int temp = new BlCategory().GetLastChildID(parentId);
                if (temp == 0)
                {
                    int? i = parentId + 1;
                    if (i != null) id = (int)i;
                }
                else
                {
                    id = temp + 1;
                }
            }

            return id;
        }

        private static int GenerateMenuKey(bool isRoot, int? parentId)
        {
            int id = 0;

            if (isRoot)
            {
                id = new BlMenus().GetLastRootID() + 1000;
            }
            else
            {
                int temp = new BlMenus().GetLastChildID(parentId);
                if (temp == 0)
                {
                    int? i = parentId + 1;
                    if (i != null) id = (int)i;
                }
                else
                {
                    id = temp + 1;
                }
            }

            return id;
        }

        private static void SendStatusUpdate(VwUser user, VwOrder order)
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

            html.Append(
                "<tr><h3 style='Margin-top:0;color:#2e3b4e;font-size:16px;Margin-bottom:16px;font-family:Cabin,Avenir,sans-serif!important;line-height:24px'>Hello, " +
                user.FirstName + " " + user.LastName + "</h3>");
            html.Append("<h2 style='font-size:15px;font-weight:600;margin:0'>");
            html.Append("Your Order code " + order.ID + " <em> <br/> (placed on " + order.DatePlaced.ToLongDateString() +
                        ")</em></h2><br/>");

            if (order.Status == "Processing" || order.Status == "Pending")
            {
                html.Append("<h2>Order Status: " + order.Status
                            +
                            "</h2><p>Order Confirmation. This email confirms that we have received your order. We recommend that you keep this email for your records.</p></tr>");
            }
            else if (order.Status == "Delivered" || order.Status == "Picked up")
            {
                html.Append("<h2>Order Status: " + order.Status
                            +
                            "</h2>p>Delivery Confirmation. This email confirms that your order has been delivered or picked up. T</p></tr>");
            }
            else if (order.Status == "Awaiting Delivery" || order.Status == "Awaiting Pickup")
            {
                html.Append("<h2>Order Status: " + order.Status
                            +
                            "</h2><p>Delivery Service Confirmation. This email confirms stock is available and item(s) are being delivered from our warehouse to your location.o inquire about arrival time of your item(s) contact <em>Gozo Express<em> on (+356) 212121212.</p></tr>");
            }
            else if (order.Status == "Cancelled")
            {
                html.Append("<h2>Order Status: " + order.Status
                            +
                            "</h2><p>Order Cancelled. Store Pickup orders will automatically be cancelled if not picked up within the designated period.</p></tr>");
            }
            else if (order.Status == "Returned")
            {
                html.Append("<h2>Order Status: " + order.Status
                            + "</h2>Order has been cancelled or item(s) returned.</tr>");
            }
            else if (order.Status == "Complete")
            {
                html.Append("<h2>Order Status: " + order.Status
                            + "</h2><p>Your Order has been completed.</p></tr>");
            }
            else
            {
                html.Append("<h2>Order Status: " + order.Status + "</h2>" +
                            "<p>Your order has been received and is being processed</p></tr>");
            }

            html.Append(
                "<br/><div style='background:#eaeaea;text-align:center;padding:5px;'><p style='font-size:12px;margin:0; text-align:center;'>Thank you, <strong>DIY-Malta</strong></p></div>");
            html.Append("<tr><h3>You can check the status of your order by:</h3><ul>");
            html.Append(
                "<li><b>Checking your email:</b> When the status of your order chages we will send you an email like this one.</li>");
            html.Append(
                "<li><b>Visiting your dashboard in the diymalta.com website:</b>You will be able to view your order history and the status of your most recent order.</li><li><b>Contacting us:</b> If you still have questions about your order, please <a href='#'>contact us</a>.</li></ul>");
            html.Append(
                "</tr><p>This is an automatic e-mail message generated by the DIY-Malta system. Please do not reply to this e-mail.</p>");

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

            new Communication().SendEmail(user.Email, "DIY-Malta Order Status Update", avHtml);
        }


        private static void SendOrderDetailUpdate(VwUser user, VwOrder order, StringBuilder orderHtml)
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
            html.Append("<a target='_blank' style='color:#1e7ec8' href='#'> logging into your account</a>. If you have any questions about your order please contact us on ");
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
                "<li><b>Checking your email:</b> When the status of your order chages we will send you an email like this one.</li>");
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

            new Communication().SendEmail(user.Email, "DIY-Malta Order Changed", avHtml);
        }


        #endregion Private Methods

        /*

         remove comments regex ///.*$
         */
    }
}