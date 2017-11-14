using Common.Views;
using DAL.Repositories;
using System.Linq;

namespace BLL.Accounts
{
    /// <summary>
    ///     Addresses business logic 
    /// </summary>
    public class BlAddresses
    {
        #region Public Methods

        /// <summary>
        ///     Get Address Details 
        /// </summary>
        /// <param name="username">
        ///     username 
        /// </param>
        /// <returns>
        ///     </returns>
        public object GetAddressDetails(string username)
        {
            return new AddressRepo(true).Filter(username);
        }

        /// <summary>
        ///     List Address Types 
        /// </summary>
        /// <returns>
        ///     </returns>
        public IQueryable<VwAddressType> ListTypes()
        {
            return new AddressRepo(false).ListTypes();
        }

        #endregion Public Methods
    }
}