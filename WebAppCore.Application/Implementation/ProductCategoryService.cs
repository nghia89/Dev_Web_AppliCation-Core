using AutoMapper.QueryableExtensions;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebAppCore.Application.Interfaces;
using WebAppCore.Application.Mappers;
using WebAppCore.Application.ViewModels.Product;
using WebAppCore.Data.Entities;
using WebAppCore.Data.Enums;
using WebAppCore.Infrastructure.Interfaces;
using WebAppCore.Repository.Interfaces;

namespace WebAppCore.Application.Implementation
{
	public class ProductCategoryService:IProductCategoryService
	{
		private IRepository<ProductCategory,int> _productCategoryRepository;
		private IRepository<Product,int> _productRepository;
		private IUnitOfWork _unitOfWork;
		private IProductCategoryRepository _ProductCategoryRepository;

		public ProductCategoryService(IRepository<ProductCategory,int> productCategoryRepository,
			IRepository<Product,int> productRepository,IUnitOfWork unitOfWork,
			IProductCategoryRepository ProductCategoryRepository)
		{
			this._productCategoryRepository = productCategoryRepository;
			this._productRepository = productRepository;
			this._unitOfWork = unitOfWork;
			this._ProductCategoryRepository = ProductCategoryRepository;
		}

		public ProductCategoryViewModel Add(ProductCategoryViewModel productCategoryVm)
		{
			var productCategory = productCategoryVm.AddModel();
			_productCategoryRepository.Add(productCategory);
			return productCategoryVm;
		}

		public void Delete(int id)
		{
			_productCategoryRepository.Remove(id);
		}

		public List<ProductCategoryViewModel> GetAll()
		{
			var data = _productCategoryRepository.FindAll().OrderBy(x => x.ParentId);
			return data.Select(a=>a.ToModel()).ToList();
		}

		public List<ProductCategoryViewModel> GetAll(string keyword)
		{
			if(!string.IsNullOrEmpty(keyword))
				return _productCategoryRepository.FindAll(x => x.Name.Contains(keyword)
				|| x.Description.Contains(keyword))
					.OrderBy(x => x.ParentId).Select(x=>x.ToModel()).ToList();
			else
				return _productCategoryRepository.FindAll().OrderBy(x => x.ParentId)
					.Select(x => x.ToModel())
					.ToList();
		}

		public List<ProductCategoryViewModel> GetAllByParentId(int parentId)
		{
			return _productCategoryRepository.FindAll(x => x.Status == Status.Active
			&& x.ParentId == parentId)
			 .Select(x => x.ToModel())
			 .ToList();
		}

		public ProductCategoryViewModel GetById(int id)
		{
			var data = _productCategoryRepository.FindById(id);
			var dataMap = data.ToModel();
			return dataMap;
		}

		public async Task<List<ProductCategoryViewModel>> GetHomeCategories(int top)
		{
			var listData =await _ProductCategoryRepository.GetHomeCategories(top);
			return listData.Select(a=>a.ToModel()).ToList(); ;
		}

		public void ReOrder(int sourceId,int targetId)
		{
			var source = _productCategoryRepository.FindById(sourceId);
			var target = _productCategoryRepository.FindById(targetId);
			int tempOrder = source.SortOrder;
			source.SortOrder = target.SortOrder;
			target.SortOrder = tempOrder;

			_productCategoryRepository.Update(source);
			_productCategoryRepository.Update(target);
		}

		public void Save()
		{
			_unitOfWork.Commit();
		}

		public void Update(ProductCategoryViewModel productCategoryVm)
		{
			var productCategory = productCategoryVm.AddModel();
			_productCategoryRepository.Update(productCategory);
		}

		public void UpdateParentId(int sourceId,int targetId,Dictionary<int,int> items)
		{
			var sourceCategory = _productCategoryRepository.FindById(sourceId);
			sourceCategory.ParentId = targetId;
			_productCategoryRepository.Update(sourceCategory);

			//Get all sibling
			var sibling = _productCategoryRepository.FindAll(x => items.ContainsKey(x.Id));
			foreach(var child in sibling)
			{
				child.SortOrder = items[child.Id];
				_productCategoryRepository.Update(child);
			}
		}
	}
}