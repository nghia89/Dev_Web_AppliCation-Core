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
	public class SlideViewComponent : ViewComponent
	{
		private ICommonService _commonService;
		private IMemoryCache _memoryCache;

		public SlideViewComponent(ICommonService commonService,IMemoryCache memoryCache)
		{
			_memoryCache = memoryCache;
			_commonService = commonService;
		}

		public async Task<IViewComponentResult> InvokeAsync()
		{
			var data = await _memoryCache.GetOrCreateAsync(CacheKeys.Slides, async entry =>
			{
				entry.SlidingExpiration = TimeSpan.FromSeconds(12);
				return await _commonService.GetSlides("top");
			});
			return View(data);
		}
	}
}
