using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebAppCore.Application.Interfaces;
using WebAppCore.Infrastructure.Enums;

namespace WebAppCore.Controllers.Components
{
    public class MainMenuViewComponent : ViewComponent
    {
        private IProductCategoryService _productCategoryService;
        private readonly IMemoryCache _memoryCache;
        public MainMenuViewComponent(IProductCategoryService productCategoryService, IMemoryCache memoryCache)
        {
            _productCategoryService = productCategoryService;
            _memoryCache = memoryCache;
        }
        public async Task<IViewComponentResult> InvokeAsync()
        {
            var categories = await _memoryCache.GetOrCreateAsync(CacheKeys.ProductCategories, async entry =>
            {
                entry.SlidingExpiration = TimeSpan.FromSeconds(12);
                return await _productCategoryService.GetAll();
            });
            return View(categories);
        }
    }
}
