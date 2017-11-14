using System;
using System.Linq;
using BLL.CustomExceptions;
using Common.EntityModel;
using Common.Views;
using DAL.Repositories;
using DAL.Repositories.Orders;

namespace BLL.Orders
{
    /// <summary>
    /// Bl Order Details
    /// </summary>
    public class BlOrderDetails
    {

        #region Public Methods

        /// <summary>
        /// Create
        /// </summary>
        /// <param name="record">record</param>
        /// <returns></returns>
        public void Create(VwOrderDetail record)
        {
            try
            {
                if (record.Stock > record.Quantity)
                {
                    var odr = new OrderDetailsRepo(false);
                    var pr = new ProductsRepo(false) { Entity = odr.Entity };

                    pr.Entity.Connection.Open();
                    pr.Transaction = odr.Transaction = pr.Entity.Connection.BeginTransaction();
                    try
                    {
                        odr.Create(record);
                        var p = pr.Read(record.ProductID);
                        if (record.Quantity != null)
                        {
                            p.Stock -= (int)record.Quantity;
                            pr.Update(p);
                        }
                        pr.Transaction.Commit();
                    }
                    catch (Exception)
                    {
                        pr.Transaction.Rollback();
                        throw new TransactionFailedException();
                    }
                    finally
                    {
                        pr.Entity.Connection.Close();
                    }
                }
                else
                {
                    throw new ExceedsStockException();
                }
            }
            catch (Exception)
            {
                throw new DataInsertionException();
            }
            
        }

        /// <summary>
        /// Delete
        /// </summary>
        /// <param name="orderId">order</param>
        /// <param name="productId">product</param>
        /// <param name="username">username</param>
        /// <returns></returns>
        public void Delete(Guid orderId, Guid productId, string username)
        {
            var pr = new ProductsRepo(true);
            var odr = new OrderDetailsRepo(true);
            try
            {
                pr.Entity = odr.Entity;

                pr.Entity.Connection.Open();

                pr.Transaction = odr.Transaction = pr.Entity.Connection.BeginTransaction();

                VwOrderDetail od = odr.GetOrderDetail(orderId, productId, username);

                //update product stock
                VwProduct p = pr.Read(productId);
                if (od.Quantity != null)
                {
                    p.Stock += (int)od.Quantity;
                }
                pr.Update(p);

                //delete order detail
                odr.Delete(od);

                pr.Transaction.Commit();
            }
            catch (Exception ex)
            {
                pr.Transaction.Rollback();
                throw new TransactionFailedException();
            }
            finally
            {
                pr.Entity.Connection.Close();
            }
        }

        /// <summary>
        /// Get Order Details
        /// </summary>
        /// <param name="id">id</param>
        /// <returns></returns>
        public IQueryable<VwOrderDetail> GetOrderDetails(Guid id)
        {
            try
            {
                return new OrderDetailsRepo(false).GetOrderDetails(id);
            }
            catch (Exception)
            {

                throw new DataListException();
            }
        }

        /// <summary>
        /// Update
        /// </summary>
        /// <param name="record">record</param>
        /// <returns></returns>
        public VwOrderDetail Update(VwOrderDetail record)
        {
            Order o = new OrdersRepo(false).GetOrder(record.OrderID);
            int temp = 0;
            if (o.OrderState.Name != "Complete" || o.OrderState.Name != "Picked Up" || o.OrderState.Name != "Delivered")
            {
                var pr = new ProductsRepo(true);
                var odr = new OrderDetailsRepo(true);
                try
                {
                    pr.Entity = odr.Entity;

                    pr.Entity.Connection.Open();

                    pr.Transaction = odr.Transaction = pr.Entity.Connection.BeginTransaction();

                    VwOrderDetail od = odr.GetOrderDetail(record.OrderID, record.ProductID, record.Username);
                    VwProduct p = pr.Read(record.ProductID);

                    if (o.OrderState.Name == "Cancelled")
                    {
                        //increase stock level and
                        if (od.Quantity != null)
                        {
                            record.Stock += (int)od.Quantity;

                            //change the stock attribute in the products table
                            p.Stock = record.Stock;
                            //update the order details
                            odr.Update(record);
                            //update the product details
                            pr.Update(p);

                            record = odr.GetOrderDetail(record.OrderID, record.ProductID, record.Username);
                            record.TotalPrice = record.Quantity * record.UnitPrice;
                        }
                    }
                    else
                    {
                        if (od.Stock >= record.Quantity && od.Stock > 0)
                        {
                            record.Stock = od.Stock;


                            //if quantity is 1 and its changed to 2 
                            if (od.Quantity < record.Quantity)
                            {
                                //calculate difference
                                temp = (int)(record.Quantity - od.Quantity);
                                //decrease stock level and
                                record.Stock -= temp;
                            }
                            else // if quantity is 2 and its changed to 1 
                            {
                                //calculate difference
                                temp = (int)(od.Quantity - record.Quantity);
                                //increase stock level and
                                record.Stock += temp;
                            }
                            //change the stock attribute in the products table
                            p.Stock = record.Stock;
                            //update the order details
                            odr.Update(record);
                            //update the product details
                            pr.Update(p);

                            record = odr.GetOrderDetail(record.OrderID, record.ProductID, record.Username);
                            record.TotalPrice = record.Quantity * record.UnitPrice;
                        }
                        else
                        {
                            throw new ExceedsStockException();
                        }
                    }
                    pr.Transaction.Commit();
                }
                catch (Exception ex)
                {
                    pr.Transaction.Rollback();
                    throw new TransactionFailedException();
                }
                finally
                {
                    pr.Entity.Connection.Close();
                }
            }
            else
            {
                throw new Exception("This order has been shipped. Editing is forbidden!");
            }
            return record;
        }

        #endregion Public Methods

    }
}