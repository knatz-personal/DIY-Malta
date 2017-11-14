using System.Collections.Generic;
using Common.EntityModel;
using Common.Views;
using System.Linq;
using System.Linq.Dynamic;

namespace DAL.Repositories
{
    /// <summary>
    ///     Address Types Repository 
    /// </summary>
    public class AddressTypesRepo : AppDbConnection, IDataRepository<VwAddressType>
    {
        #region Public Constructors

        /// <summary>
        ///     Address Types Repository 
        /// </summary>
        /// <param name="isAdministrator">
        ///     is Administrator 
        /// </param>
        /// <returns>
        ///     </returns>
        public AddressTypesRepo(bool isAdministrator)
            : base(isAdministrator)
        {
        }

        #endregion Public Constructors

        #region Public Methods

        /// <summary>
        ///     Create AddressType 
        /// </summary>
        /// <param name="newObject">
        ///     new Object 
        /// </param>
        /// <returns>
        ///     </returns>
        public void Create(VwAddressType newObject)
        {
            Entity.AddressTypes.AddObject(new AddressType
            {
                ID = newObject.ID,
                Name = newObject.Name
            });
            Entity.SaveChanges();
        }

        /// <summary>
        ///     Delete AddressType 
        /// </summary>
        /// <param name="objectToDelete">
        ///     object To Delete 
        /// </param>
        /// <returns>
        ///     </returns>
        public void Delete(VwAddressType objectToDelete)
        {
            AddressType t = GetAddressType(objectToDelete.ID);
            Entity.AddressTypes.DeleteObject(t);
            Entity.SaveChanges();
        }

        /// <summary>
        ///     Get Count 
        /// </summary>
        /// <returns>
        ///     </returns>
        public int GetCount()
        {
            return Entity.AddressTypes.Count();
        }

        /// <summary>
        ///     List All 
        /// </summary>
        /// <returns>
        ///     </returns>
        public IQueryable<VwAddressType> ListAll()
        {
            return (from g in Entity.AddressTypes
                    select new VwAddressType
                    {
                        ID = g.ID,
                        Name = g.Name
                    })
                ;
        }

        /// <summary>
        ///     Paged List of AddressType 
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
        public IQueryable<VwAddressType> PagedList(int startIndex, int pageSize, string sorting)
        {
            IQueryable<VwAddressType> result = ListAll().OrderBy(sorting).Skip(startIndex).Take(pageSize);
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
        public VwAddressType Read(object key)
        {
            var AddressType = new VwAddressType();
            var g = new AddressType();
            if (key is int)
            {
                g = Entity.AddressTypes.SingleOrDefault(t => t.ID == (int)key);
            }
            else
            {
                var s = key as string;
                if (s != null)
                {
                    g = Entity.AddressTypes.SingleOrDefault(t => t.Name.Equals(s));
                }
            }
            AddressType.ID = g.ID;
            AddressType.Name = g.Name;

            return AddressType;
        }

        /// <summary>
        ///     Update AddressType 
        /// </summary>
        /// <param name="objectToUpdate">
        ///     object To Update 
        /// </param>
        /// <returns>
        ///     </returns>
        public void Update(VwAddressType objectToUpdate)
        {
            AddressType AddressType = GetAddressType(objectToUpdate.ID);
            AddressType.Name = objectToUpdate.Name;
            Entity.AddressTypes.ApplyCurrentValues(AddressType);
            Entity.SaveChanges();
        }

        #endregion Public Methods

        #region Private Methods

        /// <summary>
        ///     Get Address Type 
        /// </summary>
        /// <param name="id">
        ///     id 
        /// </param>
        /// <returns>
        ///     </returns>
        private AddressType GetAddressType(int id)
        {
            return Entity.AddressTypes.SingleOrDefault(e => e.ID == id);
        }

        #endregion Private Methods
    }
}