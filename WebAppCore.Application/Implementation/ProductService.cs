﻿using Microsoft.EntityFrameworkCore;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using WebAppCore.Application.Interfaces;
using WebAppCore.Application.Mappers;
using WebAppCore.Application.ViewModels.Product;
using WebAppCore.Data.Entities;
using WebAppCore.Data.Enums;
using WebAppCore.Infrastructure.Enums;
using WebAppCore.Infrastructure.Interfaces;
using WebAppCore.Repository.Interfaces;
using WebAppCore.Utilities.Constants;
using WebAppCore.Utilities.Dtos;
using WebAppCore.Utilities.Helpers;

namespace WebAppCore.Application.Implementation
{
	public class ProductService:IProductService
	{
		private IRepository<Product,int> _productRepository;
		private IRepository<Tag,string> _tagRepository;
		private IRepository<ProductTag,int> _productTagRepository;
		private IRepository<ProductQuantity,int> _productQuantityRepository;
		private IRepository<ProductImage,int> _productImageRepository;
		private IRepository<WholePrice,int> _wholePriceRepository;
		private IProductRepository _productServiceRepository;

		private IUnitOfWork _unitOfWork;

		public ProductService(IRepository<Product,int> productRepository,
			IRepository<Tag,string> tagRepository,
			IRepository<ProductQuantity,int> productQuantityRepository,
			IRepository<ProductImage,int> productImageRepository,
			IRepository<WholePrice,int> wholePriceRepository,
		IUnitOfWork unitOfWork,IProductRepository productServiceRepository,
		IRepository<ProductTag,int> productTagRepository)
		{
			_productRepository = productRepository;
			_tagRepository = tagRepository;
			_productQuantityRepository = productQuantityRepository;
			_productTagRepository = productTagRepository;
			_wholePriceRepository = wholePriceRepository;
			_productImageRepository = productImageRepository;
			_unitOfWork = unitOfWork;
			_productServiceRepository = productServiceRepository;
		}

		public ProductViewModel Add(ProductViewModel productVm)
		{
			List<ProductTag> productTags = new List<ProductTag>();

			if(!string.IsNullOrEmpty(productVm.Tags))
			{
				string[] tags = productVm.Tags.Split(',');
				foreach(string t in tags)
				{
					var tagId = TextHelper.ToUnsignString(t);
					if(!_tagRepository.FindAll(x => x.Id == tagId).Any())
					{
						Tag tag = new Tag {
							Id = tagId,
							Name = t,
							Type = CommonConstants.ProductTag
						};
						_tagRepository.Add(tag);
					}

					ProductTag productTag = new ProductTag {
						TagId = tagId
					};
					productTags.Add(productTag);
				}
			}
			var product = productVm.AddModel();
			foreach(var productTag in productTags)
			{
				product.ProductTags.Add(productTag);
			}
			_productRepository.Add(product);

			return productVm;
		}

		public void AddQuantity(int productId,List<ProductQuantityViewModel> quantities)
		{
			_productQuantityRepository.RemoveMultiple(_productQuantityRepository.FindAll(x => x.ProductId == productId).ToList());
			foreach(var quantity in quantities)
			{
				_productQuantityRepository.Add(new ProductQuantity() {
					ProductId = productId,
					ColorId = quantity.ColorId,
					SizeId = quantity.SizeId,
					Quantity = quantity.Quantity
				});
			}
		}

		public void Delete(int id)
		{
			_productRepository.Remove(id);
		}

		public void Dispose()
		{
			GC.SuppressFinalize(this);
		}

		public List<ProductViewModel> GetAll()
		{
			return _productRepository.FindAll(x => x.ProductCategory).Select(x => x.ToModel()).ToList();
		}

		public PagedResult<ProductViewModel> GetAllPaging(int? categoryId,string keyword,int page,int pageSize,string sortBy,int? sortPrice)
		{
			var query = _productRepository.FindAll(x => x.Status == Status.Active,a => a.ProductCategory);
			if(!string.IsNullOrEmpty(keyword))
				query = query.Where(x => x.Name.Contains(keyword));
			if(categoryId.HasValue)
				query = query.Where(x => x.CategoryId == categoryId.Value);

			int totalRow = query.Count();
			if(sortPrice.HasValue)
			{
				switch(sortPrice)
				{
					case (int)PriceEnum.DUOI_1TR:
						query = query.Where(x => x.Price <= 1000000);
						break;
					case (int)PriceEnum.TU_1TR_DEN_2TR:
						query = query.Where(x => x.Price >= 1000000 && x.Price <= 2000000);
						break;
					case (int)PriceEnum.TU_2TR_DEN_4TR:
						query = query.Where(x => x.Price >= 2000000 && x.Price <= 4000000);
						break;
					case (int)PriceEnum.TU_4TR_DEN_7TR:
						query = query.Where(x => x.Price >= 4000000 && x.Price <= 7000000);
						break;
					case (int)PriceEnum.TU_7TR_DEN_10TR:
						query = query.Where(x => x.Price >= 7000000 && x.Price <= 10000000);
						break;
					case (int)PriceEnum.TREN_10TR:
						query = query.Where(x => x.Price >= 10000000);
						break;
					default:
						break;
				}
			}

			if(!string.IsNullOrEmpty(sortBy))
			{
				switch(sortBy)
				{
					case "price":
						query = query.OrderByDescending(x => x.Price);
						break;

					case "name":
						query = query.OrderBy(x => x.Name);
						break;

					case "lastest":
						query = query.OrderByDescending(x => x.DateCreated);
						break;

					default:
						query = query.OrderByDescending(x => x.DateCreated);
						break;
				}
			}

			query = query.Skip((page - 1) * pageSize).Take(pageSize);

			var data = query.OrderByDescending(x => x.DateCreated).Select(x => x.ToModel()).AsNoTracking().ToList();

			var paginationSet = new PagedResult<ProductViewModel>() {
				Results = data,
				CurrentPage = page,
				RowCount = totalRow,
				PageSize = pageSize
			};
			return paginationSet;
		}

		public ProductViewModel GetById(int id)
		{
			var data = _productRepository.FindById(id);
			return data.ToModel();
		}

		public List<ProductQuantityViewModel> GetQuantities(int productId)
		{
			return _productQuantityRepository.FindAll(x => x.ProductId == productId).Select(x => x.ToModel()).ToList();
		}

		public void ImportExcel(string filePath,int categoryId)
		{

			using(var package = new ExcelPackage(new FileInfo(filePath)))// đường dẩn gửi lên server
			{
				ExcelWorksheet workSheet = package.Workbook.Worksheets[1];//lấy all các Worksheets in excel  bắt đầu từ bản đầu tiên
				Product product;
				for(int i = workSheet.Dimension.Start.Row + 1; i <= workSheet.Dimension.End.Row; i++)
				{
					product = new Product();
					product.CategoryId = categoryId;

					product.Name = workSheet.Cells[i,1].Value.ToString();
					product.SeoAlias = TextHelper.ToUnsignString(product.Name);
					if(workSheet.Cells[i,2].Value != null)
					{
						product.Description = workSheet.Cells[i,2].Value.ToString();
					}
					else
					{
						product.Description = "";
					}

					//nêu TryParse thành công thì ra giá trị nếu không thành công thi lấy default =0
					if(workSheet.Cells[i,3].Value != null)
					{
						decimal.TryParse(workSheet.Cells[i,3].Value.ToString(),out var originalPrice);
					}
					else
					{
						product.OriginalPrice = 0;
					}

					if(workSheet.Cells[i,4].Value != null)
					{
						decimal.TryParse(workSheet.Cells[i,4].Value.ToString(),out var price);
						product.Price = price;
					}
					else
					{
						product.Price = 0;
					}

					if(workSheet.Cells[i,5].Value != null)
					{
						decimal.TryParse(workSheet.Cells[i,5].Value.ToString(),out var promotionPrice);
						product.PromotionPrice = promotionPrice;
					}
					else
					{
						product.PromotionPrice = 0;
					}


					if(workSheet.Cells[i,6].Value != null)
					{
						product.Image = workSheet.Cells[i,6].Value.ToString();
					}
					else
					{
						product.Image = "";
					}

					if(workSheet.Cells[i,7].Value != null)
					{
						product.Content = workSheet.Cells[i,6].Value.ToString();
					}
					else
					{
						product.Content = "";
					}

					if(workSheet.Cells[i,8].Value != null)
					{
						product.SeoKeywords = workSheet.Cells[i,8].Value.ToString();
					}
					else
					{
						product.SeoKeywords = "";
					}

					if(workSheet.Cells[i,9].Value != null)
					{
						product.SeoDescription = workSheet.Cells[i,9].Value.ToString();
					}
					else
					{
						product.SeoDescription = "";
					}

					if(workSheet.Cells[i,10].Value != null)
					{
						int.TryParse(workSheet.Cells[i,10].Value.ToString(),out var status);
						product.Status = status == 1 ? Status.Active : Status.InActive;
					}
					else
					{
						product.Status = Status.InActive;
					}

					if(workSheet.Cells[i,11].Value != null)
					{
						int.TryParse(workSheet.Cells[i,11].Value.ToString(),out var homeFlag);
						product.HomeFlag = homeFlag==1?true:false;
					}
					else
					{
						product.HomeFlag =false;
					}

					if(workSheet.Cells[i,12].Value != null)
					{
						int.TryParse(workSheet.Cells[i,12].Value.ToString(),out var hotFlag);
						product.HotFlag = hotFlag == 1 ? true : false;
					}
					else
					{
						product.HomeFlag = false;
					}

					_productRepository.Add(product);
				}
			}
		}

		public void Save()
		{
			_unitOfWork.Commit();
		}

		public void Update(ProductViewModel productVm)
		{
			List<ProductTag> productTags = new List<ProductTag>();

			if(!string.IsNullOrEmpty(productVm.Tags))
			{
				string[] tags = productVm.Tags.Split(',');
				foreach(string t in tags)
				{
					var tagId = TextHelper.ToUnsignString(t);
					if(!_tagRepository.FindAll(x => x.Id == tagId).Any())
					{
						Tag tag = new Tag();
						tag.Id = tagId;
						tag.Name = t;
						tag.Type = CommonConstants.ProductTag;
						_tagRepository.Add(tag);
					}
					_productTagRepository.RemoveMultiple(_productTagRepository.FindAll(x => x.Id == productVm.Id).ToList());
					ProductTag productTag = new ProductTag {
						TagId = tagId
					};
					productTags.Add(productTag);
				}
			}

			var product = productVm.AddModel();
			foreach(var productTag in productTags)
			{
				product.ProductTags.Add(productTag);
			}
			_productRepository.Update(product);
		}

		public List<ProductImageViewModel> GetImages(int productId)
		{
			return _productImageRepository.FindAll(x => x.ProductId == productId)
				.Select(a => a.ProductImage()).ToList();
		}

		public void AddImages(int productId,string[] images)
		{
			_productImageRepository.RemoveMultiple(_productImageRepository.FindAll(x => x.ProductId == productId).ToList());
			foreach(var image in images)
			{
				_productImageRepository.Add(new ProductImage() {
					Path = image,
					ProductId = productId,
					Caption = string.Empty
				});
			}
		}

		public void AddWholePrice(int productId,List<WholePriceViewModel> wholePrices)
		{
			_wholePriceRepository.RemoveMultiple(_wholePriceRepository.FindAll(x => x.ProductId == productId).ToList());
			foreach(var wholePrice in wholePrices)
			{
				_wholePriceRepository.Add(new WholePrice() {
					ProductId = productId,
					FromQuantity = wholePrice.FromQuantity,
					ToQuantity = wholePrice.ToQuantity,
					Price = wholePrice.Price
				});
			}
		}

		public List<WholePriceViewModel> GetWholePrices(int productId)
		{
			return _wholePriceRepository.FindAll(x => x.ProductId == productId).Select(x => x.ToModel()).ToList();
		}

		public List<ProductViewModel> GetLastest(int top)
		{
			return _productRepository.FindAll(x => x.Status == Status.Active && x.HomeFlag == false).OrderByDescending(x => x.DateCreated)
				.Take(top).Select(x => x.ToModel()).ToList();
		}

		public async Task<List<ProductViewModel>> GetHotProduct(int top)
		{
			var listData = await _productServiceRepository.GetHotProduct(top);
			return listData.Select(x => x.ToModel()).ToList();
		}
		public async Task<List<ProductViewModel>> GetBuyALotProduct(int top)
		{
			var listData = await _productServiceRepository.BuyALotProducts(top);
			return listData.Select(x => x.ToModel()).ToList();
		}


		public async Task<List<ProductViewModel>> GetRelatedProducts(int id,int top)
		{
			var product = _productRepository.FindById(id);
			var data = await _productRepository.FindAllAsync(x => x.Status == Status.Active
				  && x.Id != id && x.CategoryId == product.CategoryId);
			return data.OrderByDescending(x => x.DateCreated)
			.Take(top)
			.Select(x => x.ToModel())
			.ToList();
		}

		public List<ProductViewModel> GetUpsellProducts(int top)
		{
			return _productRepository.FindAll(x => x.PromotionPrice != null)
			   .OrderByDescending(x => x.DateModified)
			   .Take(top)
			   .Select(x => x.ToModel()).ToList();
		}

		public List<TagViewModel> GetProductTags(int productId)
		{
			var tags = _tagRepository.FindAll();
			var productTags = _productTagRepository.FindAll();

			var query = from t in tags
						join pt in productTags
						on t.Id equals pt.TagId
						where pt.ProductId == productId
						select new TagViewModel() {
							Id = t.Id,
							Name = t.Name
						};
			return query.ToList();
		}

		public bool CheckAvailability(int productId,int size,int color)
		{
			var quantity = _productQuantityRepository.FindSingle(x => x.ColorId == color && x.SizeId == size && x.ProductId == productId);
			if(quantity == null)
				return false;
			return quantity.Quantity > 0;
		}

		public PagedResult<ProductViewModel> GetAllProCatePaging(int? categoryId,string keyword,int page,int pageSize,string sortBy)
		{
			var query = _productRepository.FindAll(x => x.Status == Status.Active);
			if(!string.IsNullOrEmpty(keyword))
				query = query.Where(x => x.Name.Contains(keyword));
			if(categoryId.HasValue)
				query = query.Where(x => x.CategoryId == categoryId.Value);
			//if(!string.IsNullOrEmpty(sortBy))
			//{
			//    query=query.Where(x=>x.)
			//}
			int totalRow = query.Count();

			query = query.OrderByDescending(x => x.DateCreated)
				.Skip((page - 1) * pageSize).Take(pageSize);

			var data = query.Select(x => x.ToModel()).ToList();

			var paginationSet = new PagedResult<ProductViewModel>() {
				Results = data,
				CurrentPage = page,
				RowCount = totalRow,
				PageSize = pageSize
			};
			return paginationSet;
		}

		public async Task<List<ProductViewModel>> GetProductNew(int top)
		{
			var data = await _productServiceRepository.GetProductNew(top);
			return data.Select(x => x.ToModel()).ToList();
		}

		public async Task<PagedResult<ProductViewModel>> PagingAsync(int? categoryId,string keyword,int page,int pageSize,string sortBy)
		{
			var query = await _productServiceRepository.FindAllAsync();
			if(!string.IsNullOrEmpty(keyword))
				query = query.Where(x => x.Name.Contains(keyword));
			if(categoryId.HasValue)
				query = query.Where(x => x.CategoryId == categoryId.Value);

			int totalRow = query.Count();
			switch(sortBy)
			{
				case "price":
					query = query.OrderByDescending(x => x.Price);
					break;

				case "name":
					query = query.OrderBy(x => x.Name);
					break;

				case "lastest":
					query = query.OrderByDescending(x => x.DateCreated);
					break;

				default:
					query = query.OrderByDescending(x => x.DateCreated);
					break;
			}
			query = query.Skip((page - 1) * pageSize).Take(pageSize);
			List<Product> products = query.ToList();

			var data = products.Select(x => x.ToModel()).OrderByDescending(x => x.DateCreated).ToList();

			var paginationSet = new PagedResult<ProductViewModel>() {
				Results = data,
				CurrentPage = page,
				RowCount = totalRow,
				PageSize = pageSize
			};
			return paginationSet;
		}

		public async Task<PagedResult<ProductViewModel>> OldProductPage(int page,int page_size,string sortBy,int? sortPrice)
		{
			var data = await _productServiceRepository.oldProduct();
			int totalRow = data.Count();
			switch(sortBy)
			{
				case "price":
					data = data.OrderByDescending(x => x.Price);
					break;

				case "name":
					data = data.OrderBy(x => x.Name);
					break;

				case "lastest":
					data = data.OrderByDescending(x => x.DateCreated);
					break;

				default:
					data = data.OrderByDescending(x => x.DateCreated);
					break;
			}
			if(sortPrice.HasValue)
			{
				switch(sortPrice)
				{
					case (int)PriceEnum.DUOI_1TR:
						data = data.Where(x => x.Price <= 500000);
						break;
					case (int)PriceEnum.TU_1TR_DEN_2TR:
						data = data.Where(x => x.Price >= 1000000 && x.Price <= 2000000);
						break;
					case (int)PriceEnum.TU_2TR_DEN_4TR:
						data = data.Where(x => x.Price >= 2000000 && x.Price <= 4000000);
						break;
					case (int)PriceEnum.TU_4TR_DEN_7TR:
						data = data.Where(x => x.Price >= 4000000 && x.Price <= 7000000);
						break;
					case (int)PriceEnum.TU_7TR_DEN_10TR:
						data = data.Where(x => x.Price >= 7000000 && x.Price <= 10000000);
						break;
					case (int)PriceEnum.TREN_10TR:
						data = data.Where(x => x.Price >= 10000000);
						break;
					default:
						break;
				}
			}

			var query = data.Skip((page - 1) * page_size).Take(page_size);
			var paginationSet = new PagedResult<ProductViewModel>() {
				Results = data.Select(a => a.ToModel()).ToList(),
				CurrentPage = page,
				RowCount = totalRow,
				PageSize = page_size
			};
			return paginationSet;
		}

		public async Task<ProductViewModel> GetByIdAsync(int id)
		{
			var data = await _productRepository.GetAByIdIncludeAsyn(x => x.Id == id);
			return data.ToModel();
		}

		public async Task<List<ProductViewModel>> GetRelatedOldProducts(int id,int top)
		{
			var product = await _productRepository.GetAByIdIncludeAsyn(x => x.Id == id);
			if(product == null) return null;
			var data = await _productRepository.FindAllAsync(x => x.Status == Status.Active && x.OldProduct == true
				  && x.Id != id && x.CategoryId == product.CategoryId);

			var listData = data.OrderByDescending(x => x.DateCreated)
				.Take(top)
				.Select(x => x.ToModel())
				.ToList();
			return listData;
		}

		public async Task<List<ProductImageViewModel>> GetImageAsync(int productId)
		{
			var data = await _productImageRepository.FindAllAsync(x => x.ProductId == productId);
			return data.Select(a => a.ProductImage()).ToList();
		}
	}
}