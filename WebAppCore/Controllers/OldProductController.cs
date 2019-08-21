
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System.Threading.Tasks;
using WebAppCore.Application.Interfaces;
using WebAppCore.Models.ProductViewModels;

namespace WebAppCore.Controllers
{
	public class OldProductController:Controller
	{
		private IProductService _productService;
		private IProductCategoryService _productCategoryService;
		private IConfiguration _configuration;

		public OldProductController(IProductService productService,IConfiguration configuration,
			IProductCategoryService productCategoryService)
		{
			_productService = productService;
			_productCategoryService = productCategoryService;
			_configuration = configuration;
		}
		[Route("san-pham-cu.html")]
		public async Task<IActionResult> Index([FromQuery]int? pageSize,[FromQuery]string sortBy,[FromQuery] int? sortPrice,[FromQuery]int page = 1)
		{
			var product = new OldProductViewModels();
			if(pageSize == null)
				pageSize = _configuration.GetValue<int>("PageSize");
			product.PageSize = pageSize;
			product.SortType = sortBy;
			product.SortType = sortBy;
			foreach(var item in product.SortPrice)
			{
				if(item.Value == sortPrice)
				{
					item.Selected = true;
				}
				else
				{
					item.Selected = false;
				}
			}
			var data =await _productService.OldProductPage(page,pageSize.Value,sortBy,sortPrice);
			product.Data = data;
			return View(product);
		}

		[Route("san-pham-cu/{alias}-spc.{id}.html",Name = "OldProductDetail")]
		public async Task<IActionResult> Detail(int id)
		{
			ViewData["BodyClass"] = "product-page";
			var model = new DetailViewModel();
			model.Product =await _productService.GetByIdAsync(id);
			model.Category = await _productCategoryService.GetById(model.Product.CategoryId);
			model.RelatedProducts =await _productService.GetRelatedOldProducts(id,9);
			//model.UpsellProducts = _productService.GetUpsellProducts(6);
			model.ProductImages =await _productService.GetImageAsync(id);
			model.Tags = _productService.GetProductTags(id);
			return View(model);
		}
	}
}