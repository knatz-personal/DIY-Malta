using BLL.CustomExceptions;
using Common.Views;
using DAL.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BLL.Accounts
{
    /// <summary>
    ///     Roles business logic 
    /// </summary>
    public class BlRoles
    {
        #region Public Methods

        /// <summary>
        ///     Allocate Role 
        /// </summary>
        /// <param name="username">
        ///     username 
        /// </param>
        /// <param name="roleId">
        ///     role 
        /// </param>
        /// <returns>
        /// </returns>
        public VwUserRoles AllocateRole(string username, int roleId)
        {
            var ur = new UsersRepo(false);
            var rr = new RolesRepo(true);
            ur.Entity = rr.Entity;
            ur.Entity.Connection.Open();
            try
            {
                ur.Transaction = rr.Transaction = rr.Entity.Connection.BeginTransaction();

                rr.Allocate(ur.Read(username), rr.Read(roleId));

                ur.Transaction.Commit();
                ;
                return new VwUserRoles { Username = username, RoleID = roleId };
            }
            catch (Exception ex)
            {
                ur.Transaction.Rollback();
                throw new TransactionFailedException();
            }
            finally
            {
                ur.Entity.Connection.Close();
            }
        }

        /// <summary>
        ///     Create Role 
        /// </summary>
        /// <param name="name">
        ///     name 
        /// </param>
        /// <returns>
        /// </returns>
        public VwRole Create(string name)
        {
            try
            {
                var tr = new RolesRepo(true);
                tr.Create(new VwRole { Name = name });
                VwRole t = tr.Read(name);
                return t;
            }
            catch (Exception)
            {
                throw new DataInsertionException();
            }
        }

        /// <summary>
        ///     Deallocate Role 
        /// </summary>
        /// <param name="username">
        ///     username 
        /// </param>
        /// <param name="role">
        ///     role 
        /// </param>
        /// <returns>
        /// </returns>
        public void DeallocateRole(string username, int role)
        {
            var ur = new UsersRepo(true);
            var rr = new RolesRepo(true);
            ur.Entity = rr.Entity;
            ur.Entity.Connection.Open();
            try
            {
                ur.Transaction = rr.Transaction = rr.Entity.Connection.BeginTransaction();

                rr.DeAllocate(ur.Read(username), rr.Read(role));

                ur.Transaction.Commit();
            }
            catch (Exception ex)
            {
                ur.Transaction.Rollback();
                throw new TransactionFailedException();
            }
            finally
            {
                ur.Entity.Connection.Close();
            }
        }

        /// <summary>
        ///     Delete Role 
        /// </summary>
        /// <param name="role">
        ///     role 
        /// </param>
        /// <returns>
        /// </returns>
        public void Delete(int role)
        {
            try
            {
                var gr = new RolesRepo(true);
                VwRole g = gr.Read(role);
                gr.Delete(g);
            }
            catch (Exception)
            {
                throw new DataDeletionException();
            }
        }

        /// <summary>
        ///     Get Count 
        /// </summary>
        /// <returns>
        /// </returns>
        public int GetCount()
        {
            return new RolesRepo(true).GetCount();
        }

        /// <summary>
        ///     Get Role Details 
        /// </summary>
        /// <param name="username">
        ///     username 
        /// </param>
        /// <returns>
        /// </returns>
        public IQueryable<VwUserRoles> GetRoleDetails(string username)
        {
            return new BlUserRoles().Filter(username);
        }

        /// <summary>
        ///     Get Role id 
        /// </summary>
        /// <param name="roleTitle">
        ///     role Title 
        /// </param>
        /// <returns>
        /// </returns>
        public int GetRoleId(string roleTitle)
        {
            return new RolesRepo(false).GetRoleId(roleTitle);
        }

        /// <summary>
        ///     Get Roles Of User 
        /// </summary>
        /// <param name="username">
        ///     username 
        /// </param>
        /// <returns>
        /// </returns>
        public List<VwRole> GetRolesOfUser(string username)
        {
            return new RolesRepo(false).GetRolesOfUser(username);
        }

        /// <summary>
        ///     List All 
        /// </summary>
        /// <returns>
        /// </returns>
        public IQueryable<VwRole> ListAll()
        {
            try
            {
                return new RolesRepo(true).ListAll();
            }
            catch (Exception)
            {
                throw new DataInsertionException();
            }
        }

        /// <summary>
        ///     Paged List of Role 
        /// </summary>
        /// <param name="startIndex">
        ///     jt Start Index 
        /// </param>
        /// <param name="pageSize">
        ///     jt Page Size 
        /// </param>
        /// <param name="sorting">
        ///     jt Sorting 
        /// </param>
        /// <returns>
        /// </returns>
        public IQueryable<VwRole> PagedList(int startIndex, int pageSize, string sorting)
        {
            try
            {
                return new RolesRepo(true).PagedList(startIndex, pageSize, sorting);
            }
            catch (Exception)
            {
                throw new DataListException();
            }
        }

        /// <summary>
        ///     Update Role 
        /// </summary>
        /// <param name="role">
        ///     role 
        /// </param>
        /// <returns>
        /// </returns>
        public void Update(VwRole role)
        {
            try
            {
                new RolesRepo(true).Update(role);
            }
            catch (Exception)
            {
                throw new DataUpdateException();
            }
        }

        #endregion Public Methods
    }
}