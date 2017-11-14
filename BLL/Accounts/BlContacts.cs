using Common.Views;
using DAL.Repositories;
using System.Linq;

namespace BLL.Accounts
{
    /// <summary>
    ///     Contacts business logic 
    /// </summary>
    public class BlContacts
    {
        #region Public Methods

        /// <summary>
        ///     Get Contact Details 
        /// </summary>
        /// <param name="username">
        ///     username 
        /// </param>
        /// <returns>
        ///     </returns>
        public IQueryable<VwContact> GetContactDetails(string username)
        {
            return new ContactsRepo(true).Filter(username);
        }

        /// <summary>
        ///     List Contact Types 
        /// </summary>
        /// <returns>
        ///     </returns>
        public IQueryable<VwContactType> ListTypes()
        {
            return new ContactsRepo(false).ListTypes();
        }

        #endregion Public Methods
    }
}