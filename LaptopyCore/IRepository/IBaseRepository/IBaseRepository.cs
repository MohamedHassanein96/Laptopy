using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace LaptopyCore.IRepository.IBaseRepository
{
    public interface IBaseRepository<T> where T : class
    {
       public IEnumerable<T> Get(
       Expression<Func<T, bool>>? expression = null,
       Func<IQueryable<T>, IQueryable<T>>? includes = null,
       bool tracked = true
       );

        public T1 GetByCompositeKeys<T1>(params object[] keys) where T1 : class;


        void Create(T entity);

        void Edit(T entity);

        void Delete(T entity);
    }
}
