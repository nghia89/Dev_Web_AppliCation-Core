using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebAppCore.Data.EF;
using WebAppCore.Data.Entities;
using WebAppCore.Data.Enums;
using WebAppCore.Infrastructure.Interfaces;
using WebAppCore.Repository.Interfaces;

namespace WebAppCore.Repository.Implemention
{
	public class ProductCategoryRepository:IProductCategoryRepository
	{
		private AppDbContext _appDbContext;
		private IRepository<ProductCategory,int> _productCategory;
		private IRepository<Product,int> _productRepository;
		public ProductCategoryRepository(AppDbContext appDbContext,IRepository<ProductCategory,int> productCategory,
			IRepository<Product,int> productRepository)
		{
			this._appDbContext = appDbContext;
			this._productCategory = productCategory;
			this._productRepository = productRepository;
		}

		public async Task<List<ProductCategory>> GetAll()
		{
			var data = await _productCategory.FindAllAsync(x => x.Status == Status.Active);
			return data.ToList();
		}

		public async Task<ProductCategory> GetById(long id)
		{
			var data = await _productCategory.GetAByIdIncludeAsyn(x => x.Id == id);
			return data;
		}

		public async Task<List<ProductCategory>> GetHomeCategories(int top)
		{
			var query = await _productCategory
				 .FindAllAsync(x => x.HomeFlag == true);

			var categories = query.OrderByDescending(x => x.HomeOrder)
				   .Take(top).ToList();

			foreach(var category in categories)
			{
				var product = await _productRepository.FindAllAsync(x => x.HomeFlag == true && x.CategoryId == category.Id);
				category.Products = product.OrderByDescending(x => x.DateCreated)
					.Take(top).ToList();
			}
			return categories;
		}

		public Task<List<ProductCategory>> GetProductNew(int top)
		{
			throw new System.NotImplementedException();
		}

		public Task<(List<ProductCategory> data, long totalCount)> Paging(int page,int page_size)
		{
			throw new System.NotImplementedException();
		}
	}
}
