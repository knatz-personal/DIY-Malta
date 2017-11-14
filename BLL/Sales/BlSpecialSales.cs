using System;
using System.Linq;
using BLL.CustomExceptions;
using Common.Views;
using DAL.Repositories;

namespace BLL.Sales
{
    /// <summary>
    ///     Bl Special Sales
    /// </summary>
    public class BlSpecialSales
    {
        /// <summary>
        ///     List All
        /// </summary>
        /// <returns></returns>
        public IQueryable<VwSpecialSale> ListAll()
        {
            try
            {
                return new SpecialSalesRepo(false).ListAll();
            }
            catch (Exception)
            {
                throw new DataListException();
            }
        }

        /// <summary>
        ///     Get Count
        /// </summary>
        /// <returns></returns>
        public int GetCount()
        {
            return new SpecialSalesRepo(true).GetCount();
        }

        /// <summary>
        ///     Create
        /// </summary>
        /// <param name="record">record</param>
        /// <returns></returns>
        public VwSpecialSale Create(VwSpecialSale record)
        {
            try
            {
                var tr = new SpecialSalesRepo(true);
                record.ID = Guid.NewGuid();
                tr.Create(record);
                VwSpecialSale t = tr.Read(record);
                return t;
            }
            catch (Exception)
            {
                throw new DataInsertionException();
            }
        }

        /// <summary>
        ///     Update
        /// </summary>
        /// <param name="record">record</param>
        /// <returns></returns>
        public void Update(VwSpecialSale record)
        {
            try
            {
                new SpecialSalesRepo(true).Update(record);
            }
            catch (Exception)
            {
                throw new DataUpdateException();
            }
        }

        /// <summary>
        ///     Delete
        /// </summary>
        /// <param name="id">id</param>
        /// <returns></returns>
        public void Delete(Guid id)
        {
            try
            {
                var tr = new SpecialSalesRepo(true);
                VwSpecialSale t = tr.Read(id);
                tr.Delete(t);
            }
            catch (Exception)
            {
                throw new DataDeletionException();
            }
        }

        /// <summary>
        ///     Paged List
        /// </summary>
        /// <param name="startIndex">start Index</param>
        /// <param name="pageSize">page Size</param>
        /// <param name="sorting">sorting</param>
        /// <returns></returns>
        public IQueryable<VwSpecialSale> PagedList(int startIndex, int pageSize, string sorting)
        {
            try
            {
                return new SpecialSalesRepo(true).PagedList(startIndex, pageSize, sorting);
            }
            catch (Exception)
            {
                throw new DataListException();
            }
        }

        public IQueryable<VwSpecialSale> PagedList(string query, string start, string end, int startIndex, int pageSize,
            string sorting)
        {
            try
            {
                DateTime startDate;
                DateTime endDate;
                if (string.IsNullOrEmpty(start) || string.IsNullOrEmpty(end))
                {
                    return new SpecialSalesRepo(true).PagedList(query, startIndex, pageSize, sorting);
                }
                bool isValidStartDate = DateTime.TryParse(start, out startDate);
                bool isValidEndDate = DateTime.TryParse(end, out endDate);
                if (isValidStartDate && isValidEndDate)
                {
                    return new SpecialSalesRepo(true).PagedList(query, startDate, endDate, startIndex, pageSize, sorting);
                }
                return new SpecialSalesRepo(true).PagedList(query, startIndex, pageSize, sorting);
            }
            catch (Exception)
            {
                throw new DataListException();
            }
        }
    }
}