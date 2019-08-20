
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using WebAppCore.Application.Interfaces;
using WebAppCore.Application.ViewModels.Product;
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
		public IActionResult Index([FromQuery]int? pageSize,[FromQuery]string sortBy,[FromQuery] int? sortPrice,[FromQuery]int page = 1)
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
			var data = _productService.OldProductPage(page,pageSize.Value,sortBy,sortPrice);
			product.Data = data.Result;
			return View(product);
		}
	}
}