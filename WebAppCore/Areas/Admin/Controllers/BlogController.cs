using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebAppCore.Application.Interfaces;
using WebAppCore.Application.ViewModels.Blog;
using WebAppCore.Utilities.Helpers;

namespace WebAppCore.Areas.Admin.Controllers
{
	public class BlogController:BaseController
	{
		public IBlogService _blogService;

		public BlogController(IBlogService blogService)
		{
			_blogService = blogService;
		}

		public IActionResult Index()
		{
			return View();
		}
		public IActionResult GetAll()
		{
			var model = _blogService.GetAll();
			return new OkObjectResult(model);
		}
		[HttpGet]
		public IActionResult GetById(int id)
		{
			var model = _blogService.GetById(id);
			return new OkObjectResult(model);
		}
		[HttpGet]
		public IActionResult GetAllPaging(string keyword,int page,int pageSize)
		{
			var model = _blogService.GetAllPaging(keyword,page,pageSize);
			return new OkObjectResult(model);
		}
		[HttpPost]
		public IActionResult SaveEntity(BlogViewModel pageVm)
		{
			if(!ModelState.IsValid)
			{
				IEnumerable<ModelError> allErrors = ModelState.Values.SelectMany(v => v.Errors);
				return new BadRequestObjectResult(allErrors);
			}
			if(pageVm.Id == 0)
			{
				pageVm.SeoAlias = TextHelper.ToUnsignString(pageVm.Name);
				_blogService.Add(pageVm);
			}
			else
			{
				pageVm.SeoAlias = TextHelper.ToUnsignString(pageVm.Name);
				_blogService.Update(pageVm);
			}
			_blogService.Save();
			return new OkObjectResult(pageVm);
		}
		[HttpPost]
		public IActionResult Delete(int id)
		{
			if(!ModelState.IsValid)
			{
				return new BadRequestObjectResult(ModelState);
			}
			_blogService.Delete(id);
			_blogService.Save();
			return new OkObjectResult(id);
		}
	}
}
