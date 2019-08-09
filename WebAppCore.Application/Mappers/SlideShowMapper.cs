using AutoMapper;
using System;
using System.Collections.Generic;
using System.Text;
using WebAppCore.Application.ViewModels.Common;
using WebAppCore.Data.Entities;

namespace WebAppCore.Application.Mappers
{
	public class SlideShowMapperProfile:Profile
	{
		public SlideShowMapperProfile()
		{
			CreateMap<Slide,SlideShowViewModel>().ReverseMap();
		}
	}
	public static class SlideShowMapper
	{
		internal static IMapper Mapper { get; }

		static SlideShowMapper()
		{
			Mapper = new MapperConfiguration(cfg => cfg.AddProfile<SlideShowMapperProfile>())
			   .CreateMapper();
		}

		public static SlideShowViewModel ToModel(this Slide slide)
		{
			return Mapper.Map<SlideShowViewModel>(slide);
		}

		public static Slide AddModel(this SlideShowViewModel slide)
		{
			return Mapper.Map<Slide>(slide);
		}
	}
}
