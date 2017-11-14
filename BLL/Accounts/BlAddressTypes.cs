using System;
using BLL.CustomExceptions;
using Common.Views;
using DAL.Repositories;
using System.Linq;

namespace BLL.Accounts
{
    /// <summary>
    ///     Address Types business logic 
    /// </summary>
    public class BlAddressTypes
    {
        #region Public Methods

        /// <summary>
        ///     Create AddressType 
        /// </summary>
        /// <param name="name">
        ///     name 
        /// </param>
        /// <returns>
        ///     </returns>
        public VwAddressType Create(string name)
        {
            try
            {
                var tr = new AddressTypesRepo(true);
                tr.Create(new VwAddressType { Name = name });
                VwAddressType t = tr.Read(name);
                return t;
            }
            catch (Exception)
            {
                throw new DataInsertionException();
            }
        }

        /// <summary>
        ///     Delete AddressType 
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
                var gr = new AddressTypesRepo(true);
                VwAddressType g = gr.Read(id);
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
            return new AddressTypesRepo(true).GetCount();
        }

        /// <summary>
        ///     List All 
        /// </summary>
        /// <returns>
        ///     </returns>
        public IQueryable<VwAddressType> ListAll()
        {
            try
            {
                return new AddressTypesRepo(false).ListAll();
            }
            catch (Exception)
            {
                
                throw new DataListException();
            }
        }

        /// <summary>
        ///     Paged List of AddressTypes 
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
        public IQueryable<VwAddressType> PagedList(int jtStartIndex, int jtPageSize, string jtSorting)
        {
            try
            {
                return new AddressTypesRepo(true).PagedList(jtStartIndex, jtPageSize, jtSorting);
            }
            catch (Exception)
            {
                
                throw new DataListException();
            }
        }

        /// <summary>
        ///     Update AddressType 
        /// </summary>
        /// <param name="record">
        ///     record 
        /// </param>
        /// <returns>
        ///     </returns>
        public void Update(VwAddressType record)
        {
            try
            {
                new AddressTypesRepo(true).Update(record);
            }
            catch (Exception)
            {
                
                throw new DataUpdateException();
            }
        }

        #endregion Public Methods
    }
}