
using System;
using BLL.CustomExceptions;
using Common.Views;
using DAL.Repositories;
using System.Linq;

namespace BLL.Orders
{
    /// <summary>
    /// Bl Order States
    /// </summary>
    public class BlOrderStates
    {
        #region Public Methods

        /// <summary>
        /// Create
        /// </summary>
        /// <param name="name">name</param>
        /// <returns></returns>
        public VwOrderState Create(string name)
        {
            try
            {
                var tr = new OrderStatesRepo(true);
                tr.Create(new VwOrderState { Name = name });
                VwOrderState t = tr.Read(name);
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
        public void Delete(int id)
        {
            try
            {
                var tr = new OrderStatesRepo(true);
                VwOrderState t = tr.Read(id);
                tr.Delete(t);
            }
            catch (Exception)
            {

                throw new DataDeletionException();
            }
        }

        /// <summary>
        /// Get Count
        /// </summary>
        /// <returns></returns>
        public int GetCount()
        {
            return new OrderStatesRepo(true).GetCount();
        }

        /// <summary>
        /// Get State ID
        /// </summary>
        /// <param name="name">name</param>
        /// <returns></returns>
        public int? GetStateID(string name)
        {
            return new OrderStatesRepo(false).GetStateID(name);
        }

        /// <summary>
        /// List All
        /// </summary>
        /// <returns></returns>
        public IQueryable<VwOrderState> ListAll()
        {
            try
            {
                return new OrderStatesRepo(false).ListAll();
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
        public IQueryable<VwOrderState> PagedList(int startIndex, int pageSize, string sorting)
        {
            try
            {
                return new OrderStatesRepo(true).PagedList(startIndex, pageSize, sorting);
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
        public void Update(VwOrderState record)
        {
            try
            {
                new OrderStatesRepo(true).Update(record);
            }
            catch (Exception)
            {
                
                throw new DataUpdateException();
            }
        }

        #endregion Public Methods
    }
}
