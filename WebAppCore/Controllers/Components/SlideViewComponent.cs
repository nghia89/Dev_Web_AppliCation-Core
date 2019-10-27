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
		private ISlideShowService _slideShowService;
		private ICommonService _commonService;
		private IMemoryCache _memoryCache;

		public SlideViewComponent(ISlideShowService slideShowService,ICommonService commonService,IMemoryCache memoryCache)
		{
			_slideShowService = slideShowService;
			_memoryCache = memoryCache;
			_commonService = commonService;
		}

		public async Task<IViewComponentResult> InvokeAsync()
		{
			var data =await  _commonService.GetSlides("top");
			return View(data);
		}
	}
}
