using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic;
using Common.EntityModel;
using Common.Views;

namespace DAL.Repositories
{
    public class OrdersRepo : AppDbConnection, IDataRepository<VwOrder>
    {

        public OrdersRepo(bool isAdministrator)
            : base(isAdministrator)
        {
        }

        public void Create(VwOrder newObject)
        {
            Entity.Orders.AddObject(new Order
            {
                ID = newObject.ID,
                DatePlaced = newObject.DatePlaced,
                Status = newObject.State
            });
            Entity.SaveChanges();
        }


        public void Delete(VwOrder objectToDelete)
        {
            Order t = GetOrder(objectToDelete.ID);
            Entity.Orders.DeleteObject(t);
            Entity.SaveChanges();
        }

        public IQueryable<VwOrder> Filter(string query)
        {
            IQueryable<VwOrder> result;

            result = (from t in Entity.Orders
                      from od in t.OrderDetails
                      where od.Username.Equals(query)
                      select new VwOrder
                      {
                          ID = t.ID,
                          DatePlaced = t.DatePlaced,
                          State = t.Status,
                          Username = od.Username
                      });

            return result.Distinct();
        }

        public IQueryable<VwOrder> Filter(int state)
        {
            IQueryable<VwOrder> result = (from t in Entity.Orders
                from od in t.OrderDetails
                where t.Status == state
                select new VwOrder
                {
                    ID = t.ID,
                    DatePlaced = t.DatePlaced,
                    State = t.Status,
                    Username = od.Username
                });

            return result.Distinct();
        }

        public IQueryable<VwOrder> Filter(DateTime startDate, DateTime endDate)
        {
            IEnumerable<VwOrder> result;
            IEnumerable<VwOrder> temp = ListAll();
            result = temp.Where(s =>s.DatePlaced.Date >= startDate.Date && (startDate.Date <= endDate.Date) &&
                    s.DatePlaced.Date <= endDate.Date).Distinct();

            return result.AsQueryable();
        }

        public IQueryable<VwOrder> Filter(string query, int state, DateTime startDate, DateTime endDate)
        {
            IEnumerable<VwOrder> result;
            IEnumerable<VwOrder> temp = null;

            if (!string.IsNullOrEmpty(query) && state == 0)
            {
                temp = Filter(query).AsEnumerable();
            }
            else if (string.IsNullOrEmpty(query) && state > 0)
            {
                temp = ListAll().Where(s => s.State == state).AsEnumerable();
            }
            else if (string.IsNullOrEmpty(query) && state == 0)
            {
                temp = ListAll().AsEnumerable();
            }
            else if (state > 0)
            {
                temp = Filter(query);
                temp = temp.Where(s => s.State == state).AsEnumerable();
            }
            else
            {
                temp = ShowOrphanRecords().AsEnumerable();
            }

            result = temp.Where(
                s =>
                    s.DatePlaced.Date >= startDate.Date && (startDate.Date <= endDate.Date) &&
                    s.DatePlaced.Date <= endDate.Date);


            return result.AsQueryable();
        }

        public int GetCount()
        {
            return Entity.Orders.Count();
        }

        public IQueryable<VwOrderDetail> GetItems(Guid id)
        {
            IQueryable<OrderDetail> result = Entity.OrderDetails.Where(o => o.OrderID == id);
            return result.Select(d => new VwOrderDetail
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
            });
        }

        public VwOrder GetLatestOrder(string username)
        {
            VwOrder result = null;
            Order temp = (from o in Entity.Orders
                          from od in o.OrderDetails
                          where od.Username == username
                          select o).OrderByDescending(d => d.DatePlaced).FirstOrDefault();
            if (temp != null)
            {
                result = new VwOrder
                {
                    ID = temp.ID,
                    DatePlaced = temp.DatePlaced,
                    State = temp.Status,
                    Status = temp.OrderState.Name
                };
            }
            return result;
        }

        public Order GetOrder(Guid id)
        {
            return Entity.Orders.SingleOrDefault(e => e.ID == id);
        }

        public IQueryable<VwOrder> ListAll()
        {
            return (from t in Entity.Orders
                from od in t.OrderDetails
                select new VwOrder
                {
                    ID = t.ID,
                    DatePlaced = t.DatePlaced,
                    State = t.Status,
                    Username = od.Username
                }).Distinct();
        }

        public IQueryable<VwOrder> PagedList(int startIndex, int pageSize, string sorting)
        {
            IQueryable<VwOrder> result = ListAll().OrderBy(sorting).Skip(startIndex).Take(pageSize);
            return result;
        }

        public IQueryable<VwOrder> PagedList(string query, int state, int startIndex, int pageSize, string sorting)
        {
            IQueryable<VwOrder> result;
            IQueryable<VwOrder> temp = null;
            if (string.IsNullOrEmpty(query) && state == 0)
            {
                temp = ShowOrphanRecords();
            }
            else if (string.IsNullOrEmpty(query) && state > 0)
            {
                temp = Filter(state);
            }
            else if (state > 0)
            {
                temp = Filter(query);
                temp = temp.Where(s => s.State == state);
            }
            else
            {
                temp = Filter(query);
            }
            result = temp.OrderBy(sorting).Skip(startIndex).Take(pageSize);

            return result;
        }

        public IQueryable<VwOrder> PagedList(string query, int state, DateTime startDate, DateTime endDate,
            int startIndex, int pageSize, string sorting)
        {
            IQueryable<VwOrder> result = null;

            IQueryable<VwOrder> temp = Filter(query, state, startDate, endDate);

            if (temp != null)
            {
                result = temp.OrderBy(sorting).Skip(startIndex).Take(pageSize);
            }
            return result;
        }

        public IQueryable<VwOrder> PagedList(string query, int startIndex, int pageSize, string sorting)
        {
            IQueryable<VwOrder> result = null;

            IQueryable<VwOrder> temp = Filter(query);

            if (temp != null)
            {
                result = temp.OrderBy(sorting).Skip(startIndex).Take(pageSize);
            }
            return result;
        }

        public IQueryable<VwOrder> PagedList(int state, int startIndex, int pageSize, string sorting)
        {
            IQueryable<VwOrder> result = null;

            IQueryable<VwOrder> temp = Filter(state);

            if (temp != null)
            {
                result = temp.OrderBy(sorting).Skip(startIndex).Take(pageSize);
            }
            return result;
        }

        public IQueryable<VwOrder> PagedList(DateTime startDate, DateTime endDate, int startIndex, int pageSize, string sorting)
        {
            IQueryable<VwOrder> result = null;

            IQueryable<VwOrder> temp = Filter(startDate, endDate);

            if (temp != null)
            {
                result = temp.OrderBy(sorting).Skip(startIndex).Take(pageSize);
            }
            return result;
        }

        public VwOrder Read(object key)
        {
            var result = new VwOrder();
            var id = (Guid) key;
            Order order = GetOrder(id);
            result.ID = order.ID;
            result.DatePlaced = order.DatePlaced;
            result.State = order.Status;
            result.Status = order.OrderState.Name;

            var firstOrDefault = order.OrderDetails.FirstOrDefault(od=> od.OrderID == order.ID);
            if (firstOrDefault != null)
            {

                result.Username = firstOrDefault.Username;

            }
            return result;
        }

        public IQueryable<VwOrder> ShowOrphanRecords()
        {
            return (from t in Entity.Orders
                    where t.OrderDetails.Count < 1
                    select new VwOrder
                    {
                        ID = t.ID,
                        DatePlaced = t.DatePlaced,
                        State = t.Status,
                        Username = null
                    });
        }
    
        public void Update(VwOrder objectToUpdate)
        {
            Order t = GetOrder(objectToUpdate.ID);
            t.Status = objectToUpdate.State;
            Entity.Orders.ApplyCurrentValues(t);
            Entity.SaveChanges();
        }

    }
}