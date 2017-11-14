using System.Collections.Generic;
using Common.EntityModel;
using Common.Views;
using System.Linq;
using System.Linq.Dynamic;

namespace DAL.Repositories
{
    /// <summary>
    ///     User Types Repository 
    /// </summary>
    public class UserTypesRepo : AppDbConnection, IDataRepository<VwUserType>
    {
        #region Public Constructors

        /// <summary>
        ///     User Types Repository 
        /// </summary>
        /// <param name="isAdministrator">
        ///     is Administrator 
        /// </param>
        /// <returns>
        ///     </returns>
        public UserTypesRepo(bool isAdministrator)
            : base(isAdministrator)
        {
        }

        #endregion Public Constructors

        #region Public Methods

        /// <summary>
        ///     Create UserType 
        /// </summary>
        /// <param name="newObject">
        ///     new Object 
        /// </param>
        /// <returns>
        ///     </returns>
        public void Create(VwUserType newObject)
        {
            if (newObject.ID != null)
                Entity.UserTypes.AddObject(new UserType
                {
                    ID = (int)newObject.ID,
                    Name = newObject.Name
                });
            Entity.SaveChanges();
        }

        /// <summary>
        ///     Delete UserType 
        /// </summary>
        /// <param name="objectToDelete">
        ///     object To Delete 
        /// </param>
        /// <returns>
        ///     </returns>
        public void Delete(VwUserType objectToDelete)
        {
            UserType t = GetUserType(objectToDelete.ID);
            Entity.UserTypes.DeleteObject(t);
            Entity.SaveChanges();
        }

        /// <summary>
        ///     Get Count 
        /// </summary>
        /// <returns>
        ///     </returns>
        public int GetCount()
        {
            return Entity.UserTypes.Count();
        }

        /// <summary>
        ///     List All 
        /// </summary>
        /// <returns>
        ///     </returns>
        public IQueryable<VwUserType> ListAll()
        {
            return (from g in Entity.UserTypes
                    select new VwUserType
                    {
                        ID = g.ID,
                        Name = g.Name
                    })
                ;
        }

        /// <summary>
        ///     Paged List of UserTypes 
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
        public IQueryable<VwUserType> PagedList(int startIndex, int pageSize, string sorting)
        {
            IQueryable<VwUserType> result = ListAll().OrderBy(sorting).Skip(startIndex).Take(pageSize);
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
        public VwUserType Read(object key)
        {
            var UserType = new VwUserType();
            var g = new UserType();
            if (key is int)
            {
                g = Entity.UserTypes.SingleOrDefault(t => t.ID == (int)key);
            }
            else
            {
                var s = key as string;
                if (s != null)
                {
                    g = Entity.UserTypes.SingleOrDefault(t => t.Name.Equals(s));
                }
            }
            UserType.ID = g.ID;
            UserType.Name = g.Name;

            return UserType;
        }

        /// <summary>
        ///     Update UserType 
        /// </summary>
        /// <param name="objectToUpdate">
        ///     object To Update 
        /// </param>
        /// <returns>
        ///     </returns>
        public void Update(VwUserType objectToUpdate)
        {
            UserType UserType = GetUserType(objectToUpdate.ID);
            UserType.Name = objectToUpdate.Name;
            Entity.UserTypes.ApplyCurrentValues(UserType);
            Entity.SaveChanges();
        }

        #endregion Public Methods

        #region Private Methods

        /// <summary>
        ///     Get User Type 
        /// </summary>
        /// <param name="id">
        ///     id 
        /// </param>
        /// <returns>
        ///     </returns>
        private UserType GetUserType(int? id)
        {
            return Entity.UserTypes.SingleOrDefault(e => e.ID == id);
        }

        #endregion Private Methods
    }
}