using Common.EntityModel;
using Common.Views;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic;

namespace DAL.Repositories
{
    /// <summary>
    ///     MenusRepository
    /// </summary>
    public class MenusRepo : AppDbConnection, IDataRepository<VwMenu>
    {
        #region Public Constructors

        /// <summary>
        ///     MenusRepository
        /// </summary>
        /// <param name="isAdministrator">
        ///     is Administrator
        /// </param>
        /// <returns>
        /// </returns>
        public MenusRepo(bool isAdministrator)
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
        /// </returns>
        public void Create(VwMenu newObject)
        {
            Entity.Menus.AddObject(new Menu
            {
                ID = newObject.ID,
                Name = newObject.Name,
                Description = newObject.Description,
                Url = newObject.Url,
                ParentID = newObject.ParentID
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
        /// </returns>
        public void Delete(VwMenu objectToDelete)
        {
            Menu t = GetMenu(objectToDelete.ID);
            Entity.Menus.DeleteObject(t);
            Entity.SaveChanges();
        }

        /// <summary>
        ///     Get Count
        /// </summary>
        /// <returns>
        /// </returns>
        public int GetCount()
        {
            return Entity.Menus.Count();
        }

        /// <summary>
        ///     List All
        /// </summary>
        /// <returns>
        /// </returns>
        public IQueryable<VwMenu> ListAll()
        {
            IQueryable<VwMenu> result = Entity.Menus.Select(m => new VwMenu
            {
                ID = m.ID,
                Name = m.Name,
                ParentID = m.ParentID,
                Description = m.Description,
                Url = m.Url
            });
            return result;
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
        /// </returns>
        public IQueryable<VwMenu> PagedList(int startIndex, int pageSize, string sorting)
        {
            IQueryable<VwMenu> result = ListAll().OrderBy(sorting).Skip(startIndex).Take(pageSize);
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
        public VwMenu Read(object key)
        {
            var id = (int)key;
            var m = Entity.Menus.SingleOrDefault(t => t.ID == id);
            VwMenu result = null;
            if (m != null)
            {
                result = new VwMenu
                {
                    ID = m.ID,
                    Name = m.Name,
                    ParentID = m.ParentID,
                    Description = m.Description,
                    Url = m.Url
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
        /// </returns>
        public void Update(VwMenu objectToUpdate)
        {
            Menu t = GetMenu(objectToUpdate.ID);
            t.Name = objectToUpdate.Name;
            t.ParentID = objectToUpdate.ParentID;
            t.Url = objectToUpdate.Url;
            t.Description = objectToUpdate.Description;
            Entity.Menus.ApplyCurrentValues(t);
            Entity.SaveChanges();
        }

        /// <summary>
        ///     Get Root Menu
        /// </summary>
        /// <param name="roleId">
        ///     role
        /// </param>
        /// <returns>
        /// </returns>
        public IEnumerable<VwMenu> GetRootMenu(int roleId)
        {
            IQueryable<VwMenu> result =
                (from r in Entity.Roles
                 from m in r.Menus
                 where r.ID == roleId && m.ParentID == null
                 select new VwMenu
                 {
                     ID = m.ID,
                     Name = m.Name,
                     ParentID = m.ParentID,
                     Description = m.Description,
                     Url = m.Url
                 });

            return result.OrderBy(x => x.ID);
        }

        /// <summary>
        ///     Get Sub Menus
        /// </summary>
        /// <param name="roleId">
        ///     role
        /// </param>
        /// <param name="parentId">
        ///     parent
        /// </param>
        /// <returns>
        /// </returns>
        public IEnumerable<VwMenu> GetSubMenus(int roleId, int parentId)
        {
            IQueryable<VwMenu> result =
                (from r in Entity.Roles
                 from m in r.Menus
                 where r.ID == roleId && m.ParentID == parentId
                 select new VwMenu
                 {
                     ID = m.ID,
                     Name = m.Name,
                     ParentID = m.ParentID,
                     Description = m.Description,
                     Url = m.Url
                 }
                    );
            return result.OrderBy(x => x.ID);
        }

        #endregion Public Methods

        #region Private Methods

        /// <summary>
        ///     Get Menu
        /// </summary>
        /// <param name="id">
        ///     id
        /// </param>
        /// <returns>
        /// </returns>
        public Menu GetMenu(int id)
        {
            return Entity.Menus.SingleOrDefault(e => e.ID == id);
        }

        #endregion Private Methods

        public IEnumerable<VwMenu> ListRootMenus()
        {
            IQueryable<VwMenu> result =
                (from m in Entity.Menus
                 where m.ParentID == null
                 select new VwMenu
                 {
                     ID = m.ID,
                     Name = m.Name,
                     ParentID = m.ParentID,
                     Description = m.Description,
                     Url = m.Url
                 });

            return result.OrderBy(x => x.ID).Distinct();
        }

        public IEnumerable<VwMenu> ListSubMenus(int? parentId)
        {
            IQueryable<VwMenu> result;

            if (parentId == 0)
            {
                //show all options
                result =
                    (
                        from m in Entity.Menus
                        select new VwMenu
                        {
                            ID = m.ID,
                            Name = m.Name,
                            ParentID = m.ParentID,
                            Description = m.Description,
                            Url = m.Url
                        }
                        );
            }
            else
            {
                result =
                    (
                        from m in Entity.Menus
                        where m.ParentID == parentId
                        select new VwMenu
                        {
                            ID = m.ID,
                            Name = m.Name,
                            ParentID = m.ParentID,
                            Description = m.Description,
                            Url = m.Url
                        }
                        );
            }


            return result.OrderBy(x => x.ID).Distinct();
        }

        public int GetLastRootID()
        {
            int result = (
                from en in Entity.Menus
                where en.ParentID == null
                select en
                ).OrderByDescending(o => o.ID).First().ID;
            return result;
        }

        public int GetLastChildID(int? parentId)
        {
            IQueryable<Menu> temp = (
                from en in Entity.Menus
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

        public IQueryable<VwMenuRole> GetMenuRoles(int menuId)
        {
            IQueryable<VwMenuRole> result = (from m in Entity.Menus
                                             from r in m.Roles
                                             where m.ID == menuId
                                             select new VwMenuRole
                                             {
                                                 MenuID = m.ID,
                                                 RoleID = r.ID
                                             });
            return result;
        }

        public void CreateMenuRole(Menu menu, Role role)
        {
            menu.Roles.Add(role);
            Entity.Menus.ApplyCurrentValues(menu);
            Entity.SaveChanges();
        }

        public void DeleteMenuRole(Menu menu, Role role)
        {
            menu.Roles.Remove(role);
            Entity.Menus.ApplyCurrentValues(menu);
            Entity.SaveChanges();
        }
    }
}