﻿using Microsoft.EntityFrameworkCore;
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
    public class ProductCategoryRepository : IProductCategoryRepository
    {
        private AppDbContext _appDbContext;
        private IRepository<ProductCategory, int> _productCategory;
        private IRepository<Product, int> _productRepository;
        public ProductCategoryRepository(AppDbContext appDbContext, IRepository<ProductCategory, int> productCategory,
            IRepository<Product, int> productRepository)
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
                 .FindAllAsync(x => x.HomeFlag == true && x.ParentId == null);

            var categories = query.OrderByDescending(x => x.HomeOrder)
                   .Take(top).ToList();

            foreach (var category in categories)
            {
                var idChildren = await this.GetChildrenByParent(category.Id);
                var product = await _productRepository.FindAllAsync(x => x.HomeFlag == true && idChildren.Contains(x.CategoryId));
                category.Products = product.OrderByDescending(x => x.DateCreated)
                    .Take(top).ToList();
            }
            return categories;
        }


        public async Task<List<int?>> GetChildrenByParent(int id)
        {
            var listId = new List<int?>();
            var data = await _appDbContext.Set<ProductCategory>().Where(a => a.ParentId == id).ToListAsync();


            while (data.Count() > 0 && data != null)
            {
                var ids = data.Select(a => (int?)a.Id).ToList();
                listId.AddRange(ids);
                data = await _appDbContext.Set<ProductCategory>().Where(a => ids.Contains(a.ParentId)).ToListAsync();
            }
            return listId;
        }

        public Task<List<ProductCategory>> GetProductNew(int top)
        {
            throw new System.NotImplementedException();
        }

        public Task<(List<ProductCategory> data, long totalCount)> Paging(int page, int page_size)
        {
            throw new System.NotImplementedException();
        }
    }
}
