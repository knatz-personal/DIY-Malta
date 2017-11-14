using System.Collections.Generic;
using Common.EntityModel;
using Common.Views;
using System.Linq;
using System.Linq.Dynamic;

namespace DAL.Repositories
{
    /// <summary>
    ///     Towns Repository 
    /// </summary>
    public class TownsRepo : AppDbConnection, IDataRepository<VwTown>
    {
        #region Public Constructors

        /// <summary>
        ///     Towns Repository 
        /// </summary>
        /// <param name="isAdministrator">
        ///     is Administrator 
        /// </param>
        /// <returns>
        ///     </returns>
        public TownsRepo(bool isAdministrator)
            : base(isAdministrator)
        {
        }

        #endregion Public Constructors

        #region Public Methods

        /// <summary>
        ///     Create Town 
        /// </summary>
        /// <param name="newObject">
        ///     new Object 
        /// </param>
        /// <returns>
        ///     </returns>
        public void Create(VwTown newObject)
        {
            Entity.Towns.AddObject(new Town()
            {
                ID = newObject.ID,
                Name = newObject.Name
            });
            Entity.SaveChanges();
        }

        /// <summary>
        ///     Delete Town 
        /// </summary>
        /// <param name="objectToDelete">
        ///     object To Delete 
        /// </param>
        /// <returns>
        ///     </returns>
        public void Delete(VwTown objectToDelete)
        {
            var t = GetTown(objectToDelete.ID);
            Entity.Towns.DeleteObject(t);
            Entity.SaveChanges();
        }

        /// <summary>
        ///     Get Count 
        /// </summary>
        /// <returns>
        ///     </returns>
        public int GetCount()
        {
            return Entity.Towns.Count();
        }

        /// <summary>
        ///     List All 
        /// </summary>
        /// <returns>
        ///     </returns>
        public IQueryable<VwTown> ListAll()
        {
            return (from t in Entity.Towns
                    select new VwTown()
                    {
                        ID = t.ID,
                        Name = t.Name
                    });
        }

        /// <summary>
        ///     Paged List of Towns 
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
        public IQueryable<VwTown> PagedList(int startIndex, int pageSize, string sorting)
        {
            var result = ListAll().OrderBy(sorting).Skip(startIndex).Take(pageSize);
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
        public VwTown Read(object key)
        {
            var town = new VwTown();
            var tw = new Town();
            if (key is int)
            {
                tw = Entity.Towns.SingleOrDefault(t => t.ID == (int)key);
            }
            else
            {
                var s = key as string;
                if (s != null)
                {
                    tw = Entity.Towns.SingleOrDefault(t => t.Name.Equals(s));
                }
            }
            town.ID = tw.ID;
            town.Name = tw.Name;

            return town;
        }

        /// <summary>
        ///     Update Town 
        /// </summary>
        /// <param name="objectToUpdate">
        ///     object To Update 
        /// </param>
        /// <returns>
        ///     </returns>
        public void Update(VwTown objectToUpdate)
        {
            var t = GetTown(objectToUpdate.ID);
            t.Name = objectToUpdate.Name;
            Entity.Towns.ApplyCurrentValues(t);
            Entity.SaveChanges();
        }

        #endregion Public Methods

        #region Private Methods

        /// <summary>
        ///     Get Town 
        /// </summary>
        /// <param name="id">
        ///     id 
        /// </param>
        /// <returns>
        ///     </returns>
        private Town GetTown(int id)
        {
            return Entity.Towns.SingleOrDefault(e => e.ID == id);
        }

        #endregion Private Methods
    }
}