using System;
using System.Linq;
using BLL.CustomExceptions;
using Common.Views;
using DAL.Repositories;

namespace BLL.Accounts
{
    /// <summary>
    /// Bl Users
    /// </summary>
    public class BlUsers
    {
        /// <summary>
        /// Authenticate
        /// </summary>
        /// <param name="username">username</param>
        /// <param name="password">password</param>
        /// <returns></returns>
        public bool Authenticate(string username, string password)
        {
            bool result = false;
            var user = new UsersRepo(false);

            if (user.IsUsernameTaken(username))
            {
                VwUser tmp = user.Read(username);
                if (tmp != null)
                {
                    if (tmp.NoOfAttempts <= 3)
                    {
                        if (tmp.Blocked)
                        {
                            throw new AccountBlockedException();
                        }
                        if (user.Authenticate(username, password))
                        {
                            user.ResetNoOfAttempts(username);
                            result = true;
                        }
                        else
                        {
                            user.IncrementNoOfAttempts(username);
                        }
                    }
                    else
                    {
                        user.Block(username);
                    }
                }
            }
            return result;
        }


        /// <summary>
        /// Delete
        /// </summary>
        /// <param name="user">user</param>
        /// <returns></returns>
        public void Delete(VwUser user)
        {
            try
            {
                new UsersRepo(true).Delete(user);
            }
            catch (Exception)
            {
                throw new DataDeletionException();
            }
        }


        /// <summary>
        /// Does Email Exist
        /// </summary>
        /// <param name="email">email</param>
        /// <returns></returns>
        public bool DoesEmailExist(string email)
        {
            bool result = false;
            try
            {
                result = new UsersRepo(false).DoesEmailExist(email);
            }
            catch
            {
                throw new UserAlreadyExistsException();
            }
            return result;
        }


        /// <summary>
        /// Get Count By Filter
        /// </summary>
        /// <param name="username">username</param>
        /// <returns></returns>
        public int GetCountByFilter(string username)
        {
            return new UsersRepo(true).GetCount(username);
        }

        /// <summary>
        /// Get User
        /// </summary>
        /// <param name="username">username</param>
        /// <returns></returns>
        public VwUser GetUser(string username)
        {
            return new UsersRepo(false).Read(username);
        }

        /// <summary>
        /// Get Usernames
        /// </summary>
        /// <param name="query">query</param>
        /// <returns></returns>
        public IQueryable<string> GetUsernames(string query)
        {
            return new UsersRepo(true).GetUsernames(query);
        }


        /// <summary>
        /// Is Username Taken
        /// </summary>
        /// <param name="username">username</param>
        /// <returns></returns>
        public bool IsUsernameTaken(string username)
        {
            bool result;
            try
            {
                result = new UsersRepo(false).IsUsernameTaken(username);
            }
            catch
            {
                throw new UserAlreadyExistsException();
            }
            return result;
        }


        /// <summary>
        /// Paged List
        /// </summary>
        /// <param name="username">username</param>
        /// <param name="startIndex">start Index</param>
        /// <param name="pageSize">page Size</param>
        /// <param name="sorting">sorting</param>
        /// <returns></returns>
        public IQueryable<VwUser> PagedList(string username, int startIndex, int pageSize, string sorting)
        {
            try
            {
                return new UsersRepo(true).PagedList(username, startIndex, pageSize, sorting);
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
        public IQueryable<VwUser> PagedList(int startIndex, int pageSize, string sorting)
        {
            try
            {
                return new UsersRepo(true).PagedList(startIndex, pageSize, sorting);
            }
            catch (Exception)
            {
                throw new DataListException();
            }
        }


        /// <summary>
        /// Register
        /// </summary>
        /// <param name="user">user</param>
        /// <returns></returns>
        public bool Register(VwUser user)
        {
            var ur = new UsersRepo(false);
            ur.Create(user);
            var blr = new BlRoles();
            blr.AllocateRole(user.Username, blr.GetRoleId("Customer"));
            if (ur.Authenticate(user.Username, user.Password))
            {
                return true;
            }
            return false;
        }


        /// <summary>
        /// Update
        /// </summary>
        /// <param name="user">user</param>
        /// <returns></returns>
        public void Update(VwUser user)
        {
            var ut = new UserTypesRepo(true);
            var t = new TownsRepo(true);
            var gr = new GendersRepo(true);
            var ur = new UsersRepo(true);

            try
            {
                ur.Entity = ut.Entity = t.Entity = gr.Entity;
                ur.Entity.Connection.Open();
                ur.Transaction =
                    ut.Transaction = t.Transaction = gr.Transaction = ur.Entity.Connection.BeginTransaction();

                ur.Update(user);
                ur.Transaction.Commit();
            }
            catch (Exception)
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
        /// Get Type ID
        /// </summary>
        /// <param name="name">name</param>
        /// <returns></returns>
        public int? GetTypeID(string name)
        {
            return new UsersRepo(false).GetTypeID(name);
        }
    }
}