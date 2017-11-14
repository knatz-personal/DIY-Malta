using System;
using System.Collections.Generic;
using System.Linq;
using BLL.Accounts;
using BLL.CustomExceptions;
using Common.Views;
using DAL.Repositories;

namespace BLL.Catalogue
{
    /// <summary>
    /// Bl Products
    /// </summary>
    public class BlProducts
    {
        /// <summary>
        /// Add To Cart
        /// </summary>
        /// <param name="s">s</param>
        /// <returns></returns>
        public bool AddToCart(VwShoppingCart s)
        {
            bool result = false;
            if (s.Quantity > 0)
            {
                try
                {
                    var pr = new ProductsRepo(false);
                    VwShoppingCart sc = CheckShoppingCart(s.ProductID, s.Username);
                    if (sc == null)
                    {
                        sc = new VwShoppingCart
                        {
                            ID = Guid.NewGuid(),
                            Username = s.Username,
                            ProductID = s.ProductID,
                            Quantity = s.Quantity
                        };
                        pr.AddToCart(sc);
                        result = true;
                    }
                    else
                    {
                        sc.Quantity = s.Quantity;
                        pr.UpdateCart(sc);
                        result = true;
                    }
                }
                catch (Exception)
                {
                    result = false;
                    throw new Exception("Failed to add product to your cart");
                }
            }
            return result;
        }

        /// <summary>
        /// Check Shopping Cart
        /// </summary>
        /// <param name="productId">product</param>
        /// <param name="username">username</param>
        /// <returns></returns>
        public VwShoppingCart CheckShoppingCart(Guid productId, string username)
        {
            return new ProductsRepo(false).CheckProductInCart(productId, username);
        }

        /// <summary>
        /// Create
        /// </summary>
        /// <param name="record">record</param>
        /// <returns></returns>
        public VwProduct Create(VwProduct record)
        {
            try
            {
                record.ID = Guid.NewGuid();
                new ProductsRepo(true).Create(record);
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
            try
            {
                VwProduct record = new ProductsRepo(true).Read(id);
                record.IsActive = false;
                new ProductsRepo(true).Update(record);
            }
            catch (Exception)
            {
                throw new DataDeletionException();
            }
            
        }

        /// <summary>
        /// Find Product
        /// </summary>
        /// <param name="query">query</param>
        /// <returns></returns>
        public IQueryable<string> FindProduct(string query)
        {
            return new ProductsRepo(false).FindProduct(query);
        }

        /// <summary>
        /// Get Cart Items Count
        /// </summary>
        /// <param name="username">username</param>
        /// <returns></returns>
        public int GetCartItemsCount(string username)
        {
            return new ProductsRepo(false).GetCartItemsCount(username);
        }

        /// <summary>
        /// Get Cart Total Price
        /// </summary>
        /// <param name="username">username</param>
        /// <returns></returns>
        public decimal GetCartTotalPrice(string username)
        {
            return new ProductsRepo(false).GetCartTotalPrice(username);
        }

        /// <summary>
        /// Get Count
        /// </summary>
        /// <returns></returns>
        public int GetCount()
        {
            return new ProductsRepo(false).GetCount();
        }

        /// <summary>
        /// Paged List
        /// </summary>
        /// <param name="query">query</param>
        /// <param name="startIndex">start Index</param>
        /// <param name="pageSize">page Size</param>
        /// <param name="sorting">sorting</param>
        /// <returns></returns>
        public IQueryable<VwProduct> PagedList(string query, int startIndex, int pageSize, string sorting)
        {
            try
            {
                return new ProductsRepo(false).FilteredPagedList(query, startIndex, pageSize, sorting);
            }
            catch (Exception)
            {
                
                throw new DataListException();
            }
        }

        /// <summary>
        /// Paged List
        /// </summary>
        /// <param name="query">query</param>
        /// <param name="category">category</param>
        /// <param name="userType">user Type</param>
        /// <returns></returns>
        public IEnumerable<VwProduct> PagedList(string query, int category, int userType)
        {
            IEnumerable<VwProduct> result = null;
            try
            {
                var pr = new ProductsRepo(false);
                //1. Display all active products
                if (string.IsNullOrEmpty(query) && category == 0)
                {
                    result = pr.ListAllActive(userType);
                }
                //2. Display all active product that match the query
                if (!string.IsNullOrEmpty(query) && category == 0)
                {
                    result = pr.Filter(query, userType);
                }
                //3. Display all active products in a category
                if (string.IsNullOrEmpty(query) && category > 0)
                {
                    result = pr.Filter(category, userType);
                }
                //4. Display all active products in a category that match the query
                if (!string.IsNullOrEmpty(query) && category > 0)
                {
                    result = pr.Filter(query, category, userType);
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
        /// <param name="id">id</param>
        /// <param name="username">username</param>
        /// <returns></returns>
        public VwProduct Read(Guid id, string username)
        {
            try
            {
                int? userType = !string.IsNullOrEmpty(username)
            ? new BlUsers().GetUser(username).UserType
            : new BlUsers().GetTypeID("Customer");
                return new ProductsRepo(false).Read(id, userType);
            }
            catch (Exception)
            {
                throw new Exception("An error occurred while loading product details. Sorry for the inconvenience");
            }
        }

        /// <summary>
        /// Read
        /// </summary>
        /// <param name="productId">product</param>
        /// <returns></returns>
        public VwProduct Read(Guid productId)
        {
            return new ProductsRepo(false).Read(productId);
        }

        /// <summary>
        /// Update
        /// </summary>
        /// <param name="record">record</param>
        /// <returns></returns>
        public void Update(VwProduct record)
        {
            var pr = new ProductsRepo(true);
            var catr = new CategorysRepo(true);
            try
            {
                pr.Entity = catr.Entity;

                pr.Entity.Connection.Open();

                pr.Transaction = catr.Transaction = pr.Entity.Connection.BeginTransaction();

                pr.Update(record);

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

        /// <summary>
        /// Update Cart
        /// </summary>
        /// <param name="s">s</param>
        /// <returns></returns>
        public void UpdateCart(VwShoppingCart s)
        {
            try
            {
                new ProductsRepo(false).UpdateCart(s);
            }
            catch (Exception)
            {
                throw new DataUpdateException();
            }
        }
    }
}