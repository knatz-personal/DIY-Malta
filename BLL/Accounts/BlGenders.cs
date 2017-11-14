using System;
using BLL.CustomExceptions;
using Common.Views;
using DAL.Repositories;
using System.Linq;

namespace BLL.Accounts
{
    /// <summary>
    ///     Genders business logic 
    /// </summary>
    public class BlGenders
    {
        #region Public Methods

        /// <summary>
        ///     Create Gender 
        /// </summary>
        /// <param name="name">
        ///     name 
        /// </param>
        /// <returns>
        ///     </returns>
        public VwGender Create(string name)
        {
            try
            {
                var tr = new GendersRepo(true);
                tr.Create(new VwGender { Name = name });
                VwGender t = tr.Read(name);
                return t;
            }
            catch (Exception)
            {
                throw new DataInsertionException();
            }
        }

        /// <summary>
        ///     Delete Gender 
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
                var gr = new GendersRepo(true);
                VwGender g = gr.Read(id);
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
            return new GendersRepo(true).GetCount();
        }

        /// <summary>
        ///     List All 
        /// </summary>
        /// <returns>
        ///     </returns>
        public IQueryable<VwGender> ListAll()
        {
            try
            {
                return new GendersRepo(false).ListAll();
            }
            catch (Exception)
            {
                
                throw new DataListException();
            }
        }

        /// <summary>
        ///     Paged List of Genders 
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
        public IQueryable<VwGender> PagedList(int jtStartIndex, int jtPageSize, string jtSorting)
        {
            try
            {
                return new GendersRepo(true).PagedList(jtStartIndex, jtPageSize, jtSorting);
            }
            catch (Exception)
            {
                
                throw new DataListException();
            }
        }

        /// <summary>
        ///     Update Gender 
        /// </summary>
        /// <param name="record">
        ///     record 
        /// </param>
        /// <returns>
        ///     </returns>
        public void Update(VwGender record)
        {
            try
            {
                new GendersRepo(true).Update(record);
            }
            catch (Exception)
            {
                
                throw new DataUpdateException();
            }
        }

        #endregion Public Methods
    }
}