using System.Collections.Generic;
using System.Linq;
using Common.Views;

namespace DAL
{
    internal interface IDataRepository<T>
    {
        void Create(T newObject);
        void Delete(T objectToDelete);

        IQueryable<T> ListAll();

        T Read(object key);
        void Update(T objectToUpdate);
        IQueryable<T> PagedList(int startIndex, int pageSize, string sorting);
    }
}