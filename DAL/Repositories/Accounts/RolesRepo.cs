using Common.EntityModel;
using Common.Views;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic;

namespace DAL.Repositories
{
    /// <summary>
    ///     Roles Repository
    /// </summary>
    public class RolesRepo : AppDbConnection, IDataRepository<VwRole>
    {
        #region Public Constructors

        /// <summary>
        ///     Roles Repository
        /// </summary>
        /// <param name="isAdministrator">
        ///     is Administrator
        /// </param>
        /// <returns>
        /// </returns>
        public RolesRepo(bool isAdministrator)
            : base(isAdministrator)
        {
        }

        #endregion Public Constructors

        #region Public Methods

        /// <summary>
        ///     Create Role
        /// </summary>
        /// <param name="newObject">
        ///     new Object
        /// </param>
        /// <returns>
        /// </returns>
        public void Create(VwRole newObject)
        {
            Entity.Roles.AddObject(new Role
            {
                ID = newObject.ID,
                Name = newObject.Name
            });
            Entity.SaveChanges();
        }

        /// <summary>
        ///     Delete Role
        /// </summary>
        /// <param name="objectToDelete">
        ///     object To Delete
        /// </param>
        /// <returns>
        /// </returns>
        public void Delete(VwRole objectToDelete)
        {
            Role t = GetRole(objectToDelete.ID);
            Entity.Roles.DeleteObject(t);
            Entity.SaveChanges();
        }

        /// <summary>
        ///     Get Count
        /// </summary>
        /// <returns>
        /// </returns>
        public int GetCount()
        {
            return Entity.Roles.Count();
        }

        /// <summary>
        ///     List All
        /// </summary>
        /// <returns>
        /// </returns>
        public IQueryable<VwRole> ListAll()
        {
            IQueryable<VwRole> result = Entity.Roles.Select(r => new VwRole
            {
                ID = r.ID,
                Name = r.Name
            });
            return result;
        }

        /// <summary>
        ///     Paged List of Roles
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
        public IQueryable<VwRole> PagedList(int startIndex, int pageSize, string sorting)
        {
            IQueryable<VwRole> result = ListAll().OrderBy(sorting).Skip(startIndex).Take(pageSize);
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
        public VwRole Read(object key)
        {
            var gender = new VwRole();
            var g = new Role();
            if (key is int)
            {
                g = Entity.Roles.SingleOrDefault(t => t.ID == (int)key);
            }
            else
            {
                var s = key as string;
                if (s != null)
                {
                    g = Entity.Roles.SingleOrDefault(t => t.Name.Equals(s));
                }
            }
            gender.ID = g.ID;
            gender.Name = g.Name;

            return gender;
        }

        /// <summary>
        ///     Update Role
        /// </summary>
        /// <param name="objectToUpdate">
        ///     object To Update
        /// </param>
        /// <returns>
        /// </returns>
        public void Update(VwRole objectToUpdate)
        {
            Role role = GetRole(objectToUpdate.ID);
            role.Name = objectToUpdate.Name;
            Entity.Roles.ApplyCurrentValues(role);
            Entity.SaveChanges();
        }

        /// <summary>
        ///     Allocate
        /// </summary>
        /// <param name="user">
        ///     user
        /// </param>
        /// <param name="role">
        ///     role
        /// </param>
        /// <returns>
        /// </returns>
        public void Allocate(VwUser user, VwRole role)
        {
            User us = GetUser(user.Username);
            if (us != null)
                us.Roles.Add(GetRole(role.ID));
            Entity.SaveChanges();
        }

        /// <summary>
        ///     De Allocate
        /// </summary>
        /// <param name="user">
        ///     user
        /// </param>
        /// <param name="role">
        ///     role
        /// </param>
        /// <returns>
        /// </returns>
        public void DeAllocate(VwUser user, VwRole role)
        {
            User us = GetUser(user.Username);
            if (us != null)
                us.Roles.Remove(GetRole(role.ID));
            Entity.SaveChanges();
        }

        /// <summary>
        ///     De Allocate All
        /// </summary>
        /// <param name="user">
        ///     user
        /// </param>
        /// <param name="roleList">
        ///     role List
        /// </param>
        /// <returns>
        /// </returns>
        public void DeAllocateAll(VwUser user, IEnumerable<VwRole> roleList)
        {
            User us = GetUser(user.Username);
            foreach (VwRole role in roleList)
            {
                us.Roles.Remove(GetRole(role.ID));
            }
            Entity.SaveChanges();
        }

        /// <summary>
        ///     Get Role
        /// </summary>
        /// <param name="roleTitle">
        ///     role Title
        /// </param>
        /// <returns>
        /// </returns>
        public int GetRoleId(string roleTitle)
        {
            Role role = Entity.Roles.SingleOrDefault(r => r.Name.Equals(roleTitle));
            return role != null ? (role.ID) : 0;
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
            var result = new List<VwRole>();

            User user = GetUser(username);

            if (user != null)
                result.AddRange(user.Roles.Select(r => new VwRole
                {
                    ID = r.ID,
                    Name = r.Name
                }));
            return result;
        }

        /// <summary>
        ///     Get Users In Role
        /// </summary>
        /// <param name="roleId">
        ///     role
        /// </param>
        /// <returns>
        /// </returns>
        public IQueryable<VwUser> GetUsersInRole(int roleId)
        {
            Role role = GetRole(roleId);
            return
                role.Users.Select(
                    u =>
                        u.Contact.TypeID != null
                            ? (u.Address.TownID != null
                                ? (u.Address.TypeID != null
                                    ? (u.NoOfAttempts != null
                                        ? (u.GenderID != null
                                            ? new VwUser
                                            {
                                                Username = u.Username,
                                                Password = u.Password,
                                                FirstName = u.FirstName,
                                                MiddleInitial = u.MiddleInitial,
                                                LastName = u.LastName,
                                                DateOfBirth = u.DateOfBirth,
                                                GenderID = (int)u.GenderID,
                                                UserType = u.TypeID,
                                                Blocked = u.Blocked != null && (bool)u.Blocked,
                                                NoOfAttempts = (int)u.NoOfAttempts,
                                                AddressID = u.AddressID,
                                                AddressType = (int)u.Address.TypeID,
                                                Address = u.Address.Residence,
                                                Street = u.Address.Street,
                                                PostCode = u.Address.PostCode,
                                                TownID = (int)u.Address.TownID,
                                                ContactID = u.ContactID,
                                                ContactType = (int)u.Contact.TypeID,
                                                Email = u.Contact.Email,
                                                Mobile = u.Contact.Mobile,
                                                Phone = u.Contact.Phone
                                            }
                                            : null)
                                        : null)
                                    : null)
                                : null)
                            : null).AsQueryable();
        }

        /// <summary>
        ///     Is User In Role
        /// </summary>
        /// <param name="username">
        ///     username
        /// </param>
        /// <param name="roleId">
        ///     role
        /// </param>
        /// <returns>
        /// </returns>
        public bool IsUserInRole(string username, int roleId)
        {
            return
                (GetRolesOfUser(username).SingleOrDefault(
                    t => t.ID == roleId) != null);
        }

        #endregion Public Methods

        #region Private Methods

        /// <summary>
        ///     Get Role
        /// </summary>
        /// <param name="roleId">
        ///     role
        /// </param>
        /// <returns>
        /// </returns>
        public Role GetRole(int roleId)
        {
            Role role = Entity.Roles.SingleOrDefault(r => r.ID == roleId);
            return role;
        }

        /// <summary>
        ///     Get Role
        /// </summary>
        /// <param name="roleName">
        ///     role Name
        /// </param>
        /// <returns>
        /// </returns>
        private Role GetRole(string roleName)
        {
            return Entity.Roles.SingleOrDefault(x => x.Name == roleName);
        }

        /// <summary>
        ///     Get User
        /// </summary>
        /// <param name="username">
        ///     username
        /// </param>
        /// <returns>
        /// </returns>
        private User GetUser(string username)
        {
            User singleOrDefault = Entity.Users.SingleOrDefault(u => u.Username.Equals(username));
            return singleOrDefault;
        }

        #endregion Private Methods
    }
}