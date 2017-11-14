using System.Collections.Generic;
using Common.EntityModel;
using Common.Views;
using System;
using System.Linq;

namespace DAL.Repositories
{
    /// <summary>
    ///     Contacts Repository 
    /// </summary>
    public class ContactsRepo : AppDbConnection, IDataRepository<VwContact>
    {
        #region Public Constructors

        /// <summary>
        ///     Contacts Repository 
        /// </summary>
        /// <param name="isAdministrator">
        ///     is Administrator 
        /// </param>
        /// <returns>
        ///     </returns>
        public ContactsRepo(bool isAdministrator)
            : base(isAdministrator)
        {
        }

        #endregion Public Constructors

        #region Public Methods

        /// <summary>
        ///     Create Contact 
        /// </summary>
        /// <param name="newObject">
        ///     new Object 
        /// </param>
        /// <returns>
        ///     </returns>
        public void Create(VwContact newObject)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        ///     Delete Contact 
        /// </summary>
        /// <param name="objectToDelete">
        ///     object To Delete 
        /// </param>
        /// <returns>
        ///     </returns>
        public void Delete(VwContact objectToDelete)
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
        public IQueryable<VwContact> ListAll()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        ///     Paged List of Contacts 
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
        public IQueryable<VwContact> PagedList(int startIndex, int pageSize, string sorting)
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
        public VwContact Read(object key)
        {
            if (key is int)
            {
                Contact t = Entity.Contacts.SingleOrDefault(c => c.ID.Equals((int) key));
                return new VwContact
                {
                    ID = t.ID,
                    TypeID = t.TypeID,
                    Phone = t.Phone,
                    Mobile = t.Mobile,
                    Email = t.Email
                };
            }
            return null;
        }

        /// <summary>
        ///     Update Contact 
        /// </summary>
        /// <param name="objectToUpdate">
        ///     object To Update 
        /// </param>
        /// <returns>
        ///     </returns>
        public void Update(VwContact objectToUpdate)
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
        public IQueryable<VwContact> Filter(string query)
        {
            IQueryable<VwContact> t = (from c in Entity.Contacts
                from u in c.Users
                where u.Username.Equals(query)
                select new VwContact
                {
                    ID = c.ID,
                    TypeID = c.TypeID,
                    Phone = c.Phone,
                    Mobile = c.Mobile,
                    Email = c.Email
                }
                );
            return t;
        }

        /// <summary>
        ///     List Contact Types 
        /// </summary>
        /// <returns>
        ///     </returns>
        public IQueryable<VwContactType> ListTypes()
        {
            return (from ct in Entity.ContactTypes
                select new VwContactType
                {
                    ID = ct.ID,
                    Name = ct.Name
                }
                )
                ;
        }

        #endregion Public Methods
    }
}