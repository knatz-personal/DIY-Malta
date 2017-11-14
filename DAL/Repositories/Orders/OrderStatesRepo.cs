using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic;
using Common.EntityModel;
using Common.Views;

namespace DAL.Repositories
{
    /// <summary>
    ///     Order States Repository
    /// </summary>
    public class OrderStatesRepo : AppDbConnection, IDataRepository<VwOrderState>
    {
        /// <summary>
        ///     Order States Repository
        /// </summary>
        /// <param name="isAdministrator">
        ///     is Administrator
        /// </param>
        /// <returns>
        /// </returns>
        public OrderStatesRepo(bool isAdministrator)
            : base(isAdministrator)
        {
        }

        /// <summary>
        ///     Create
        /// </summary>
        /// <param name="newObject">
        ///     new Object
        /// </param>
        /// <returns>
        /// </returns>
        public void Create(VwOrderState newObject)
        {
            Entity.OrderStates.AddObject(new OrderState
            {
                ID = newObject.ID,
                Name = newObject.Name
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
        public void Delete(VwOrderState objectToDelete)
        {
            OrderState t = GetOrderState(objectToDelete.ID);
            Entity.OrderStates.DeleteObject(t);
            Entity.SaveChanges();
        }

        /// <summary>
        ///     Get Count
        /// </summary>
        /// <returns>
        /// </returns>
        public int GetCount()
        {
            return Entity.OrderStates.Count();
        }

        /// <summary>
        ///     List All
        /// </summary>
        /// <returns>
        /// </returns>
        public IQueryable<VwOrderState> ListAll()
        {
            return (from t in Entity.OrderStates
                select new VwOrderState
                {
                    ID = t.ID,
                    Name = t.Name
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
        /// </returns>
        public IQueryable<VwOrderState> PagedList(int startIndex, int pageSize, string sorting)
        {
            IQueryable<VwOrderState> result = ListAll().OrderBy(sorting).Skip(startIndex).Take(pageSize);
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
        public VwOrderState Read(object key)
        {
            var OrderState = new VwOrderState();
            var tw = new OrderState();
            if (key is int)
            {
                tw = Entity.OrderStates.SingleOrDefault(t => t.ID == (int) key);
            }
            else
            {
                var s = key as string;
                if (s != null)
                {
                    tw = Entity.OrderStates.SingleOrDefault(t => t.Name.Equals(s));
                }
            }
            OrderState.ID = tw.ID;
            OrderState.Name = tw.Name;

            return OrderState;
        }

        /// <summary>
        ///     Update
        /// </summary>
        /// <param name="objectToUpdate">
        ///     object To Update
        /// </param>
        /// <returns>
        /// </returns>
        public void Update(VwOrderState objectToUpdate)
        {
            OrderState t = GetOrderState(objectToUpdate.ID);
            t.Name = objectToUpdate.Name;
            Entity.OrderStates.ApplyCurrentValues(t);
            Entity.SaveChanges();
        }

        /// <summary>
        ///     Get Order State
        /// </summary>
        /// <param name="id">
        ///     id
        /// </param>
        /// <returns>
        /// </returns>
        private OrderState GetOrderState(int id)
        {
            return Entity.OrderStates.SingleOrDefault(e => e.ID == id);
        }

        public int? GetStateID(string name)
        {
            int result = 0;

            OrderState singleOrDefault = Entity.OrderStates.SingleOrDefault(s => s.Name == name);
            if (singleOrDefault != null)
            {
                result = singleOrDefault.ID;
            }
            return result;
        }
    }
}