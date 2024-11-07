using LaptopyCore.IRepository.IBaseRepository;
using LaptopyEF.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace LaptopyEF.Repository.BaseRepository
{
    public class BaseRepository<T> : IBaseRepository<T> where T : class
    {
        private readonly ApplicationDbContext _context;

        public BaseRepository(ApplicationDbContext context)
        {
            _context = context;
        }
        public void Create(T entity)
        {
            _context.Set<T>().Add(entity);
        }

        public void Delete(T entity)
        {
            _context.Set<T>().Remove(entity);
        }

        public void Edit(T entity)
        {
            _context.Set<T>().Update(entity);
        }

        public T1 GetByCompositeKeys<T1>(params object[] keys) where T1 : class
        {
            var entity = _context.Set<T1>().Find(keys);
            return entity;
        }

        public IEnumerable<T> Get(Expression<Func<T, bool>>? expression = null, Func<IQueryable<T>, IQueryable<T>>? includes = null, bool tracked = true)
        {
            IQueryable<T> query = _context.Set<T>();

            if (includes != null)
            {
                query = includes(query);
            }
            if (expression != null)
            {
                query = query.Where(expression);
            }
            if (!tracked)
            {
                query = query.AsNoTracking();
            }
            return query.ToList();
        }
    }
}
