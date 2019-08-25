using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using WebAppCore.Data.Interfaces;
using WebAppCore.Infrastructure.Interfaces;
using WebAppCore.Infrastructure.SharedKernel;
using WebAppCore.Utilities.Helpers;

namespace WebAppCore.Data.EF
{
    public class EFRepository<T, K> : IRepository<T, K>, IDisposable where T : DomainEntity<K>
    {
        private readonly AppDbContext _context;

        public EFRepository(AppDbContext context)
        {
            _context = context;
        }

        public void Add(T entity)
        {
            _context.Add(entity);
        }

		public async Task<T> AddAsyn(T t)
		{
			_context.Set<T>().Add(t);
			await _context.SaveChangesAsync();
			return t;
		}

		public void Dispose()
        {
            if (_context != null)
            {
                _context.Dispose();
            }
        }

        public IQueryable<T> FindAll(params Expression<Func<T, object>>[] includeProperties)
        {
            IQueryable<T> items = _context.Set<T>();
            if (includeProperties != null)
            {
                foreach (var includeProperty in includeProperties)
                {
                    items = items.Include(includeProperty);
                }
            }
            return items;
        }

        public IQueryable<T> FindAll(Expression<Func<T, bool>> predicate, params Expression<Func<T, object>>[] includeProperties)
        {
            IQueryable<T> items = _context.Set<T>();
            if (includeProperties != null)
            {
                foreach (var includeProperty in includeProperties)
                {
                    items = items.Include(includeProperty);
                }
            }
            return items.Where(predicate).AsNoTracking();
        }

		public async Task<ICollection<T>> FindAllAsync(Expression<Func<T,bool>> match,params Expression<Func<T,object>>[] includeProperties)
		{
			IQueryable<T> items = _context.Set<T>();
				if(includeProperties != null)
				{
					foreach(Expression<Func<T,object>> includeProperty in includeProperties)
					{
						items = items.Include(includeProperty);
					}
				}
				return await items.Where(match).AsNoTracking().ToListAsync();
		}

		public T FindById(K id, params Expression<Func<T, object>>[] includeProperties)
        {
            return FindAll(includeProperties).SingleOrDefault(x => x.Id.Equals(id));
        }

        public T FindSingle(Expression<Func<T, bool>> predicate, params Expression<Func<T, object>>[] includeProperties)
        {
            return FindAll(includeProperties).SingleOrDefault(predicate);
        }

		public async Task<T> GetAByIdIncludeAsyn(Expression<Func<T,bool>> predicate,params Expression<Func<T,object>>[] includeProperties)
		{
			IQueryable<T> items = _context.Set<T>();
			if(includeProperties != null)
			{
				foreach(var includeProperty in includeProperties)
				{
					items = items.Include(includeProperty);
				}
			}
			return await items.Where(predicate).AsNoTracking().FirstOrDefaultAsync();
		}

		public async Task<ICollection<T>> GetAllAsyn(params Expression<Func<T,object>>[] includeProperties)
		{
			IQueryable<T> items = _context.Set<T>();
			if(includeProperties != null)
			{
				foreach(var includeProperty in includeProperties)
				{
					items = items.Include(includeProperty);
				}
			}
			return await items.AsNoTracking().ToListAsync();
		}

		public async Task<(ICollection<T>, long count)> Paging(int page,int pageSize,Expression<Func<T,bool>> predicate,params Expression<Func<T,object>>[] includeProperties)
		{
			IQueryable<T> items = _context.Set<T>();
			//var totalRow = _context.Set<T>().Where(predicate);
			if(includeProperties != null)
			{
				foreach(var includeProperty in includeProperties)
				{
					items = items.Include(includeProperty);
				}
			}
			var data =  items.Where(predicate);
			var skip = (page - 1) * pageSize;
			var dataPaging = await data.Skip(skip)
								  .Take(pageSize).AsNoTracking()
								  .ToListAsync();

			return (dataPaging, data.Count());
		}

		public void Remove(T entity)
        {
            _context.Set<T>().Remove(entity);
        }

        public void Remove(K id)
        {
            var entity = FindById(id);
            Remove(entity);
        }

        public void RemoveMultiple(List<T> entities)
        {
            _context.Set<T>().RemoveRange(entities);
        }

        //public void Update(T entity)
        //{
        //    _context.Set<T>().Update(entity);
        //}

        public T Update(T entity)
        {
            var dbEntity = _context.Set<T>().AsNoTracking().Single(p => p.Id.Equals(entity.Id));
            var databaseEntry = _context.Entry(dbEntity);
            var inputEntry = _context.Entry(entity);

            //no items mentioned, so find out the updated entries

            IEnumerable<string> dateProperties = typeof(IDateTracking).GetPublicProperties().Select(x => x.Name);

            var allProperties = databaseEntry.Metadata.GetProperties()
            .Where(x => !dateProperties.Contains(x.Name));

            foreach (var property in allProperties)
            {
                var proposedValue = inputEntry.Property(property.Name).CurrentValue;

                var originalValue = databaseEntry.Property(property.Name).OriginalValue;

                if (proposedValue != null && !proposedValue.Equals(originalValue))
                {
                    databaseEntry.Property(property.Name).IsModified = true;
                    databaseEntry.Property(property.Name).CurrentValue = proposedValue;
                }
            }

            var result = _context.Set<T>().Update(dbEntity);
            return result.Entity;

        }
    }
}