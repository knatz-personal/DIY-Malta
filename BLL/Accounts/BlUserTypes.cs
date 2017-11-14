using System;
using BLL.CustomExceptions;
using Common.Views;
using DAL.Repositories;
using System.Linq;

namespace BLL.Accounts
{
    /// <summary>
    ///     User Types business logic 
    /// </summary>
    public class BlUserTypes
    {
        #region Public Methods

        /// <summary>
        ///     Create User Type 
        /// </summary>
        /// <param name="name">
        ///     name 
        /// </param>
        /// <returns>
        ///     </returns>
        public VwUserType Create(string name)
        {
            try
            {
                var tr = new UserTypesRepo(true);
                tr.Create(new VwUserType { Name = name });
                VwUserType t = tr.Read(name);
                return t;
            }
            catch (Exception)
            {
                throw new DataInsertionException();
            }
        }

        /// <summary>
        ///     Delete User Type 
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
                var gr = new UserTypesRepo(true);
                VwUserType g = gr.Read(id);
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
            return new UserTypesRepo(true).GetCount();
        }

        /// <summary>
        ///     List All 
        /// </summary>
        /// <returns>
        ///     </returns>
        public IQueryable<VwUserType> ListAll()
        {
            try
            {
                return new UserTypesRepo(false).ListAll();
            }
            catch (Exception)
            {
                throw new DataListException();
            }
        }

        /// <summary>
        ///     Paged List of User Types 
        /// </summary>
        /// <param name="startIndex">
        ///     jt Start Index 
        /// </param>
        /// <param name="pageSize">
        ///     jt Page Size 
        /// </param>
        /// <param name="sorting">
        ///     jt Sorting 
        /// </param>
        /// <returns>
        ///     </returns>
        public IQueryable<VwUserType> PagedList(int startIndex, int pageSize, string sorting)
        {
            try
            {
                return new UserTypesRepo(true).PagedList(startIndex, pageSize, sorting);
            }
            catch (Exception)
            {
                throw new DataListException();
            }
        }

        /// <summary>
        ///     Update User Type 
        /// </summary>
        /// <param name="record">
        ///     record 
        /// </param>
        /// <returns>
        ///     </returns>
        public void Update(VwUserType record)
        {
            try
            {
                new UserTypesRepo(true).Update(record);
            }
            catch (Exception)
            {
                throw new DataUpdateException();
            }
        }

        #endregion Public Methods
    }
}