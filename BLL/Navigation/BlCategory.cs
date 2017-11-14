using System;
using System.Collections.Generic;
using System.Linq;
using BLL.CustomExceptions;
using Common.Views;
using DAL.Repositories;

namespace BLL.Navigation
{
    /// <summary>
    /// Bl Category
    /// </summary>
    public class BlCategory
    {
        #region Public Methods

        /// <summary>
        /// Create
        /// </summary>
        /// <param name="record">record</param>
        /// <returns></returns>
        public VwCategory Create(VwCategory record)
        {
            try
            {
                var mr = new CategorysRepo(true);
                mr.Create(record);
                VwCategory m = mr.Read(record.ID);
                return m;
            }
            catch
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
                var mr = new CategorysRepo(true);
                VwCategory m = mr.Read(id);
                mr.Delete(m);
            }
            catch
            {
                throw new DataDeletionException();
            }
        }

        /// <summary>
        /// Get Children
        /// </summary>
        /// <param name="parentId">parent</param>
        /// <returns></returns>
        public IQueryable<VwCategory> GetChildren(int parentId)
        {
            try
            {
                return new CategorysRepo(false).GetChildren(parentId);
            }
            catch (Exception)
            {
                throw new
                    DataListException();
            }
        }

        /// <summary>
        /// Get Count
        /// </summary>
        /// <returns></returns>
        public int GetCount()
        {
            return new CategorysRepo(true).GetCount();
        }

        /// <summary>
        /// Get Last Child ID
        /// </summary>
        /// <param name="parentId">parent</param>
        /// <returns></returns>
        public int GetLastChildID(int? parentId)
        {
            return new CategorysRepo(true).GetLastChildID(parentId);
        }

        /// <summary>
        /// Get Last Root ID
        /// </summary>
        /// <returns></returns>
        public int GetLastRootID()
        {
            return new CategorysRepo(true).GetLastRootID();
        }

        /// <summary>
        /// Get Root Parents
        /// </summary>
        /// <returns></returns>
        public IEnumerable<VwCategory> GetRootParents()
        {
            try
            {
                return new CategorysRepo(false).GetRootParents();
            }
            catch (Exception)
            {
                throw new DataListException();
            }
        }

        /// <summary>
        /// List All
        /// </summary>
        /// <returns></returns>
        public IQueryable<VwCategory> ListAll()
        {
            try
            {
                return new CategorysRepo(false).ListAll();
            }
            catch (Exception)
            {
                throw new DataListException();
            }
        }

        /// <summary>
        /// List Root Categories
        /// </summary>
        /// <returns></returns>
        public IEnumerable<VwCategory> ListRootCategories()
        {
            try
            {
                return new CategorysRepo(false).GetRootParents();
            }
            catch (Exception)
            {
                throw new DataListException();
            }
        }

        /// <summary>
        /// List Sub Categories
        /// </summary>
        /// <param name="parentId">parent</param>
        /// <returns></returns>
        public IEnumerable<VwCategory> ListSubCategories(int? parentId)
        {
            try
            {
                return new CategorysRepo(false).GetChildren(parentId);
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
        public IQueryable<VwCategory> PagedList(int startIndex, int pageSize, string sorting)
        {
            try
            {
                return new CategorysRepo(true).PagedList(startIndex, pageSize, sorting);
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
        public VwCategory Update(VwCategory record)
        {
            try
            {
                new CategorysRepo(true).Update(record);
                record = new CategorysRepo(true).Read(record.ID);
                return record;
            }
            catch (Exception)
            {
                throw new DataUpdateException();
            }
        }

        #endregion Public Methods
    }
}