using System;
using System.Collections.Generic;
using System.Data.Objects;
using System.Linq;
using System.Linq.Dynamic;
using Common.EntityModel;
using Common.Views;

namespace DAL.Repositories.Catalogue
{
    /// <summary>
    ///     Shopping Carts Repository
    /// </summary>
    public class ShoppingCartsRepo : AppDbConnection, IDataRepository<VwShoppingCart>
    {

        #region Public Constructors

        /// <summary>
        ///     Shopping Carts Repository
        /// </summary>
        /// <param name="isAdministrator">
        ///     is Administrator
        /// </param>
        /// <returns>
        /// </returns>
        public ShoppingCartsRepo(bool isAdministrator)
            : base(isAdministrator)
        {
        }

        #endregion Public Constructors

        #region Public Methods

        /// <summary>
        ///     Create ShoppingCart
        /// </summary>
        /// <param name="newObject">
        ///     new Object
        /// </param>
        /// <returns>
        /// </returns>
        public void Create(VwShoppingCart newObject)
        {
            Entity.ShoppingCarts.AddObject(new ShoppingCart
            {
                ID = newObject.ID,
                Username = newObject.Username,
                ProductID = newObject.ProductID,
                Quantity = newObject.Quantity
            });
            Entity.SaveChanges();
        }

        /// <summary>
        ///     Delete ShoppingCart
        /// </summary>
        /// <param name="objectToDelete">
        ///     object To Delete
        /// </param>
        /// <returns>
        /// </returns>
        public void Delete(VwShoppingCart objectToDelete)
        {
            ShoppingCart t = GetShoppingCart(objectToDelete.ID);
            Entity.ShoppingCarts.DeleteObject(t);
            Entity.SaveChanges();
        }

        /// <summary>
        ///     Get Count
        /// </summary>
        /// <returns>
        /// </returns>
        public int GetCount()
        {
            return Entity.ShoppingCarts.Count();
        }

        /// <summary>
        ///     List All
        /// </summary>
        /// <returns>
        /// </returns>
        public IQueryable<VwShoppingCart> ListAll()
        {
            return (from t in Entity.ShoppingCarts
                select new VwShoppingCart
                {
                    ID = t.ID,
                    Username = t.Username,
                    ProductID = t.ProductID,
                    Quantity = t.Quantity,
                    Name = t.Product.Name,
                    Category = t.Product.Category.Name
                });
        }

        /// <summary>
        ///     List All
        /// </summary>
        /// <param name="username">
        ///     username
        /// </param>
        /// <returns>
        /// </returns>
        public IQueryable<VwShoppingCart> ListAll(string username)
        {
            int? userTypeId = (from u in Entity.Users
                               where u.Username == username
                               select u.TypeID
                ).FirstOrDefault();
            
            return (from t in Entity.ShoppingCarts
                    from p in Entity.Products
                    from a in t.Product.PriceTypes
                    where t.Username.Equals(username) && a.UserTypeID == userTypeId && p.ID == t.ProductID
                    select new VwShoppingCart
                    {
                        ID = t.ID,
                        Username = t.Username,
                        ProductID = t.ProductID,
                        Quantity = t.Quantity,
                        Name = t.Product.Name,
                        Category = t.Product.Category.Name,
                        Stock = t.Product.Stock,
                        UnitPrice = a.UnitPrice,
                        DiscountPrice = a.UnitPrice * (p.SpecialSale != null && (EntityFunctions.TruncateTime(DateTime.Now) <
                         EntityFunctions.TruncateTime(p.SpecialSale.DateExpired)) ? ((100 - p.SpecialSale.Discount) / 100) : 0),
                        TotalPrice = a.UnitPrice * (p.SpecialSale != null && (EntityFunctions.TruncateTime(DateTime.Now) <
                         EntityFunctions.TruncateTime(p.SpecialSale.DateExpired)) ? ((100 - p.SpecialSale.Discount) / 100) : 1) * t.Quantity,
                        Image = t.Product.ImagePath,
                        Tax = t.Product.VAT 
                    }).OrderBy(o=>o.Name);
        }

        public decimal Tax2 { get; set; }


        /// <summary>
        ///     Paged List of ShoppingCarts
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
        /// </returns>
        public IQueryable<VwShoppingCart> PagedList(int startIndex, int pageSize, string sorting)
        {
            IQueryable<VwShoppingCart> result = ListAll().OrderBy(sorting).Skip(startIndex).Take(pageSize);
            return result;
        }

        /// <summary>
        ///     Read
        /// </summary>
        /// <param name="key">
        ///     key
        /// </param>
        /// <returns>
        /// </returns>
        public VwShoppingCart Read(object key)
        {
            var id = (Guid) key;
            ShoppingCart t = GetShoppingCart(id);
            return new VwShoppingCart
            {
                ID = t.ID,
                Username = t.Username,
                ProductID = t.ProductID,
                Quantity = t.Quantity
            };
        }

        /// <summary>
        ///     Update ShoppingCart
        /// </summary>
        /// <param name="objectToUpdate">
        ///     object To Update
        /// </param>
        /// <returns>
        /// </returns>
        public void Update(VwShoppingCart objectToUpdate)
        {
            ShoppingCart t = GetShoppingCart(objectToUpdate.ID);
            t.Quantity = objectToUpdate.Quantity;
            Entity.ShoppingCarts.ApplyCurrentValues(t);
            Entity.SaveChanges();
        }

        #endregion Public Methods

        #region Private Methods

        /// <summary>
        ///     Get Shopping Cart
        /// </summary>
        /// <param name="id">
        ///     id
        /// </param>
        /// <returns>
        /// </returns>
        private ShoppingCart GetShoppingCart(Guid id)
        {
            return Entity.ShoppingCarts.SingleOrDefault(e => e.ID == id);
        }

        #endregion Private Methods

    }
}