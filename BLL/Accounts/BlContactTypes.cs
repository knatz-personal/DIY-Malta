using System;
using BLL.CustomExceptions;
using Common.Views;
using DAL.Repositories;
using System.Linq;

namespace BLL.Accounts
{
    /// <summary>
    ///     Contact Types business logic 
    /// </summary>
    public class BlContactTypes
    {
        #region Public Methods

        /// <summary>
        ///     Create ContactType 
        /// </summary>
        /// <param name="name">
        ///     name 
        /// </param>
        /// <returns>
        ///     </returns>
        public VwContactType Create(string name)
        {
            try
            {
                var tr = new ContactTypesRepo(true);
                tr.Create(new VwContactType { Name = name });
                VwContactType t = tr.Read(name);
                return t;
            }
            catch (Exception)
            {
                
                throw new DataInsertionException();
            }
        }

        /// <summary>
        ///     Delete ContactType 
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
                var gr = new ContactTypesRepo(true);
                VwContactType g = gr.Read(id);
                gr.Delete(g);
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
            return new ContactTypesRepo(true).GetCount();
        }

        /// <summary>
        ///     List All 
        /// </summary>
        /// <returns>
        ///     </returns>
        public IQueryable<VwContactType> ListAll()
        {
            try
            {
                return new ContactTypesRepo(false).ListAll();
            }
            catch (Exception)
            {
                
                throw new DataListException();
            }
        }

        /// <summary>
        ///     Paged List of ContactTypes 
        /// </summary>
        /// <param name="jtStartIndex">
        ///     jt Start Index 
        /// </param>
        /// <param name="jtPageSize">
        ///     jt Page Size 
        /// </param>
        /// <param name="jtSorting">
        ///     jt Sorting 
        /// </param>
        /// <returns>
        ///     </returns>
        public IQueryable<VwContactType> PagedList(int jtStartIndex, int jtPageSize, string jtSorting)
        {
            try
            {
                return new ContactTypesRepo(true).PagedList(jtStartIndex, jtPageSize, jtSorting);
            }
            catch (Exception)
            {
                
                throw new DataListException();
            }
        }

        /// <summary>
        ///     Update ContactType 
        /// </summary>
        /// <param name="record">
        ///     record 
        /// </param>
        /// <returns>
        ///     </returns>
        public void Update(VwContactType record)
        {
            try
            {
                new ContactTypesRepo(true).Update(record);
            }
            catch (Exception)
            {
                
                throw new DataUpdateException();
            }
        }

        #endregion Public Methods
    }
}