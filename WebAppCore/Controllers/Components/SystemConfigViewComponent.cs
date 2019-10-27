using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using WebAppCore.Application.Interfaces;
using WebAppCore.Infrastructure.Enums;

namespace WebAppCore.Controllers.Components
{
	public class SystemConfigViewComponent: ViewComponent
	{
		private ISystemConfigService _systemConfigService;
		private IMemoryCache _memoryCache;

		public SystemConfigViewComponent(ISystemConfigService systemConfigService,IMemoryCache memoryCache)
		{
			_systemConfigService = systemConfigService;
			_memoryCache = memoryCache;
		}

		public async Task<IViewComponentResult> InvokeAsync()
		{
			var data = _memoryCache.GetOrCreate(CacheKeys.SystemConfig, entry =>
			{
				entry.SlidingExpiration = TimeSpan.FromSeconds(5);
				return  _systemConfigService.GetById();
			});
			return View(data);
		}
	}
}
