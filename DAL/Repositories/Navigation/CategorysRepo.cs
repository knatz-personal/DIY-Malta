using Common.EntityModel;
using Common.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic;

namespace DAL.Repositories
{
    /// <summary>
    ///     CategorysRepository 
    /// </summary>
    public class CategorysRepo : AppDbConnection, IDataRepository<VwCategory>
    {
        #region Public Constructors

        /// <summary>
        ///     CategorysRepository 
        /// </summary>
        /// <param name="isAdministrator">
        ///     is Administrator 
        /// </param>
        /// <returns>
        ///     </returns>
        public CategorysRepo(bool isAdministrator)
            : base(isAdministrator)
        {
        }

        #endregion Public Constructors

        #region Public Methods

        /// <summary>
        ///     Create 
        /// </summary>
        /// <param name="newObject">
        ///     new Object 
        /// </param>
        /// <returns>
        ///     </returns>
        public void Create(VwCategory newObject)
        {
            Entity.Categorys.AddObject(new Category
            {
                ID = newObject.ID,
                Name = newObject.Name,
                ParentID = newObject.ParentID,
                Description = newObject.Description
            });
            Entity.SaveChanges();
        }

        /// <summary>
        ///     Delete 
        /// </summary>
        /// <param name="objectToDelete">
        ///     object To Delete 
        /// </param>
        /// <returns>
        ///     </returns>
        public void Delete(VwCategory objectToDelete)
        {
            Category t = GetCategory(objectToDelete.ID);
            Entity.Categorys.DeleteObject(t);
            Entity.SaveChanges();
        }

        /// <summary>
        ///     Get Children 
        /// </summary>
        /// <param name="parentId">
        ///     parent 
        /// </param>
        /// <returns>
        ///     </returns>
        public IQueryable<VwCategory> GetChildren(int? parentId)
        {
            IQueryable<VwCategory> result;

            if (parentId == 0)
            {
                //show all options
                result =
                    (
                        from c in Entity.Categorys
                        select new VwCategory
                        {
                            ID = c.ID,
                            Name = c.Name,
                            ParentID = c.ParentID,
                            Description = c.Description,
                            Quantity = c.Products.Count()
                        }
                        );
            }
            else
            {
                result = (from c in Entity.Categorys
                          where c.ParentID == parentId
                          orderby c.ID ascending
                          select new VwCategory
                          {
                              ID = c.ID,
                              Name = c.Name,
                              ParentID = c.ParentID,
                              Description = c.Description,
                              Quantity = c.Products.Count()
                          });
            }
            return result;
        }

        /// <summary>
        ///     Get Count 
        /// </summary>
        /// <returns>
        ///     </returns>
        public int GetCount()
        {
            return Entity.Categorys.Count();
        }

        public int GetLastRootID()
        {
            int result = (
                from en in Entity.Categorys
                where en.ParentID == null
                select en
                ).OrderByDescending(o => o.ID).First().ID;
            return result;
        }

        public int GetLastChildID(int? parentId)
        {
            IQueryable<Category> temp = (
                from en in Entity.Categorys
                where en.ParentID == parentId
                select en
                );
            int result = 0;
            if (temp.Any())
            {
                result = temp.OrderByDescending(o => o.ID).First().ID;
            }
            return result;
        }
        /// <summary>
        ///     Get Root Parents 
        /// </summary>
        /// <returns>
        ///     </returns>
        public IEnumerable<VwCategory> GetRootParents()
        {
            IQueryable<VwCategory> root =
                from c in Entity.Categorys
                where c.ParentID == null
                orderby c.ID ascending
                select new VwCategory
                {
                    ID = c.ID,
                    Name = c.Name,
                    ParentID = c.ParentID,
                    Description = c.Description,
                    Quantity = c.Products.Count()
                };

            return root;
        }

        /// <summary>
        ///     List All 
        /// </summary>
        /// <returns>
        ///     </returns>
        public IQueryable<VwCategory> ListAll()
        {
            return Entity.Categorys.Select(c => new VwCategory
            {
                ID = c.ID,
                Name = c.Name,
                ParentID = c.ParentID,
                Description = c.Description,
                Quantity = c.Products.Count()
            });
        }

        /// <summary>
        ///     Paged List 
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
        public IQueryable<VwCategory> PagedList(int startIndex, int pageSize, string sorting)
        {
            IQueryable<VwCategory> result = ListAll().OrderBy(sorting).Skip(startIndex).Take(pageSize);
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
        public VwCategory Read(object key)
        {
            VwCategory result = null;
            if (key is int)
            {
                var id = (int)key;
                Category temp = Entity.Categorys.SingleOrDefault(c => c.ID == id);

                result = new VwCategory
                {
                    ID = temp.ID,
                    Name = temp.Name,
                    ParentID = temp.ParentID,
                    Description = temp.Description,
                    Quantity = temp.Products.Count()
                };
            }
            else if (key is string)
            {
                Category temp = Entity.Categorys.SingleOrDefault(c => c.Name == (string) key);
                result = new VwCategory
                {
                    ID = temp.ID,
                    Name = temp.Name,
                    ParentID = temp.ParentID,
                    Description = temp.Description,
                    Quantity = temp.Products.Count()
                };
            }
            return result;
        }

        /// <summary>
        ///     Update 
        /// </summary>
        /// <param name="objectToUpdate">
        ///     object To Update 
        /// </param>
        /// <returns>
        ///     </returns>
        public void Update(VwCategory objectToUpdate)
        {
            Category t = GetCategory(objectToUpdate.ID);

            if (t != null)
            {
                t.Name = objectToUpdate.Name;
                t.ParentID = objectToUpdate.ParentID;
                t.Description = objectToUpdate.Description;
                Entity.Categorys.ApplyCurrentValues(t);
                Entity.SaveChanges();
            }
            else
            {
                throw new Exception("Failed to apply Update operation");
            }
        }

        #endregion Public Methods

        #region Private Methods

        /// <summary>
        ///     Get Category 
        /// </summary>
        /// <param name="id">
        ///     id 
        /// </param>
        /// <returns>
        ///     </returns>
        private Category GetCategory(int id)
        {
            return Entity.Categorys.SingleOrDefault(c => c.ID.Equals(id));
        }

        #endregion Private Methods
    }
}