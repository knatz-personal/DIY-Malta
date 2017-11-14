using BLL.CustomExceptions;
using Common.Views;
using DAL.Repositories.Catalogue;
using System;
using System.Linq;

namespace BLL.Catalogue
{
    /// <summary>
    /// Bl Price Types
    /// </summary>
    public class BlPriceTypes
    {

        /// <summary>
        /// Create
        /// </summary>
        /// <param name="record">record</param>
        /// <returns></returns>
        public VwPriceType Create(VwPriceType record)
        {
            try
            {
                new PriceTypesRepo(true).Create(record);
                return record;
            }
            catch (Exception)
            {
                throw new DataInsertionException();
            }
        }

        /// <summary>
        /// Delete
        /// </summary>
        /// <param name="productId">product</param>
        /// <param name="userType">user Type</param>
        /// <returns></returns>
        public void Delete(Guid productId, int userType)
        {
            try
            {
                new PriceTypesRepo(true).Delete(new VwPriceType { ProductID = productId, UserType = userType });
            }
            catch (Exception)
            {
                throw new DataDeletionException();
            }
        }

        /// <summary>
        /// Get Price Type Details
        /// </summary>
        /// <param name="productId">product</param>
        /// <returns></returns>
        public IQueryable<VwPriceType> GetPriceTypeDetails(Guid productId)
        {
            try
            {
                return new PriceTypesRepo(true).Filter(productId);
            }
            catch (Exception)
            {
                throw new DataListException();
            }
        }

        /// <summary>
        /// Paged List
        /// </summary>
        /// <param name="startIndex">start Index</param>
        /// <param name="pageSize">page Size</param>
        /// <param name="sorting">sorting</param>
        /// <returns></returns>
        public IQueryable<VwPriceType> PagedList(int startIndex, int pageSize, string sorting)
        {
            try
            {
                return new PriceTypesRepo(true).PagedList(startIndex, pageSize, sorting);
            }
            catch (Exception)
            {
                throw new DataListException();
            }
        }
        /// <summary>
        /// Update
        /// </summary>
        /// <param name="record">record</param>
        /// <returns></returns>
        public void Update(VwPriceType record)
        {
            try
            {
                new PriceTypesRepo(true).Update(record);
            }
            catch (Exception)
            {
                throw new DataUpdateException();
            }
        }

    }
}