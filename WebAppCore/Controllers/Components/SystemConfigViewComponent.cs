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
    public class SystemConfigViewComponent : ViewComponent
    {
        private ISystemConfigService _systemConfigService;
        private IMemoryCache _memoryCache;

        public SystemConfigViewComponent(ISystemConfigService systemConfigService, IMemoryCache memoryCache)
        {
            _systemConfigService = systemConfigService;
            _memoryCache = memoryCache;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var data = await _memoryCache.GetOrCreateAsync(CacheKeys.SystemConfig, async entry =>
              {
                  entry.SlidingExpiration = TimeSpan.FromSeconds(24);
                  return await _systemConfigService.GetById();
              });
            return View(data);
        }
    }
}
