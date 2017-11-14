using System.Collections.Generic;
using Common.EntityModel;
using Common.Views;
using System;
using System.Linq;

namespace DAL.Repositories
{
    /// <summary>
    ///     AddressRepository
    /// </summary>
    public class AddressRepo : AppDbConnection, IDataRepository<VwAddress>
    {
        #region Public Constructors

        /// <summary>
        ///     AddressRepository
        /// </summary>
        /// <param name="isAdministrator">
        ///     is Administrator 
        /// </param>
        /// <returns>
        ///     </returns>
        public AddressRepo(bool isAdministrator)
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
        public void Create(VwAddress newObject)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        ///     Delete 
        /// </summary>
        /// <param name="objectToDelete">
        ///     object To Delete 
        /// </param>
        /// <returns>
        ///     </returns>
        public void Delete(VwAddress objectToDelete)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        ///     Get Count 
        /// </summary>
        /// <returns>
        ///     </returns>
        public int GetCount()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        ///     List All 
        /// </summary>
        /// <returns>
        ///     </returns>
        public IQueryable<VwAddress> ListAll()
        {
            throw new NotImplementedException();
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
        public IQueryable<VwAddress> PagedList(int startIndex, int pageSize, string sorting)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        ///     Read 
        /// </summary>
        /// <param name="key">
        ///     key 
        /// </param>
        /// <returns>
        ///     </returns>
        public VwAddress Read(object key)
        {
            if (key is int)
            {
                Address t = Entity.Addresses.SingleOrDefault(a => a.ID.Equals((int) key));
                return new VwAddress
                {
                    ID = t.ID,
                    TypeID = t.TypeID,
                    Residence = t.Residence,
                    Street = t.Street,
                    TownID = t.TownID,
                    PostCode = t.PostCode
                };
            }
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
        public void Update(VwAddress objectToUpdate)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        ///     Filter 
        /// </summary>
        /// <param name="query">
        ///     query 
        /// </param>
        /// <returns>
        ///     </returns>
        public IQueryable<VwAddress> Filter(string query)
        {
            IQueryable<VwAddress> c = (from t in Entity.Addresses
                from u in t.Users
                where u.Username.Equals(query)
                select new VwAddress
                {
                    ID = t.ID,
                    TypeID = t.TypeID,
                    Residence = t.Residence,
                    Street = t.Street,
                    TownID = t.TownID,
                    PostCode = t.PostCode
                }
                )
                ;
            return c;
        }

        /// <summary>
        ///     List Types 
        /// </summary>
        /// <returns>
        ///     </returns>
        public IQueryable<VwAddressType> ListTypes()
        {
            return (from at in Entity.AddressTypes
                select new VwAddressType
                {
                    ID = at.ID,
                    Name = at.Name
                }
                )
                ;
        }

        #endregion Public Methods
    }
}