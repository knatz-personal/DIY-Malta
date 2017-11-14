using System;
using System.Data.Objects;
using System.Linq;
using System.Linq.Dynamic;
using Common.EntityModel;
using Common.Views;
using DAL.Repositories.Catalogue;

namespace DAL.Repositories
{
    public class ProductsRepo : AppDbConnection, IDataRepository<VwProduct>
    {
        #region Public Constructors

        public ProductsRepo(bool isAdministrator)
            : base(isAdministrator)
        {
        }

        #endregion Public Constructors

        #region Public Methods

        public void Create(VwProduct newObject)
        {
            Entity.Products.AddObject(new Product
            {
                ID = newObject.ID,
                Name = newObject.Name,
                Description = newObject.Description,
                ImagePath = newObject.Image,
                CategoryID = newObject.CategoryID,
                Stock = newObject.Stock,
                SaleID = newObject.SaleID,
                VAT = newObject.VAT,
                Active = newObject.IsActive
            });
            Entity.SaveChanges();
        }

        public void Delete(VwProduct objectToDelete)
        {
            Product c = GetProduct(objectToDelete.ID);

            c.Active = false;

            Entity.Products.ApplyCurrentValues(c);
            Entity.SaveChanges();
        }

        public IQueryable<VwProduct> ListAll()
        {
            return (from p in Entity.Products
                select new VwProduct
                {
                    ID = p.ID,
                    Image = p.ImagePath,
                    Name = p.Name,
                    CategoryID = p.CategoryID,
                    Stock = p.Stock,
                    SaleID = p.SaleID,
                    Description = p.Description,
                    IsActive = p.Active,
                    VAT = p.VAT
                });
        }

        public IQueryable<VwProduct> PagedList(int startIndex, int pageSize, string sorting)
        {
            IQueryable<VwProduct> result = ListAll().OrderBy(sorting).Skip(startIndex).Take(pageSize);
            return result;
        }

        public VwProduct Read(object key)
        {
            VwProduct result = (from p in Entity.Products
                where p.ID == (Guid) key
                select new VwProduct
                {
                    ID = p.ID,
                    Image = p.ImagePath,
                    Name = p.Name,
                    Stock = p.Stock,
                    SaleID = p.SaleID,
                    CategoryID = p.CategoryID,
                    Discount =
                        p.SpecialSale != null &&
                        (EntityFunctions.TruncateTime(DateTime.Now) <
                         EntityFunctions.TruncateTime(p.SpecialSale.DateExpired))
                            ? p.SpecialSale.Discount
                            : 0,
                    Description = p.Description,
                    IsActive = p.Active,
                    VAT = p.VAT
                }).FirstOrDefault();
            return result;
        }

        public void Update(VwProduct objectToUpdate)
        {
            Product c = GetProduct(objectToUpdate.ID);

            c.Name = objectToUpdate.Name;
            c.Description = objectToUpdate.Description;
            c.ImagePath = objectToUpdate.Image;
            c.CategoryID = objectToUpdate.CategoryID;
            c.Stock = objectToUpdate.Stock;
            c.SaleID = objectToUpdate.SaleID;
            c.VAT = objectToUpdate.VAT;
            c.Active = objectToUpdate.IsActive;

            Entity.Products.ApplyCurrentValues(c);
            Entity.SaveChanges();
        }

        public void AddToCart(VwShoppingCart newCart)
        {
            Entity.ShoppingCarts.AddObject(new ShoppingCart
            {
                ID = newCart.ID,
                Username = newCart.Username,
                ProductID = newCart.ProductID,
                Quantity = newCart.Quantity
            });
            Entity.SaveChanges();
        }

        public VwShoppingCart CheckProductInCart(Guid productId, string username)
        {
            VwShoppingCart result = (from sc in Entity.ShoppingCarts
                where sc.ProductID == productId && sc.Username == username
                select new VwShoppingCart
                {
                    ID = sc.ID,
                    ProductID = sc.ProductID,
                    Username = sc.Username,
                    Quantity = sc.Quantity
                }
                ).FirstOrDefault();

            return result;
        }

        public IQueryable<VwProduct> Filter(string query)
        {
            IQueryable<VwProduct> prodList = from p in Entity.Products
                where p.Active && (p.Name.Contains(query) || p.Category.Name.Contains(query))
                select new VwProduct
                {
                    ID = p.ID,
                    Image = p.ImagePath,
                    Name = p.Name,
                    Stock = p.Stock,
                    SaleID = p.SaleID,
                    CategoryID = p.CategoryID,
                    Description = p.Description,
                    IsActive = p.Active,
                    VAT = p.VAT
                };
            return prodList;
        }

        public IQueryable<VwProduct> Filter(int category, int userType)
        {
            IQueryable<VwProduct> prodList = from p in Entity.Products
                from pr in p.PriceTypes.Where(t => t.UserTypeID == userType)
                where p.Active && p.CategoryID == category
                select new VwProduct
                {
                    ID = p.ID,
                    Image = p.ImagePath,
                    Name = p.Name,
                    Stock = p.Stock,
                    SaleID = p.SaleID,
                    CategoryID = p.CategoryID,
                    Discount =
                        p.SpecialSale != null &&
                        (EntityFunctions.TruncateTime(DateTime.Now) <
                         EntityFunctions.TruncateTime(p.SpecialSale.DateExpired))
                            ? p.SpecialSale.Discount
                            : 0,
                    Description = p.Description,
                    UnitPrice = pr.UnitPrice,
                    IsActive = p.Active,
                    VAT = p.VAT
                };
            return prodList.Distinct();
        }

        public IQueryable<VwProduct> Filter(string query, int userType)
        {
            IQueryable<VwProduct> prodList = from p in Entity.Products
                from pr in p.PriceTypes.Where(t => t.UserTypeID == userType)
                where p.Active && (p.Name.Contains(query) || p.Category.Name.Contains(query))
                select new VwProduct
                {
                    ID = p.ID,
                    Image = p.ImagePath,
                    Name = p.Name,
                    Stock = p.Stock,
                    SaleID = p.SaleID,
                    CategoryID = p.CategoryID,
                    Discount =
                        p.SpecialSale != null &&
                        (EntityFunctions.TruncateTime(DateTime.Now) <
                         EntityFunctions.TruncateTime(p.SpecialSale.DateExpired))
                            ? p.SpecialSale.Discount
                            : 0,
                    Description = p.Description,
                    UnitPrice = pr.UnitPrice,
                    IsActive = p.Active,
                    VAT = p.VAT
                };
            return prodList.Distinct();
        }

        public IQueryable<VwProduct> Filter(string query, int category, int userType)
        {
            IQueryable<Product> temp = (from p in Entity.Products
                where p.Active && p.CategoryID == category
                select p);
            IQueryable<VwProduct> result = (from p in temp
                from pr in p.PriceTypes.Where(t => t.UserTypeID == userType)
                where p.Name.Contains(query)
                select new VwProduct
                {
                    ID = p.ID,
                    Image = p.ImagePath,
                    Name = p.Name,
                    Stock = p.Stock,
                    SaleID = p.SaleID,
                    CategoryID = p.CategoryID,
                    Discount =
                        p.SpecialSale != null &&
                        (EntityFunctions.TruncateTime(DateTime.Now) <
                         EntityFunctions.TruncateTime(p.SpecialSale.DateExpired))
                            ? p.SpecialSale.Discount
                            : 0,
                    Description = p.Description,
                    UnitPrice = pr.UnitPrice,
                    IsActive = p.Active,
                    VAT = p.VAT
                });

            return result;
        }

        public IQueryable<VwProduct> FilteredPagedList(string query, int startIndex, int pageSize, string sorting)
        {
            return Filter(query).OrderBy(sorting).Skip(startIndex).Take(pageSize);
        }

        public IQueryable<string> FindProduct(string query)
        {
            IQueryable<string> result = Entity.Products.Where(u => u.Name.Contains(query)).Select(u => u.Name);
            IQueryable<string> temp = Entity.Categorys.Where(c => c.Name.Contains(query)).Select(u => u.Name);
            result = result.Concat(temp);
            return result;
        }

        public int GetCartItemsCount(string username)
        {
            int count = 0;
            count = Entity.ShoppingCarts.Count(sc => sc.Username == username);
            return count;
        }

        public decimal GetCartTotalPrice(string username)
        {
            decimal sum = 0;
            IQueryable<VwShoppingCart> cart = new ShoppingCartsRepo(false).ListAll(username);
            if (cart.Any())
            {
                sum = cart.Sum(t => t.TotalPrice);
            }
            return sum;
        }

        public decimal GetCartTotalTax(string username)
        {
            decimal sum = 0;
            IQueryable<VwShoppingCart> cart = new ShoppingCartsRepo(false).ListAll(username);
            if (cart.Any())
            {
                sum = cart.Sum(t => (t.Tax/100)*(t.DiscountPrice == 0 ? t.UnitPrice :t.DiscountPrice)*t.Quantity);
            }
            return sum;
        }

        public int GetCount()
        {
            return Entity.Products.Count();
        }

        public IQueryable<VwProduct> ListAllActive(int userType)
        {
            IQueryable<VwProduct> result = (from p in Entity.Products
                from pr in p.PriceTypes.Where(t => t.UserTypeID == userType)
                where p.Active
                select new VwProduct
                {
                    ID = p.ID,
                    Image = p.ImagePath,
                    Name = p.Name,
                    Stock = p.Stock,
                    SaleID = p.SaleID,
                    CategoryID = p.CategoryID,
                    Discount =
                        p.SpecialSale != null &&
                        (EntityFunctions.TruncateTime(DateTime.Now) <
                         EntityFunctions.TruncateTime(p.SpecialSale.DateExpired))
                            ? p.SpecialSale.Discount
                            : 0,
                    Description = p.Description,
                    UnitPrice = pr.UnitPrice,
                    IsActive = p.Active,
                    VAT = p.VAT
                });
            return result;
        }

        public VwProduct Read(Guid key, int? userType)
        {
            VwProduct result = (from p in Entity.Products
                from pr in p.PriceTypes.Where(t => t.UserTypeID == userType)
                where p.Active && p.ID == key
                select new VwProduct
                {
                    ID = p.ID,
                    Image = p.ImagePath,
                    Name = p.Name,
                    Stock = p.Stock,
                    SaleID = p.SaleID,
                    CategoryID = p.CategoryID,
                    Discount =
                        p.SpecialSale != null &&
                        (EntityFunctions.TruncateTime(DateTime.Now) <
                         EntityFunctions.TruncateTime(p.SpecialSale.DateExpired))
                            ? p.SpecialSale.Discount
                            : 0,
                    Description = p.Description,
                    UnitPrice = pr.UnitPrice,
                    IsActive = p.Active,
                    VAT = p.VAT
                }).FirstOrDefault();
            return result;
        }

        public void UpdateCart(VwShoppingCart vcart)
        {
            ShoppingCart sc = GetShoppingCart(vcart.ID);
            sc.Quantity = vcart.Quantity;
            Entity.ShoppingCarts.ApplyCurrentValues(sc);
            Entity.SaveChanges();
        }

        #endregion Public Methods

        #region Private Methods

        private Product GetProduct(Guid id)
        {
            return Entity.Products.SingleOrDefault(p => p.ID == id);
        }


        private ShoppingCart GetShoppingCart(Guid Id)
        {
            return
                Entity.ShoppingCarts.SingleOrDefault(
                    sc => sc.ID.Equals(Id));
        }

        #endregion Private Methods
    }
}