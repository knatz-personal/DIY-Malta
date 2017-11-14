using System.Collections.Generic;
using Common.EntityModel;
using Common.Views;
using System.Linq;
using System.Linq.Dynamic;

namespace DAL.Repositories
{
    /// <summary>
    ///     Users Repository 
    /// </summary>
    public class UsersRepo : AppDbConnection, IDataRepository<VwUser>
    {
        #region Public Constructors

        /// <summary>
        ///     Users Repository 
        /// </summary>
        /// <param name="isAdministrator">
        ///     is Administrator 
        /// </param>
        /// <returns>
        ///     </returns>
        public UsersRepo(bool isAdministrator)
            : base(isAdministrator)
        {
        }

        #endregion Public Constructors

        #region Public Methods

        /// <summary>
        ///     Authenticate 
        /// </summary>
        /// <param name="username">
        ///     username 
        /// </param>
        /// <param name="password">
        ///     password 
        /// </param>
        /// <returns>
        ///     </returns>
        public bool Authenticate(string username, string password)
        {
            return Entity.Users.SingleOrDefault(o => o.Username == username
                                                     && o.Password == password) != null;
        }

        /// <summary>
        ///     Block 
        /// </summary>
        /// <param name="username">
        ///     username 
        /// </param>
        /// <returns>
        ///     </returns>
        public void Block(string username)
        {
            GetUser(username).Blocked = true;
        }

        /// <summary>
        ///     Create User 
        /// </summary>
        /// <param name="newObject">
        ///     new Object 
        /// </param>
        /// <returns>
        ///     </returns>
        public void Create(VwUser newObject)
        {
            var address = new Address
            {
                Residence = newObject.Address,
                Street = newObject.Street,
                TownID = newObject.TownID,
                PostCode = newObject.PostCode,
                TypeID = newObject.AddressType
            };

            var contact = new Contact
            {
                Mobile = newObject.Mobile,
                Phone = newObject.Phone,
                Email = newObject.Email,
                TypeID = newObject.ContactType
            };

            var u = new User
            {
                FirstName = newObject.FirstName,
                MiddleInitial = newObject.MiddleInitial,
                LastName = newObject.LastName,
                GenderID = newObject.GenderID,
                Username = newObject.Username,
                Password = newObject.Password,
                DateOfBirth = newObject.DateOfBirth,
                Blocked = newObject.Blocked,
                NoOfAttempts = newObject.NoOfAttempts,
                TypeID = GetTypeID("Customer"),
                Address = address,
                Contact = contact
            };

            //merge

            Entity.Users.AddObject(u);
            Entity.SaveChanges();
        }

        /// <summary>
        ///     Delete User 
        /// </summary>
        /// <param name="objectToDelete">
        ///     object To Delete 
        /// </param>
        /// <returns>
        ///     </returns>
        public void Delete(VwUser objectToDelete)
        {
            User user = GetUser(objectToDelete.Username);
            Entity.Users.DeleteObject(user);
            Entity.SaveChanges();
        }

        /// <summary>
        ///     Does Email Exist 
        /// </summary>
        /// <param name="email">
        ///     email 
        /// </param>
        /// <returns>
        ///     </returns>
        public bool DoesEmailExist(string email)
        {
            bool result = Entity.Contacts.Count(u => u.Email.ToLower().Equals(email.Trim().ToLower())) > 0;
            return result;
        }

        /// <summary>
        ///     Filter 
        /// </summary>
        /// <param name="query">
        ///     query 
        /// </param>
        /// <returns>
        ///     </returns>
        public IQueryable<VwUser> Filter(string query)
        {
            IQueryable<VwUser> result = Entity.Users.Where(o => o.Username.Contains(query)).Select(u => new VwUser
            {
                Username = u.Username,
                //Password = u.Password,
                FirstName = u.FirstName,
                MiddleInitial = u.MiddleInitial,
                LastName = u.LastName,
                DateOfBirth = u.DateOfBirth,
                GenderID = (int)u.GenderID,
                UserType = u.TypeID,
                Blocked = (bool)u.Blocked,
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
            return Entity.Users.Count();
        }

        /// <summary>
        ///     Get Count 
        /// </summary>
        /// <param name="username">
        ///     username 
        /// </param>
        /// <returns>
        ///     </returns>
        public int GetCount(string username)
        {
            return Filter(username).Count();
        }

        /// <summary>
        ///     Get Type ID 
        /// </summary>
        /// <param name="name">
        ///     name 
        /// </param>
        /// <returns>
        ///     </returns>
        public int? GetTypeID(string name)
        {
            int? result = null;
            UserType singleOrDefault = Entity.UserTypes.SingleOrDefault(ut => ut.Name == name.Trim());
            if (singleOrDefault != null)
            {
                result = singleOrDefault.ID;
            }
            return result;
        }

        /// <summary>
        ///     Increment No Of Attempts 
        /// </summary>
        /// <param name="username">
        ///     username 
        /// </param>
        /// <returns>
        ///     </returns>
        public void IncrementNoOfAttempts(string username)
        {
            GetUser(username).NoOfAttempts++;
        }

        /// <summary>
        ///     Is Username Taken 
        /// </summary>
        /// <param name="username">
        ///     username 
        /// </param>
        /// <returns>
        ///     </returns>
        public bool IsUsernameTaken(string username)
        {
            bool result = Entity.Users.Count(u => u.Username.ToLower().Equals(username.Trim().ToLower())) > 0;

            return result;
        }

        /// <summary>
        ///     List All 
        /// </summary>
        /// <returns>
        ///     </returns>
        public IQueryable<VwUser> ListAll()
        {
            return
                (
                    from u in Entity.Users
                    select new VwUser
                    {
                        Username = u.Username,
                        Password = u.Password,
                        FirstName = u.FirstName,
                        MiddleInitial = u.MiddleInitial,
                        LastName = u.LastName,
                        DateOfBirth = u.DateOfBirth,
                        GenderID = (int)u.GenderID,
                        UserType = u.TypeID,
                        Blocked = (bool)u.Blocked,
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
                    );
        }

        /// <summary>
        ///     Paged List of Users 
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
        public IQueryable<VwUser> PagedList(int startIndex, int pageSize, string sorting)
        {
            IQueryable<VwUser> result = ListAll().OrderBy(sorting).Skip(startIndex).Take(pageSize);
            return result;
        }

        /// <summary>
        ///     Paged List of Users 
        /// </summary>
        /// <param name="username">
        ///     username 
        /// </param>
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
        public IQueryable<VwUser> PagedList(string username, int startIndex, int pageSize, string sorting)
        {
            return Filter(username).OrderBy(sorting).Skip(startIndex).Take(pageSize);
        }

        /// <summary>
        ///     Read 
        /// </summary>
        /// <param name="key">
        ///     key 
        /// </param>
        /// <returns>
        ///     </returns>
        public VwUser Read(object key)
        {
            var user = new VwUser();
            string username = key + "";
            User temp = Entity.Users.SingleOrDefault(u => u.Username.Equals(username));

            if (temp != null)
            {
                if (temp.Address.TownID != null)
                {
                    user.TownID = (int)temp.Address.TownID;
                    user.Town = temp.Address.Town.Name;
                }
                user.Address = temp.Address.Residence;
                user.Street = temp.Address.Street;
                user.PostCode = temp.Address.PostCode;
                if (temp.Address.TypeID != null) user.AddressType = (int)temp.Address.TypeID;

                user.Mobile = temp.Contact.Mobile;
                user.Phone = temp.Contact.Phone;
                user.Email = temp.Contact.Email;
                if (temp.Contact.TypeID != null) user.ContactType = (int)temp.Contact.TypeID;

                user.FirstName = temp.FirstName;
                user.MiddleInitial = temp.MiddleInitial;
                user.LastName = temp.LastName;
                if (temp.GenderID != null) user.GenderID = (int)temp.GenderID;
                user.Username = temp.Username;
                user.Password = temp.Password;
                if (temp.Blocked != null) user.Blocked = (bool)temp.Blocked;
                if (temp.NoOfAttempts != null) user.NoOfAttempts = (int)temp.NoOfAttempts;
                user.UserType = temp.TypeID;
            }
            return user;
        }

        /// <summary>
        ///     Reset No Of Attempts 
        /// </summary>
        /// <param name="username">
        ///     username 
        /// </param>
        /// <returns>
        ///     </returns>
        public void ResetNoOfAttempts(string username)
        {
            GetUser(username).NoOfAttempts++;
        }

        /// <summary>
        ///     Un Block 
        /// </summary>
        /// <param name="username">
        ///     username 
        /// </param>
        /// <returns>
        ///     </returns>
        public void UnBlock(string username)
        {
            GetUser(username).Blocked = false;
        }

        /// <summary>
        ///     Update User 
        /// </summary>
        /// <param name="objectToUpdate">
        ///     object To Update 
        /// </param>
        /// <returns>
        ///     </returns>
        public void Update(VwUser objectToUpdate)
        {
            User u = GetUser(objectToUpdate.Username);

            u.FirstName = objectToUpdate.FirstName;
            u.MiddleInitial = objectToUpdate.MiddleInitial;
            u.LastName = objectToUpdate.LastName;
            u.GenderID = objectToUpdate.GenderID;
            u.Username = objectToUpdate.Username;
            u.Password = objectToUpdate.Password;
            u.Blocked = objectToUpdate.Blocked;
            u.NoOfAttempts = objectToUpdate.NoOfAttempts;
            u.TypeID = objectToUpdate.UserType;

            Entity.Users.ApplyCurrentValues(u);
            Entity.SaveChanges();
        }

        #endregion Public Methods

        #region Private Methods

        /// <summary>
        ///     Get User 
        /// </summary>
        /// <param name="username">
        ///     username 
        /// </param>
        /// <returns>
        ///     </returns>
        private User GetUser(string username)
        {
            return Entity.Users.SingleOrDefault(u => u.Username.Equals(username));
        }

        #endregion Private Methods

        public IQueryable<string> GetUsernames(string query)
        {
            return (Entity.Users.Where(u => u.Username.Contains(query)).Select(u => u.Username)
                )
            ;
        }
    }
}