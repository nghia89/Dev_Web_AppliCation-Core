using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebAppCore.Application.Interfaces;
using WebAppCore.Application.ViewModels.Common;
using WebAppCore.Data.EF;
using WebAppCore.Data.Entities;

namespace WebAppCore.Areas.Admin.Controllers
{
	public class SettingController : BaseController
	{
		private readonly ISystemConfigService _systemConfigService;


		public SettingController(ISystemConfigService systemConfigService)
		{
			this._systemConfigService = systemConfigService;

		}
		public IActionResult Index()
		{
			return View();
		}

		[HttpGet]
		public async Task<IActionResult> GetById()
		{

			var model = await _systemConfigService.GetByIdOrDefault();

			return new OkObjectResult(model);
		}
	
		public IActionResult SaveEntity(SystemConfigViewModel model)
		{
			if(!ModelState.IsValid)
			{
				IEnumerable<ModelError> allErrors = ModelState.Values.SelectMany(v => v.Errors);
				return new BadRequestObjectResult(allErrors);
			}
			if(model.Id == 0)
			{
				_systemConfigService.Create(model);
			}
			else
			{
				_systemConfigService.Update(model);
			}
			_systemConfigService.Save();
			return new OkObjectResult(model);
		}
	}
}
