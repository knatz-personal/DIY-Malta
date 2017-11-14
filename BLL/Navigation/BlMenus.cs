using System;
using System.Collections.Generic;
using System.Linq;
using BLL.CustomExceptions;
using Common.EntityModel;
using Common.Views;
using DAL.Repositories;

namespace BLL.Navigation
{
    public class BlMenus
    {
        public VwMenu Create(VwMenu record)
        {
            try
            {
                var mr = new MenusRepo(true);
                mr.Create(record);
                VwMenu m = mr.Read(record.ID);
                return m;
            }
            catch (Exception)
            {
                
                throw new DataInsertionException();
            }
        }

        public VwMenuRole CreateMenuRole(VwMenuRole record)
        {
            var mrepo = new MenusRepo(true);
            var rrepo = new RolesRepo(true);

            try
            {
                mrepo.Entity = rrepo.Entity;
                mrepo.Entity.Connection.Open();
                mrepo.Transaction = rrepo.Entity.Connection.BeginTransaction();
                Menu menu = mrepo.GetMenu(record.MenuID);
                Role role = rrepo.GetRole(record.RoleID);
                mrepo.CreateMenuRole(menu, role);
                mrepo.Transaction.Commit();
            }
            catch (Exception)
            {
                mrepo.Transaction.Rollback();
                throw new TransactionFailedException();
            }
            finally
            {
                mrepo.Entity.Connection.Close();
            }

            return record;
        }

        public void Delete(int id)
        {
            try
            {
                var mr = new MenusRepo(true);
                VwMenu m = mr.Read(id);
                mr.Delete(m);
            }
            catch (Exception)
            {
                throw new DataRelationException("Delete all child menus first!");
            }
        }

        public void DeleteMenuRole(int menuId, int roleId)
        {
            var mrepo = new MenusRepo(true);
            var rrepo = new RolesRepo(true);

            try
            {
                mrepo.Entity = rrepo.Entity;
                mrepo.Entity.Connection.Open();
                mrepo.Transaction = rrepo.Entity.Connection.BeginTransaction();
                Menu menu = mrepo.GetMenu(menuId);
                Role role = rrepo.GetRole(roleId);
                mrepo.DeleteMenuRole(menu, role);
                mrepo.Transaction.Commit();
            }
            catch (Exception)
            {
                mrepo.Transaction.Rollback();
                throw new TransactionFailedException();
            }
            finally
            {
                mrepo.Entity.Connection.Close();
            }
        }

        public int GetCount()
        {
            return new MenusRepo(true).GetCount();
        }

        public int GetLastChildID(int? parentId)
        {
            return new MenusRepo(true).GetLastChildID(parentId);
        }

        public int GetLastRootID()
        {
            return new MenusRepo(true).GetLastRootID();
        }

        public IQueryable<VwMenuRole> GetMenuRoles(int menuId)
        {
            try
            {

                return new MenusRepo(true).GetMenuRoles(menuId);
            }
            catch (Exception)
            {
                throw new DataListException();
            }
        }

        public IEnumerable<VwMenu> GetRootMenu(int roleId, bool isAdministrator)
        {
            try
            {
                return new MenusRepo(isAdministrator).GetRootMenu(roleId);
            }
            catch (Exception)
            {
                throw new DataListException();
            }
        }

        public IEnumerable<VwMenu> GetSubMenus(int roleId, int parentId, bool isAdministrator)
        {
            try
            { return new MenusRepo(isAdministrator).GetSubMenus(roleId, parentId); }
            catch (Exception)
            {
                throw new DataListException();
            }
        }

        public IEnumerable<VwMenu> ListRootMenus()
        {
            try
            { return new MenusRepo(true).ListRootMenus(); }
            catch (Exception)
            {
                throw new DataListException();
            }
        }

        public IEnumerable<VwMenu> ListSubMenus(int? parentId)
        {
            try
            { return new MenusRepo(true).ListSubMenus(parentId); }
            catch (Exception)
            {
                throw new DataListException();
            }
        }

        public IQueryable<VwMenu> PagedList(int startIndex, int pageSize, string sorting)
        {
            try
            { return new MenusRepo(true).PagedList(startIndex, pageSize, sorting); }
            catch (Exception)
            {
                throw new DataListException();
            }
        }

        public VwMenu Update(VwMenu record)
        {
            try
            {
                new MenusRepo(true).Update(record);
                record = new MenusRepo(true).Read(record.ID);
            }
            catch (Exception)
            {
                throw new DataUpdateException();
            }
            return record;
        }
    }
}