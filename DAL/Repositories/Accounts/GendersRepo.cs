using System.Collections.Generic;
using Common.EntityModel;
using Common.Views;
using System.Linq;
using System.Linq.Dynamic;

namespace DAL.Repositories
{
    /// <summary>
    ///     Genders Repository 
    /// </summary>
    public class GendersRepo : AppDbConnection, IDataRepository<VwGender>
    {
        #region Public Constructors

        /// <summary>
        ///     Genders Repository 
        /// </summary>
        /// <param name="isAdministrator">
        ///     is Administrator 
        /// </param>
        /// <returns>
        ///     </returns>
        public GendersRepo(bool isAdministrator)
            : base(isAdministrator)
        {
        }

        #endregion Public Constructors

        #region Public Methods

        /// <summary>
        ///     Create Gender 
        /// </summary>
        /// <param name="newObject">
        ///     new Object 
        /// </param>
        /// <returns>
        ///     </returns>
        public void Create(VwGender newObject)
        {
            Entity.Genders.AddObject(new Gender
            {
                ID = newObject.ID,
                Name = newObject.Name
            });
            Entity.SaveChanges();
        }

        /// <summary>
        ///     Delete Gender 
        /// </summary>
        /// <param name="objectToDelete">
        ///     object To Delete 
        /// </param>
        /// <returns>
        ///     </returns>
        public void Delete(VwGender objectToDelete)
        {
            Gender t = GetGender(objectToDelete.ID);
            Entity.Genders.DeleteObject(t);
            Entity.SaveChanges();
        }

        /// <summary>
        ///     Get Count 
        /// </summary>
        /// <returns>
        ///     </returns>
        public int GetCount()
        {
            return Entity.Genders.Count();
        }

        /// <summary>
        ///     List All 
        /// </summary>
        /// <returns>
        ///     </returns>
        public IQueryable<VwGender> ListAll()
        {
            return (from g in Entity.Genders
                    select new VwGender
                    {
                        ID = g.ID,
                        Name = g.Name
                    })
                ;
        }

        /// <summary>
        ///     Paged List of Genders 
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
        public IQueryable<VwGender> PagedList(int startIndex, int pageSize, string sorting)
        {
            IQueryable<VwGender> result = ListAll().OrderBy(sorting).Skip(startIndex).Take(pageSize);
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
        public VwGender Read(object key)
        {
            var gender = new VwGender();
            var g = new Gender();
            if (key is int)
            {
                g = Entity.Genders.SingleOrDefault(t => t.ID == (int)key);
            }
            else
            {
                var s = key as string;
                if (s != null)
                {
                    g = Entity.Genders.SingleOrDefault(t => t.Name.Equals(s));
                }
            }
            gender.ID = g.ID;
            gender.Name = g.Name;

            return gender;
        }

        /// <summary>
        ///     Update Gender 
        /// </summary>
        /// <param name="objectToUpdate">
        ///     object To Update 
        /// </param>
        /// <returns>
        ///     </returns>
        public void Update(VwGender objectToUpdate)
        {
            Gender gender = GetGender(objectToUpdate.ID);
            gender.Name = objectToUpdate.Name;
            Entity.Genders.ApplyCurrentValues(gender);
            Entity.SaveChanges();
        }

        #endregion Public Methods

        #region Private Methods

        /// <summary>
        ///     Get Gender 
        /// </summary>
        /// <param name="id">
        ///     id 
        /// </param>
        /// <returns>
        ///     </returns>
        private Gender GetGender(int id)
        {
            return Entity.Genders.SingleOrDefault(e => e.ID == id);
        }

        #endregion Private Methods
    }
}