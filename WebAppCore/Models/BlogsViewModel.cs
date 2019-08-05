

using System;
using System.Collections.Generic;
using WebAppCore.Application.ViewModels.Blog;
using WebAppCore.Data.Enums;
using WebAppCore.Extensions;
using WebAppCore.Utilities.Dtos;

namespace WebAppCore.Models
{
	public class BlogsViewModel
	{
		public PagedResult<BlogViewModel> Data { get; set; }
		public int? PageSize { set; get; }
	}
}
