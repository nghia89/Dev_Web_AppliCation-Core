using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System.Linq;
using WebAppCore.Application.Interfaces;
using WebAppCore.Application.ViewModels.Blog;
using WebAppCore.Models;
using WebAppCore.Utilities.Dtos;

namespace WebAppCore.Controllers
{
	public class BlogsController:Controller
	{

		private IBlogService _blogService;
		private readonly IConfiguration _configuration;
		public BlogsController(IBlogService blogService,IConfiguration configuration)
		{
			_blogService = blogService;
			_configuration = configuration;
		}

		public IActionResult Index(string keywork,int? pageSize,int page = 1)
		{
			var blog = new BlogsViewModel();
			if(pageSize == null)
				pageSize = _configuration.GetValue<int>("PageSize");
			blog.PageSize = pageSize;
			var data = _blogService.GetAllPaging(keywork,page,pageSize.Value);
			var listData = data.Results.Select(a => BlogViewModel.form(a)).ToList();
			var paginationSet = new PagedResult<BlogViewModel>() {
				Results = listData,
				CurrentPage = page,
				RowCount = data.RowCount,
				PageSize = data.PageSize,
			};
			blog.Data = paginationSet;
			return View(blog);
		}

		[Route("{name}.{id}.html",Name = "Blog")]
		public IActionResult Detail(int id)
		{
			var dataVM = new blogsVM();
			var data = _blogService.GetById(id);
			var RelatedBlogs = _blogService.RelatedBlog(id,8);
			dataVM.Data = data;
			dataVM.RelatedBlogs = RelatedBlogs;
			return View(dataVM);
		}
	}
}