using System;
using BLL.CustomExceptions;
using Common.Views;
using DAL.Repositories;
using System.Linq;

namespace BLL.Accounts
{
    /// <summary>
    ///     Towns business logic 
    /// </summary>
    public class BlTowns
    {
        #region Public Methods

        /// <summary>
        ///     Create Town 
        /// </summary>
        /// <param name="name">
        ///     name 
        /// </param>
        /// <returns>
        ///     </returns>
        public VwTown Create(string name)
        {
            try
            {
                var tr = new TownsRepo(true);
                tr.Create(new VwTown { Name = name });
                VwTown t = tr.Read(name);
                return t;
            }
            catch (Exception)
            {
                throw new DataInsertionException();
            }
        }

        /// <summary>
        ///     Delete Town 
        /// </summary>
        /// <param name="id">
        ///     id 
        /// </param>
        /// <returns>
        ///     </returns>
        public void Delete(int id)
        {
            try
            {
                var tr = new TownsRepo(true);
                VwTown t = tr.Read(id);
                tr.Delete(t);
            }
            catch (Exception)
            {
                throw new DataDeletionException();
            }
        }

        /// <summary>
        ///     Get Count 
        /// </summary>
        /// <returns>
        ///     </returns>
        public int GetCount()
        {
            return new TownsRepo(true).GetCount();
        }

        /// <summary>
        ///     List All 
        /// </summary>
        /// <returns>
        ///     </returns>
        public IQueryable<VwTown> ListAll()
        {
            try
            {
                return new TownsRepo(false).ListAll();
            }
            catch (Exception)
            {
                throw new DataListException();
            }
        }

        /// <summary>
        ///     Paged List of Towns 
        /// </summary>
        /// <param name="startIndex">
        ///     start Index 
        /// </param>
        /// <param name="pageSize">
        ///     page Size 
        /// </param>
        /// <param name="sorting">
        ///     sorting 
        /// </param>
        /// <returns>
        ///     </returns>
        public IQueryable<VwTown> PagedList(int startIndex, int pageSize, string sorting)
        {
            try
            {
                return new TownsRepo(true).PagedList(startIndex, pageSize, sorting);
            }
            catch (Exception)
            {
                 throw new DataListException();
            }
        }

        /// <summary>
        ///     Update Town 
        /// </summary>
        /// <param name="record">
        ///     record 
        /// </param>
        /// <returns>
        ///     </returns>
        public void Update(VwTown record)
        {
            try
            {
                new TownsRepo(true).Update(record);
            }
            catch (Exception)
            {
                 throw new DataUpdateException();
            }
        }

        #endregion Public Methods
    }
}