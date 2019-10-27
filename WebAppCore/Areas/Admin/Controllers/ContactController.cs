using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.Collections.Generic;
using System.Linq;
using WebAppCore.Application.Interfaces;
using WebAppCore.Application.ViewModels.Common;

namespace WebAppCore.Areas.Admin.Controllers
{
	public class ContactController:BaseController
	{
		private readonly IContactService _contactService;

		public ContactController(IContactService contactService)
		{
			_contactService = contactService;
		}
		public IActionResult Index()
		{
			return View();
		}

		[HttpGet]
		public IActionResult GetById(string id)
		{
			var model = _contactService.GetById(id);

			return new OkObjectResult(model);
		}

		public IActionResult GetAllPaging(int page,int pageSize)
		{
			var data = _contactService.GetAllPaging(null,page,pageSize);
			return new OkObjectResult(data);
		}

		public IActionResult Delete(string id)
		{
			_contactService.Delete(id);
			_contactService.SaveChanges();

			return new OkObjectResult(id);
		}

		public IActionResult SaveEntity(ContactViewModel conact)
		{
			if(!ModelState.IsValid)
			{
				IEnumerable<ModelError> allErrors = ModelState.Values.SelectMany(v => v.Errors);
				return new BadRequestObjectResult(allErrors);
			}
			if(string.IsNullOrEmpty(conact.Id) || conact.Id == "0")
			{
				_contactService.Create(conact);
			}
			else
			{
				_contactService.Update(conact);
			}
			_contactService.SaveChanges();
			return new OkObjectResult(conact);
		}
	}
}