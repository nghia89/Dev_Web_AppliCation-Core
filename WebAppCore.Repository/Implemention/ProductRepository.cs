using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using WebAppCore.Data.EF;
using WebAppCore.Data.Entities;
using WebAppCore.Data.Enums;
using WebAppCore.Infrastructure.Interfaces;
using WebAppCore.Repository.Interfaces;

namespace WebAppCore.Repository.Implemention
{
	public class ProductRepository:IProductRepository
	{
		private AppDbContext _appDbContext;
		private IRepository<Product,int> _productRepository;
		public ProductRepository(AppDbContext appDbContext,IRepository<Product,int> productRepository)
		{
			this._appDbContext = appDbContext;
			this._productRepository = productRepository;
		}

		public async Task<List<Product>> BuyALotProducts(int top)
		{
			var productDB = _appDbContext.Set<Product>();
			var data = productDB.Where(x => x.Status == Status.Active && x.BuyALot == true);
			return await data.Take(top).AsNoTracking().ToListAsync();
		}

		public async Task<IQueryable<Product>> FindAllAsync()
		{
			var productDB = _appDbContext.Set<Product>();
			var listData = await productDB.Where(x=>x.Status==Status.Active).Include(a => a.ProductCategory).AsNoTracking().ToListAsync();
			return listData.AsQueryable();
		}

		public async Task<Product> GetById(long id)
		{
			return await _productRepository.GetAByIdIncludeAsyn(x => x.Id == id);
		}

		public async Task<List<Product>> GetHotProduct(int top)
		{
			var productDB = _appDbContext.Set<Product>();
			var data = productDB.Where(x => x.Status == Status.Active && x.HomeFlag == true);
			return await data.OrderByDescending(x => x.DateCreated)
				.Take(top).AsNoTracking()
				.ToListAsync();
		}

		public async Task<List<Product>> GetProductNew(int top)
		{
			var productDB = _appDbContext.Set<Product>();
			return await productDB.Where(x => x.Status == Status.Active && x.HomeFlag != true && x.HotFlag != true).OrderByDescending(x => x.DateCreated)
			   .Take(top).AsNoTracking().ToListAsync();
		}

		public async Task<IQueryable<Product>> oldProduct()
		{
			var productDB = _appDbContext.Set<Product>();
			var listData = await productDB.Where(x => x.Status == Status.Active && x.OldProduct == true).AsNoTracking().AsQueryable().ToListAsync();
			//var data= await _productRepository.FindAllAsync(x => x.Status == Status.Active && x.OldProduct == true);
			return listData.AsQueryable();
		}

		public async Task<(List<Product> data, long totalCount)> Paging(int page,int page_size)
		{
			var (data, totalRow) = await _productRepository.Paging(page,page_size,x => x.Status == Status.Active,
					new Expression<Func<Product,object>>[] { a => a.ProductCategory }
				);
			return (data.ToList(), totalRow);
		}
	}
}
