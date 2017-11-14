using System.Collections.Generic;
using Common.EntityModel;
using Common.Views;
using System;
using System.Linq;
using System.Linq.Dynamic;

namespace DAL.Repositories.Catalogue
{
    /// <summary>
    ///     Price Types Repository 
    /// </summary>
    public class PriceTypesRepo : AppDbConnection, IDataRepository<VwPriceType>
    {
        #region Public Constructors

        /// <summary>
        ///     Price Types Repository 
        /// </summary>
        /// <param name="isAdministrator">
        ///     is Administrator 
        /// </param>
        /// <returns>
        ///     </returns>
        public PriceTypesRepo(bool isAdministrator)
            : base(isAdministrator)
        {
        }

        #endregion Public Constructors

        #region Public Methods

        /// <summary>
        ///     Create PriceType 
        /// </summary>
        /// <param name="newObject">
        ///     new Object 
        /// </param>
        /// <returns>
        ///     </returns>
        public void Create(VwPriceType newObject)
        {
            Entity.PriceTypes.AddObject(new PriceType()
            {
                ProductID = newObject.ProductID,
                UserTypeID = newObject.UserType,
                UnitPrice = newObject.UnitPrice
            });
            Entity.SaveChanges();
        }

        /// <summary>
        ///     Delete PriceType 
        /// </summary>
        /// <param name="objectToDelete">
        ///     object To Delete 
        /// </param>
        /// <returns>
        ///     </returns>
        public void Delete(VwPriceType objectToDelete)
        {
            var t = GetPriceType(objectToDelete.ProductID, objectToDelete.UserType);
            Entity.PriceTypes.DeleteObject(t);
            Entity.SaveChanges();
        }

        /// <summary>
        ///     Filter 
        /// </summary>
        /// <param name="productId">
        ///     product 
        /// </param>
        /// <returns>
        ///     </returns>
        public IQueryable<VwPriceType> Filter(Guid productId)
        {
            var result = (Entity.PriceTypes.Where(p => p.ProductID == productId)).Select(e =>
                new VwPriceType()
                {
                    ProductID = e.ProductID,
                    UserType = e.UserTypeID,
                    UnitPrice = e.UnitPrice
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
            return Entity.PriceTypes.Count();
        }

        /// <summary>
        ///     List All 
        /// </summary>
        /// <returns>
        ///     </returns>
        public IQueryable<VwPriceType> ListAll()
        {
            return (from p in Entity.PriceTypes
                    select new VwPriceType()
                    {
                        ProductID = p.ProductID,
                        UserType = p.UserTypeID,
                        UnitPrice = p.UnitPrice
                    });
        }

        /// <summary>
        ///     Paged List of PriceTypes 
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
        public IQueryable<VwPriceType> PagedList(int startIndex, int pageSize, string sorting)
        {
            IQueryable<VwPriceType> result = ListAll().OrderBy(sorting).Skip(startIndex).Take(pageSize);
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
        public VwPriceType Read(object key)
        {
            throw new System.NotImplementedException();
        }

        /// <summary>
        ///     Update PriceType 
        /// </summary>
        /// <param name="objectToUpdate">
        ///     object To Update 
        /// </param>
        /// <returns>
        ///     </returns>
        public void Update(VwPriceType objectToUpdate)
        {
            var t = GetPriceType(objectToUpdate.ProductID, objectToUpdate.UserType);
            t.UnitPrice = objectToUpdate.UnitPrice;
            Entity.PriceTypes.ApplyCurrentValues(t);
            Entity.SaveChanges();
        }

        #endregion Public Methods

        #region Private Methods

        /// <summary>
        ///     Get Price Type 
        /// </summary>
        /// <param name="productId">
        ///     product 
        /// </param>
        /// <param name="userType">
        ///     user Type 
        /// </param>
        /// <returns>
        ///     </returns>
        private PriceType GetPriceType(Guid productId, int userType)
        {
            return Entity.PriceTypes.SingleOrDefault(p => p.ProductID == productId && p.UserTypeID == userType);
        }

        #endregion Private Methods
    }
}