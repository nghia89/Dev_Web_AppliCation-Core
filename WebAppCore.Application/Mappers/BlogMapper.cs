using AutoMapper;
using System;
using System.Collections.Generic;
using System.Text;
using WebAppCore.Application.ViewModels.Blog;
using WebAppCore.Data.Entities;

namespace WebAppCore.Application.Mappers
{

	public class BlogMapperProfile:Profile
	{
		public BlogMapperProfile()
		{
			CreateMap<Blog,BlogViewModel>().ReverseMap();
		}
	}
	public static class BlogMapper
	{
		internal static IMapper Mapper { get; }

		static BlogMapper()
		{
			Mapper = new MapperConfiguration(cfg => cfg.AddProfile<BlogMapperProfile>())
			   .CreateMapper();
		}

		public static BlogViewModel ToModel(this Blog blog)
		{
			return Mapper.Map<BlogViewModel>(blog);
		}

		public static Blog AddModel(this BlogViewModel blog)
		{
			return Mapper.Map<Blog>(blog);
		}
	}
}
