using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using WebAppCore.Data.Entities;

namespace WebAppCore.Repository.Interfaces
{
	public interface IProductCategoryRepository
	{
		Task<List<ProductCategory>> GetAll();

		Task<ProductCategory> GetById(long id);

		Task<(List<ProductCategory> data, long totalCount)> Paging(int page,int page_size);

		Task<List<ProductCategory>> GetHomeCategories(int top);

		Task<List<ProductCategory>> GetProductNew(int top);
	}
}
