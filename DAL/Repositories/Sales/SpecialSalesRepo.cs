using System.Collections.Generic;
using Common.EntityModel;
using Common.Views;
using System;
using System.Linq;
using System.Linq.Dynamic;

namespace DAL.Repositories
{
    /// <summary>
    ///     Special SalesRepository
    /// </summary>
    public class SpecialSalesRepo : AppDbConnection, IDataRepository<VwSpecialSale>
    {
        #region Public Constructors

        /// <summary>
        ///     Special SalesRepository
        /// </summary>
        /// <param name="isAdministrator">
        ///     is Administrator
        /// </param>
        /// <returns>
        ///     </returns>
        public SpecialSalesRepo(bool isAdministrator)
            : base(isAdministrator)
        {
        }

        #endregion Public Constructors

        #region Public Methods

        /// <summary>
        ///     Create
        /// </summary>
        /// <param name="newObject">
        ///     new Object
        /// </param>
        /// <returns>
        ///     </returns>
        public void Create(VwSpecialSale newObject)
        {
            Entity.SpecialSales.AddObject(new SpecialSale
            {
                ID = newObject.ID,
                Name = newObject.Name,
                DateStarted = newObject.DateStarted,
                DateExpired = newObject.DateExpired,
                Discount = newObject.Discount
            });
            Entity.SaveChanges();
        }

        /// <summary>
        ///     Delete
        /// </summary>
        /// <param name="objectToDelete">
        ///     object To Delete
        /// </param>
        /// <returns>
        ///     </returns>
        public void Delete(VwSpecialSale objectToDelete)
        {
            SpecialSale t = GetSpecialSale(objectToDelete.ID);
            Entity.SpecialSales.DeleteObject(t);
            Entity.SaveChanges();
        }

        public IQueryable<VwSpecialSale> Filter(string query, DateTime startDate, DateTime endDate)
        {
            var temp = Entity.SpecialSales.Where(q => q.Name.Contains(query)).AsEnumerable();
            var result = temp.Where(s => s.DateStarted.Date >= startDate.Date && (s.DateExpired >= s.DateStarted && s.DateExpired.Date <= endDate.Date));

            return result.Select(t => new VwSpecialSale
                    {
                        ID = t.ID,
                        Name = t.Name,
                        DateStarted = t.DateStarted,
                        DateExpired = t.DateExpired,
                        Discount = t.Discount
                    }).AsQueryable();
        }

        public IQueryable<VwSpecialSale> Filter(string query)
        {
            return (from t in Entity.SpecialSales
                    where t.Name.Contains(query)
                    select new VwSpecialSale
                    {
                        ID = t.ID,
                        Name = t.Name,
                        DateStarted = t.DateStarted,
                        DateExpired = t.DateExpired,
                        Discount = t.Discount
                    });
        }

        /// <summary>
        ///     Get Count
        /// </summary>
        /// <returns>
        ///     </returns>
        public int GetCount()
        {
            return Entity.SpecialSales.Count();
        }

        /// <summary>
        ///     List All
        /// </summary>
        /// <returns>
        ///     </returns>
        public IQueryable<VwSpecialSale> ListAll()
        {
            return (from t in Entity.SpecialSales
                    select new VwSpecialSale
                    {
                        ID = t.ID,
                        Name = t.Name,
                        DateStarted = t.DateStarted,
                        DateExpired = t.DateExpired,
                        Discount = t.Discount
                    });
        }

        /// <summary>
        ///     Paged List
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
        public IQueryable<VwSpecialSale> PagedList(int startIndex, int pageSize, string sorting)
        {
            IQueryable<VwSpecialSale> result =
                ListAll().OrderBy(sorting).Skip(startIndex).Take(pageSize);
            return result;
        }

        public IQueryable<VwSpecialSale> PagedList(string query, int startIndex, int pageSize, string sorting)
        {
            IQueryable<VwSpecialSale> result = null;

            result = Filter(query).OrderBy(sorting).Skip(startIndex).Take(pageSize);

            return result;
        }

        public IQueryable<VwSpecialSale> PagedList(string query, DateTime startDate, DateTime endDate, int startIndex, int pageSize, string sorting)
        {
            IQueryable<VwSpecialSale> result = null;
            result = Filter(query, startDate, endDate).OrderBy(sorting).Skip(startIndex).Take(pageSize);
            return result;
        }

        /// <summary>
        ///     Read
        /// </summary>
        /// <param name="key">
        ///     key
        /// </param>
        /// <returns>
        ///     </returns>
        public VwSpecialSale Read(object key)
        {
            var tw = new SpecialSale();
            if (key is Guid)
            {
                tw = Entity.SpecialSales.SingleOrDefault(t => t.ID == (Guid)key);
            }
            if (tw != null)
                return (new VwSpecialSale
                {
                    ID = tw.ID,
                    Name = tw.Name,
                    DateStarted = tw.DateStarted,
                    DateExpired = tw.DateExpired,
                    Discount = tw.Discount
                });
            return null;
        }

        /// <summary>
        ///     Update
        /// </summary>
        /// <param name="objectToUpdate">
        ///     object To Update
        /// </param>
        /// <returns>
        ///     </returns>
        public void Update(VwSpecialSale objectToUpdate)
        {
            SpecialSale t = GetSpecialSale(objectToUpdate.ID);
            t.Name = objectToUpdate.Name;
            t.Discount = objectToUpdate.Discount;
            t.DateStarted = objectToUpdate.DateStarted;
            t.DateExpired = objectToUpdate.DateExpired;
            Entity.SpecialSales.ApplyCurrentValues(t);
            Entity.SaveChanges();
        }

        #endregion Public Methods

        #region Private Methods

        /// <summary>
        ///     Get Special Sale
        /// </summary>
        /// <param name="id">
        ///     id
        /// </param>
        /// <returns>
        ///     </returns>
        private SpecialSale GetSpecialSale(Guid id)
        {
            return Entity.SpecialSales.SingleOrDefault(e => e.ID == id);
        }

        #endregion Private Methods
    }
}