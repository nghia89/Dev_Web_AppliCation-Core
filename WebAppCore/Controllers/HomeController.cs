using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Threading.Tasks;
using WebAppCore.Application.Interfaces;
using WebAppCore.Models;

namespace WebAppCore.Controllers
{
	public class HomeController:Controller
	{
		private IProductService _productService;
		private IProductCategoryService _productCategoryService;

		private IBlogService _blogService;
		private ICommonService _commonService;
		//  private readonly IStringLocalizer<HomeController> _localizer;

		public HomeController(IProductService productService,
	   IBlogService blogService,ICommonService commonService,
	  IProductCategoryService productCategoryService)
		{
			_blogService = blogService;
			_commonService = commonService;
			_productService = productService;
			_productCategoryService = productCategoryService;
			//_localizer = localizer;
		}

		//[ResponseCache(CacheProfileName = "Default")]
		public async Task<IActionResult> Index()
		{
			//var title = _localizer["Title"];
			//var culture = HttpContext.Features.Get<IRequestCultureFeature>().RequestCulture.Culture.Name;
			ViewData["BodyClass"] = "cms-index-index cms-home-page";
			var homeVm = new HomeViewModel();
			homeVm.HomeCategories = await _productCategoryService.GetHomeCategories(5);
			homeVm.BuyALotProducts = await _productService.GetHotProduct(5);
			//homeVm.HotProducts = await _productService.GetHotProduct(5);
			//homeVm.TopSellProducts = _productService.GetLastest(5);
			homeVm.NewSellProducts = await _productService.GetProductNew(8);
			homeVm.LastestBlogs = await _blogService.GetLastest(5);
			homeVm.HomeSlides =await _commonService.GetSlides("top");
			return View(homeVm);
		}

		public IActionResult About()
		{
			ViewData["Message"] = "Your application description page.";

			return View();
		}

		public IActionResult Contact()
		{
			ViewData["Message"] = "Your contact page.";

			return View();
		}

		public IActionResult Error()
		{
			return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
		}
	}
}