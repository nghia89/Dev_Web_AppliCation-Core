using AutoMapper;
using System;
using System.Collections.Generic;
using System.Text;
using WebAppCore.Application.ViewModels.Blog;
using WebAppCore.Data.Entities;

namespace WebAppCore.Application.Mappers
{
	public class PageMapperProfile:Profile
	{
		public PageMapperProfile()
		{
			CreateMap<Page,PageViewModel>().ReverseMap();
		}
	}
	public static class PageMapper
	{
		internal static IMapper Mapper { get; }

		static PageMapper()
		{
			Mapper = new MapperConfiguration(cfg => cfg.AddProfile<PageMapperProfile>())
			   .CreateMapper();
		}

		public static PageViewModel ToModel(this Page page)
		{
			return Mapper.Map<PageViewModel>(page);
		}

		public static Page AddModel(this PageViewModel page)
		{
			return Mapper.Map<Page>(page);
		}
	}
}
