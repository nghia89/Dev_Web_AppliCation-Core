using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Configuration;
using System.Linq;
using System.Threading.Tasks;
using WebAppCore.Application.Interfaces;
using WebAppCore.Models.ProductViewModels;

namespace WebAppCore.Controllers
{
    public class ProductController : Controller
    {
        private IProductService _productService;
        private IBillService _billService;
        private IProductCategoryService _productCategoryService;
        private IConfiguration _configuration;

        public ProductController(IProductService productService, IConfiguration configuration,
            IBillService billService,
            IProductCategoryService productCategoryService)
        {
            _productService = productService;
            _productCategoryService = productCategoryService;
            _configuration = configuration;
            _billService = billService;
        }

        [Route("products.html")]
        public IActionResult Index()
        {
            var categories = _productCategoryService.GetAll();
            return View(categories);
        }

        [Route("{alias}-c.{id}.html")]
        public async Task<IActionResult> Catalog(int id,[FromQuery]int? pageSize,[FromQuery]string sortBy,[FromQuery] int? sortprice,[FromQuery]int page = 1 )
        {
            var catalog = new CatalogViewModel();
            ViewData["BodyClass"] = "shop_grid_full_width_page";
            if (pageSize == null)
                pageSize = _configuration.GetValue<int>("PageSize");

            catalog.PageSize = pageSize;
            catalog.SortType = sortBy;
            catalog.SortType = sortBy;
			foreach(var item in catalog.SortPrice)
			{
				if(item.Value== sortprice)
				{
					item.Selected = true;
					break;
				}
			}
            catalog.Data = _productService.GetAllPaging(id, string.Empty, page, pageSize.Value, sortBy,sortprice);
            catalog.Category =await _productCategoryService.GetById(id);
            return View(catalog);
        }

        [Route("search.html")]
        public IActionResult Search(string keyword, int? pageSize, string sortBy, int page = 1)
        {
            var catalog = new SearchResultViewModel();
            ViewData["BodyClass"] = "shop_grid_full_width_page";
            if (pageSize == null)
                pageSize = _configuration.GetValue<int>("PageSize");

            catalog.PageSize = pageSize;
            catalog.SortType = sortBy;
            catalog.Data = _productService.GetAllPaging(null, keyword, page, pageSize.Value, string.Empty,null);
            catalog.Keyword = keyword;

            return View(catalog);
        }

        [Route("{alias}-p.{id}.html", Name = "ProductDetail")]
        public async Task<IActionResult> Detail(int id)
        {
            ViewData["BodyClass"] = "product-page";
            var model = new DetailViewModel();
            model.Product = _productService.GetById(id);
            model.Category =await _productCategoryService.GetById(model.Product.CategoryId);
            model.RelatedProducts = _productService.GetRelatedProducts(id, 9);
            model.UpsellProducts = _productService.GetUpsellProducts(6);
            model.ProductImages = _productService.GetImages(id);
            model.Tags = _productService.GetProductTags(id);
            model.Colors = _billService.GetColors().Select(x => new SelectListItem()
            {
                Text = x.Name,
                Value = x.Id.ToString()
            }).ToList();
            model.Sizes = _billService.GetSizes().Select(x => new SelectListItem()
            {
                Text = x.Name,
                Value = x.Id.ToString()
            }).ToList();
            return View(model);
        }
    }
}