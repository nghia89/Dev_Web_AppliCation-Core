using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebAppCore.Data.Entities;
using WebAppCore.Data.Enums;
using WebAppCore.Infrastructure.Interfaces;
using WebAppCore.Repository.Interfaces;

namespace WebAppCore.Repository.Implemention
{
	public class BlogRepository:IBlogRepository
	{
		private IRepository<Blog,int> _repository;
		public BlogRepository(IRepository<Blog,int> repository)
		{
			this._repository = repository;
		}
		public async Task<List<Blog>> GetLastest(int top)
		{
			var data =await _repository.FindAllAsync(x => x.Status == Status.Active);
			return data.OrderByDescending(x => x.DateCreated)
			   .Take(top).ToList();
		}
	}
}
