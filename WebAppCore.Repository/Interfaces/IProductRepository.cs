using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using WebAppCore.Data.Entities;

namespace WebAppCore.Repository.Interfaces
{
	public interface IProductRepository
	{
		Task<Product> GetById(long id);

		Task<(List<Product> data, long totalCount)> Paging(int page,int page_size);

		Task<List<Product>> GetHotProduct(int top);

		Task<List<Product>> GetProductNew(int top);

	}
}
