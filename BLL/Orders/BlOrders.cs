using System;
using System.Linq;
using BLL.CustomExceptions;
using Common.Views;
using DAL.Repositories;
using DAL.Repositories.Orders;

namespace BLL.Orders
{
    /// <summary>
    /// Bl Orders
    /// </summary>
    public class BlOrders
    {

        #region Public Methods

        /// <summary>
        /// Create
        /// </summary>
        /// <param name="record">record</param>
        /// <returns></returns>
        public VwOrder Create(VwOrder record)
        {
            try
            {
                var tr = new OrdersRepo(true);
                tr.Create(record);
                return record;
            }
            catch (Exception)
            {
                
                throw new DataInsertionException();
            }
        }

        /// <summary>
        /// Delete
        /// </summary>
        /// <param name="id">id</param>
        /// <returns></returns>
        public void Delete(Guid id)
        {
            var tr = new OrdersRepo(true);
            var odr = new OrderDetailsRepo(true);
            VwOrder t = tr.Read(id);
            IQueryable<VwOrderDetail> od = odr.GetOrderDetails(id);


            try
            {
                tr.Entity = odr.Entity;

                tr.Entity.Connection.Open();

                tr.Transaction = odr.Transaction = tr.Entity.Connection.BeginTransaction();

                foreach (VwOrderDetail item in od)
                {
                    odr.Delete(item);
                }

                tr.Delete(t);

                tr.Transaction.Commit();
            }
            catch (Exception)
            {
                tr.Transaction.Rollback();

                throw new TransactionFailedException();
            }
            finally
            {
                tr.Entity.Connection.Close();
            }
        }

        /// <summary>
        /// Get Grand Total
        /// </summary>
        /// <param name="subTotal">sub Total</param>
        /// <param name="tax">tax</param>
        /// <returns></returns>
        public decimal? GetGrandTotal(decimal? subTotal, decimal? tax)
        {
            return subTotal + tax;
        }

        /// <summary>
        /// Get Items
        /// </summary>
        /// <param name="id">id</param>
        /// <returns></returns>
        public IQueryable<VwOrderDetail> GetItems(Guid id)
        {
            try
            {
                return new OrdersRepo(false).GetItems(id);
            }
            catch (Exception)
            {
                
                throw new DataListException();
            }
        }

        /// <summary>
        /// Get Latest Order
        /// </summary>
        /// <param name="username">username</param>
        /// <returns></returns>
        public VwOrder GetLatestOrder(string username)
        {
            return new OrdersRepo(false).GetLatestOrder(username);
        }

        /// <summary>
        /// Get Sub Total
        /// </summary>
        /// <param name="username">username</param>
        /// <returns></returns>
        public decimal GetSubTotal(string username)
        {
            return new ProductsRepo(false).GetCartTotalPrice(username);
        }

        /// <summary>
        /// Get Sub Total
        /// </summary>
        /// <param name="orderId">order</param>
        /// <returns></returns>
        public decimal? GetSubTotal(Guid orderId)
        {
            decimal? sum = 0;
            IQueryable<VwOrderDetail> ods = new OrderDetailsRepo(false).GetOrderDetails(orderId);
            if (ods.Any())
            {
                sum = ods.Sum(t => t.TotalPrice);
            }
            return sum;
        }

        /// <summary>
        /// Get Tax
        /// </summary>
        /// <param name="username">username</param>
        /// <returns></returns>
        public decimal GetTax(string username)
        {
            return new ProductsRepo(false).GetCartTotalTax(username);
        }

        /// <summary>
        /// Get Total Tax
        /// </summary>
        /// <param name="orderId">order</param>
        /// <returns></returns>
        public decimal? GetTax(Guid orderId)
        {
            decimal? sum = 0;
            IQueryable<VwOrderDetail> od = new BlOrderDetails().GetOrderDetails(orderId);
            if (od.Any())
            {
                sum =  od.Sum(t => (t.VAT / 100) *  t.UnitPrice  * t.Quantity);
            }
            return sum;
        }

        /// <summary>
        /// Paged List
        /// </summary>
        /// <param name="query">query</param>
        /// <param name="start">start</param>
        /// <param name="end">end</param>
        /// <param name="state">state</param>
        /// <param name="startIndex">start Index</param>
        /// <param name="pageSize">page Size</param>
        /// <param name="sorting">sorting</param>
        /// <returns></returns>
        public IQueryable<VwOrder> PagedList(string query, string start, string end, int state, int startIndex,
            int pageSize, string sorting)
        {
            IQueryable<VwOrder> result = null;
            try
            {
                DateTime startDate;
                DateTime endDate;
                bool isValidStartDate = DateTime.TryParse(start, out startDate);
                bool isValidEndDate = DateTime.TryParse(end, out endDate);

                //search by username only
                if (!string.IsNullOrEmpty(query) && state < 1 && (!isValidStartDate || !isValidEndDate))
                {
                    result = new OrdersRepo(false).PagedList(query, startIndex, pageSize, sorting);
                } //search by state only
                else if (state > 0 && string.IsNullOrEmpty(query) && (!isValidStartDate || !isValidEndDate))
                {
                    result = new OrdersRepo(false).PagedList(state, startIndex, pageSize, sorting);
                } //search by date range only
                else if (isValidStartDate && isValidEndDate && string.IsNullOrEmpty(query) && state < 1)
                {
                    result = new OrdersRepo(false).PagedList(startDate, endDate, startIndex, pageSize,
                        sorting);
                } //search by username then order state
                else if (!string.IsNullOrEmpty(query) && state > 0 && (!isValidStartDate || !isValidEndDate))
                {
                    result = new OrdersRepo(false).PagedList(query, state, startIndex, pageSize, sorting);
                } //search by username then date range
                else if (!string.IsNullOrEmpty(query) && state < 1 && isValidStartDate && isValidEndDate)
                {
                    result = new OrdersRepo(false).PagedList(query, 0, startDate, endDate, startIndex, pageSize, sorting);
                }
                //search by state then date range
                else if (string.IsNullOrEmpty(query) && state > 0 && isValidStartDate && isValidEndDate)
                {
                    result = new OrdersRepo(false).PagedList(string.Empty, state, startDate, endDate, startIndex, pageSize,
                        sorting);
                } //search by username then order state then date range
                else if (!string.IsNullOrEmpty(query) && state > 1 && isValidStartDate && isValidEndDate)
                {
                    result = new OrdersRepo(false).PagedList(query, state, startDate, endDate, startIndex, pageSize,
                        sorting);
                } //Show Orphan Records
                else
                {
                    result = new OrdersRepo(false).ShowOrphanRecords();
                }
            }
            catch (Exception)
            {
                
                throw new DataListException();
            }

            return result;
        }

        /// <summary>
        /// Read
        /// </summary>
        /// <param name="orderId">order</param>
        /// <returns></returns>
        public VwOrder Read(Guid orderId)
        {
            return new OrdersRepo(false).Read(orderId);
        }

        /// <summary>
        /// Update
        /// </summary>
        /// <param name="record">record</param>
        /// <returns></returns>
        public VwOrder Update(VwOrder record)
        {
            try
            {
                var blo = new OrdersRepo(true);
                blo.Update(record);
                return blo.Read(record.ID);
            }
            catch (Exception)
            {
                
                throw new DataUpdateException();
            }
        }

        #endregion Public Methods

    }
}