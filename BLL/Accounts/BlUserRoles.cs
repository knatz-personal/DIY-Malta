using System;
using BLL.CustomExceptions;
using Common.Views;
using DAL.Repositories.Accounts;
using System.Linq;

namespace BLL.Accounts
{
    /// <summary>
    ///     User Roles business logic 
    /// </summary>
    public class BlUserRoles
    {
        #region Public Methods

        /// <summary>
        ///     Filter 
        /// </summary>
        /// <param name="username">
        ///     username 
        /// </param>
        /// <returns>
        ///     </returns>
        public IQueryable<VwUserRoles> Filter(string username)
        {
            return new UserRolesRepo(true).Filter(username);
        }

        /// <summary>
        ///     Paged List of user roles 
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
            try
            {
                return new UserRolesRepo(true).PagedList(startIndex, pageSize, sorting);
            }
            catch (Exception)
            {
                throw new DataListException();
            }
        }

        #endregion Public Methods
    }
}