using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using WebAppCore.Application.Interfaces;
using WebAppCore.Application.ViewModels.Common;

namespace WebAppCore.Areas.Admin.Controllers
{
    public class SlideShowController : Controller
    {
        private readonly ISlideShowService _slideShowService;

        public SlideShowController(ISlideShowService slideShowService)
        {
            _slideShowService = slideShowService;
        }
        public IActionResult Index()
        {
            return View();
        }

        //[HttpGet]
        //public IActionResult GetById(int id)
        //{
        //    var model = _slideShowService.GetDetail(id);

        //    return new OkObjectResult(model);
        //}

        public IActionResult SaveEntity(SlideShowViewModel slideShow)
        {
            if (!ModelState.IsValid)
            {
                IEnumerable<ModelError> allErrors = ModelState.Values.SelectMany(v => v.Errors);
                return new BadRequestObjectResult(allErrors);
            }
            if (slideShow.Id == 0)
            {
                _slideShowService.Create(slideShow);
            }
            else
            {
                _slideShowService.Update(slideShow);
            }
            _slideShowService.Save();
            return new OkObjectResult(slideShow);
        }
    }
}