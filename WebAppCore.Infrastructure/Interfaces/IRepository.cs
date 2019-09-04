using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace WebAppCore.Infrastructure.Interfaces
{
	//T this is class K a params
	public interface IRepository<T, K> where T : class
	{
		T FindById(K id,params Expression<Func<T,object>>[] includeProperties);

		T FindSingle(Expression<Func<T,bool>> predicate,params Expression<Func<T,object>>[] includeProperties);

		IQueryable<T> FindAll(params Expression<Func<T,object>>[] includeProperties);

		IQueryable<T> FindAll(Expression<Func<T,bool>> predicate,params Expression<Func<T,object>>[] includeProperties);

		void Add(T entity);

		Task<T> AddAsyn(T t);

		T Update(T entity);

		void Remove(T entity);

		void Remove(K id);

		void RemoveMultiple(List<T> entities);

		Task<ICollection<T>> FindAllAsync(Expression<Func<T,bool>> match,params Expression<Func<T,object>>[] includeProperties);

		Task<ICollection<T>> GetAllAsyn(params Expression<Func<T,object>>[] includeProperties);

		Task<T> GetAByIdIncludeAsyn(Expression<Func<T,bool>> predicate,params Expression<Func<T,object>>[] includeProperties);

		Task<(ICollection<T>, long count)> Paging(int page,int pageSize,Expression<Func<T,bool>> predicate,params Expression<Func<T,object>>[] includeProperties);

		Task RemoveRange(T entity,Expression<Func<T,bool>> predicate);

	}
}
