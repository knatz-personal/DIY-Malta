using System.Collections.Generic;
using Common.EntityModel;
using Common.Views;
using System.Linq;
using System.Linq.Dynamic;

namespace DAL.Repositories
{
    /// <summary>
    ///     Contact Types Repository 
    /// </summary>
    public class ContactTypesRepo : AppDbConnection, IDataRepository<VwContactType>
    {
        #region Public Constructors

        /// <summary>
        ///     Contact Types Repository 
        /// </summary>
        /// <param name="isAdministrator">
        ///     is Administrator 
        /// </param>
        /// <returns>
        ///     </returns>
        public ContactTypesRepo(bool isAdministrator)
            : base(isAdministrator)
        {
        }

        #endregion Public Constructors

        #region Public Methods

        /// <summary>
        ///     Create Contact Type 
        /// </summary>
        /// <param name="newObject">
        ///     new Object 
        /// </param>
        /// <returns>
        ///     </returns>
        public void Create(VwContactType newObject)
        {
            Entity.ContactTypes.AddObject(new ContactType
            {
                ID = newObject.ID,
                Name = newObject.Name
            });
            Entity.SaveChanges();
        }

        /// <summary>
        ///     Delete Contact Type 
        /// </summary>
        /// <param name="objectToDelete">
        ///     object To Delete 
        /// </param>
        /// <returns>
        ///     </returns>
        public void Delete(VwContactType objectToDelete)
        {
            ContactType t = GetContactType(objectToDelete.ID);
            Entity.ContactTypes.DeleteObject(t);
            Entity.SaveChanges();
        }

        /// <summary>
        ///     Get Count 
        /// </summary>
        /// <returns>
        ///     </returns>
        public int GetCount()
        {
            return Entity.ContactTypes.Count();
        }

        /// <summary>
        ///     List All 
        /// </summary>
        /// <returns>
        ///     </returns>
        public IQueryable<VwContactType> ListAll()
        {
            return (from g in Entity.ContactTypes
                    select new VwContactType
                    {
                        ID = g.ID,
                        Name = g.Name
                    })
                ;
        }

        /// <summary>
        ///     Paged List of Contact Type 
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
        public IQueryable<VwContactType> PagedList(int startIndex, int pageSize, string sorting)
        {
            IQueryable<VwContactType> result = ListAll().OrderBy(sorting).Skip(startIndex).Take(pageSize);
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
        public VwContactType Read(object key)
        {
            var ContactType = new VwContactType();
            var g = new ContactType();
            if (key is int)
            {
                g = Entity.ContactTypes.SingleOrDefault(t => t.ID == (int)key);
            }
            else
            {
                var s = key as string;
                if (s != null)
                {
                    g = Entity.ContactTypes.SingleOrDefault(t => t.Name.Equals(s));
                }
            }
            ContactType.ID = g.ID;
            ContactType.Name = g.Name;

            return ContactType;
        }

        /// <summary>
        ///     Update Contact Type 
        /// </summary>
        /// <param name="objectToUpdate">
        ///     object To Update 
        /// </param>
        /// <returns>
        ///     </returns>
        public void Update(VwContactType objectToUpdate)
        {
            ContactType ContactType = GetContactType(objectToUpdate.ID);
            ContactType.Name = objectToUpdate.Name;
            Entity.ContactTypes.ApplyCurrentValues(ContactType);
            Entity.SaveChanges();
        }

        #endregion Public Methods

        #region Private Methods

        /// <summary>
        ///     Get Contact Type 
        /// </summary>
        /// <param name="id">
        ///     id 
        /// </param>
        /// <returns>
        ///     </returns>
        private ContactType GetContactType(int id)
        {
            return Entity.ContactTypes.SingleOrDefault(e => e.ID == id);
        }

        #endregion Private Methods
    }
}