using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic;
using Common.EntityModel;
using Common.Views;

namespace DAL.Repositories.Orders
{
    //remove comments regex ///.*$

    public class OrderDetailsRepo : AppDbConnection, IDataRepository<VwOrderDetail>
    {
        #region Public Constructors

        public OrderDetailsRepo(bool isAdministrator)
            : base(isAdministrator)
        {
        }

        #endregion Public Constructors

        #region Public Methods

        public void Create(VwOrderDetail newObject)
        {
            Entity.OrderDetails.AddObject(new OrderDetail
            {
                Username = newObject.Username,
                OrderID = newObject.OrderID,
                ProductID = newObject.ProductID,
                UnitPrice = newObject.UnitPrice,
                Quantity = newObject.Quantity
            });
            Entity.SaveChanges();
        }

        public void Delete(VwOrderDetail objectToDelete)
        {
            OrderDetail t = GetOrderDetail(objectToDelete);
            Entity.OrderDetails.DeleteObject(t);
            Entity.SaveChanges();
        }

        public int GetCount()
        {
            return Entity.OrderDetails.Count();
        }

        public IQueryable<VwOrderDetail> ListAll()
        {
            return (from temp in Entity.OrderDetails
                    select new VwOrderDetail
                    {
                        Username = temp.Username,
                        OrderID = temp.OrderID,
                        ProductID = temp.ProductID,
                        TotalPrice = temp.UnitPrice * temp.Quantity,
                        Quantity = (int)temp.Quantity,
                        Discount = temp.Product.SpecialSale.Discount,
                        Item = temp.Product.Name,
                        Category = temp.Product.Category.Name,
                        Stock = temp.Product.Stock,
                        UnitPrice = temp.UnitPrice,
                        VAT = temp.Product.VAT
                    });
        }

        public IQueryable<VwOrderDetail> PagedList(int startIndex, int pageSize, string sorting)
        {
            IQueryable<VwOrderDetail> result = ListAll().OrderBy(sorting).Skip(startIndex).Take(pageSize);
            return result;
        }

        public VwOrderDetail Read(object key)
        {
            var vo = (VwOrderDetail)key;
            OrderDetail temp =
                Entity.OrderDetails.SingleOrDefault(
                    od => od.OrderID == vo.OrderID && od.ProductID == vo.ProductID && od.Username == vo.Username);

            if (temp != null)
            {

                return new VwOrderDetail
                {
                    Username = temp.Username,
                    OrderID = temp.OrderID,
                    ProductID = temp.ProductID,
                    TotalPrice = temp.UnitPrice * temp.Quantity,
                    Quantity = temp.Quantity,
                    Discount = temp.Product.SpecialSale.Discount,
                    Item = temp.Product.Name,
                    UnitPrice = temp.UnitPrice,
                    VAT = temp.Product.VAT
                };
            }

            return null;
        }

        public void Update(VwOrderDetail objectToUpdate)
        {
            OrderDetail t = GetOrderDetail(objectToUpdate);
            t.Quantity = objectToUpdate.Quantity;
            t.UnitPrice = GetUnitPrice(t);
            Entity.OrderDetails.ApplyCurrentValues(t);
            Entity.SaveChanges();
        }

        private decimal GetUnitPrice(OrderDetail orderDetail)
        {
            decimal result = 0;

            var singleOrDefault = orderDetail.Product.PriceTypes.SingleOrDefault(p => p.UserTypeID == orderDetail.User.TypeID);
            if (singleOrDefault != null)
            {
                result = singleOrDefault.UnitPrice;
            }

            return result;
        }

        private OrderDetail GetOrderDetail(VwOrderDetail objectToDelete)
        {
            OrderDetail result = Entity.OrderDetails.SingleOrDefault(
                o =>
                    o.OrderID == objectToDelete.OrderID && o.ProductID == objectToDelete.ProductID &&
                    o.Username == objectToDelete.Username);

            return result;
        }

        public VwOrderDetail GetOrderDetail(Guid orderId, Guid productId, string username)
        {
            VwOrderDetail result = (
                from d in Entity.OrderDetails
                select new VwOrderDetail
                {
                    Username = d.Username,
                    OrderID = d.OrderID,
                    ProductID = d.ProductID,
                    TotalPrice = d.UnitPrice * d.Quantity,
                    Quantity = (int)d.Quantity,
                    Item = d.Product.Name,
                    Category = d.Product.Category.Name,
                    Stock = d.Product.Stock,
                    UnitPrice = d.UnitPrice,
                    VAT = d.Product.VAT
                }
                ).SingleOrDefault(t => t.OrderID == orderId && t.ProductID == productId &&
                      t.Username == username);

            return result;
        }

        public IQueryable<VwOrderDetail> GetOrderDetails(Guid id)
        {
            IQueryable<VwOrderDetail> result = null;

            result = Entity.OrderDetails.Where(o => o.OrderID == id).Select(temp => new VwOrderDetail
            {
                Username = temp.Username,
                OrderID = temp.OrderID,
                ProductID = temp.ProductID,
                TotalPrice = temp.UnitPrice * temp.Quantity,
                Quantity = (int)temp.Quantity,
                Item = temp.Product.Name,
                Category = temp.Product.Category.Name,
                Stock = temp.Product.Stock,
                UnitPrice = temp.UnitPrice,
                VAT = temp.Product.VAT
            });

            return result;
        }

        #endregion Public Methods
    }
}