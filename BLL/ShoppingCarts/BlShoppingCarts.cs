
using System;
using BLL.CustomExceptions;
using Common.Views;
using DAL.Repositories.Catalogue;
using System.Linq;

namespace BLL.ShoppingCarts
{
    /// <summary>
    /// Bl Shopping Carts
    /// </summary>
    public class BlShoppingCarts
    {
        #region Public Methods

        /// <summary>
        /// Create
        /// </summary>
        /// <param name="record">record</param>
        /// <returns></returns>
        public VwShoppingCart Create(VwShoppingCart record)
        {
            try
            {
                var tr = new ShoppingCartsRepo(true);
                tr.Create(record);
                VwShoppingCart t = tr.Read(record);
                return t;
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
                var tr = new ShoppingCartsRepo(false);
                var t = tr.Read(id);
                tr.Delete(t);
            }
            catch (Exception)
            {
                throw new DataDeletionException();
            }
        }

        /// <summary>
        /// List All
        /// </summary>
        /// <param name="username">username</param>
        /// <returns></returns>
        public IQueryable<VwShoppingCart> ListAll(string username)
        {
            try
            {
                return new ShoppingCartsRepo(false).ListAll(username);
            }
            catch (Exception)
            {
                
                throw new DataListException();
            }
        }
        /// <summary>
        /// Paged List
        /// </summary>
        /// <param name="startIndex">start Index</param>
        /// <param name="pageSize">page Size</param>
        /// <param name="sorting">sorting</param>
        /// <returns></returns>
        public IQueryable<VwShoppingCart> PagedList(int startIndex, int pageSize, string sorting)
        {
            try
            {
                return new ShoppingCartsRepo(true).PagedList(startIndex, pageSize, sorting);
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
        public void Update(VwShoppingCart record)
        {
            try
            {
                new ShoppingCartsRepo(true).Update(record);
            }
            catch (Exception)
            {
                
                throw new DataUpdateException();
            }
        }

        #endregion Public Methods
    }
}
