using System.Collections.Generic;
using Common.Views;
using System.Linq;
using System.Linq.Dynamic;

namespace DAL.Repositories.Accounts
{
    /// <summary>
    ///     User Roles Repository 
    /// </summary>
    public class UserRolesRepo : AppDbConnection, IDataRepository<VwUserRoles>
    {
        #region Public Constructors

        /// <summary>
        ///     User Roles Repository 
        /// </summary>
        /// <param name="isAdministrator">
        ///     is Administrator 
        /// </param>
        /// <returns>
        ///     </returns>
        public UserRolesRepo(bool isAdministrator)
            : base(isAdministrator)
        {
        }

        #endregion Public Constructors

        #region Public Methods

        /// <summary>
        ///     Create UserRoles 
        /// </summary>
        /// <param name="newObject">
        ///     new Object 
        /// </param>
        /// <returns>
        ///     </returns>
        public void Create(VwUserRoles newObject)
        {
            throw new System.NotImplementedException();
        }

        /// <summary>
        ///     Delete UserRoles 
        /// </summary>
        /// <param name="objectToDelete">
        ///     object To Delete 
        /// </param>
        /// <returns>
        ///     </returns>
        public void Delete(VwUserRoles objectToDelete)
        {
            throw new System.NotImplementedException();
        }

        /// <summary>
        ///     Filter 
        /// </summary>
        /// <param name="query">
        ///     query 
        /// </param>
        /// <returns>
        ///     </returns>
        public IQueryable<VwUserRoles> Filter(string query)
        {
            var result = (from u in Entity.Users
                          from r in u.Roles
                          where u.Username.Equals(query)
                          select new VwUserRoles()
                          {
                              Username = u.Username,
                              RoleID = r.ID
                          });
            return result;
        }

        /// <summary>
        ///     Get Count 
        /// </summary>
        /// <returns>
        ///     </returns>
        public int GetCount()
        {
            var t = (from e in Entity.Users
                     from r in e.Roles
                     select e.Roles.Count
                         );
            return t.Sum();
        }

        /// <summary>
        ///     List All 
        /// </summary>
        /// <returns>
        ///     </returns>
        public IQueryable<VwUserRoles> ListAll()
        {
            return (from t in Entity.Users
                    from r in t.Roles
                    select new VwUserRoles()
                    {
                        Username = t.Username,
                        RoleID = r.ID
                    });
        }

        /// <summary>
        ///     Paged List of UserRoles 
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
        public IQueryable<VwUserRoles> PagedList(int startIndex, int pageSize, string sorting)
        {
            var result = ListAll().OrderBy(sorting).Skip(startIndex).Take(pageSize);
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
        public VwUserRoles Read(object key)
        {
            throw new System.NotImplementedException();
        }

        /// <summary>
        ///     Update UserRoles 
        /// </summary>
        /// <param name="objectToUpdate">
        ///     object To Update 
        /// </param>
        /// <returns>
        ///     </returns>
        public void Update(VwUserRoles objectToUpdate)
        {
            throw new System.NotImplementedException();
        }

        #endregion Public Methods
    }
}