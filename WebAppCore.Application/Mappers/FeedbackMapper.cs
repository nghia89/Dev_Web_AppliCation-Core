using AutoMapper;
using System;
using System.Collections.Generic;
using System.Text;
using WebAppCore.Application.ViewModels.Common;
using WebAppCore.Data.Entities;

namespace WebAppCore.Application.Mappers
{
	public class FeedbackMapperProfile:Profile
	{
		public FeedbackMapperProfile()
		{
			CreateMap<Feedback,FeedbackViewModel>().ReverseMap();
		}
	}
	public static class FeedbackMapper
	{
		internal static IMapper Mapper { get; }

		static FeedbackMapper()
		{
			Mapper = new MapperConfiguration(cfg => cfg.AddProfile<FeedbackMapperProfile>())
			   .CreateMapper();
		}

		public static FeedbackViewModel ToModel(this Feedback feedback)
		{
			return Mapper.Map<FeedbackViewModel>(feedback);
		}

		public static Feedback AddModel(this FeedbackViewModel feedback)
		{
			return Mapper.Map<Feedback>(feedback);
		}
	}
}
