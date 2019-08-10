using AutoMapper;
using System;
using System.Collections.Generic;
using System.Text;
using WebAppCore.Application.ViewModels.System;
using WebAppCore.Data.Entities;

namespace WebAppCore.Application.Mappers
{
	public class AnnouncementMapperProfile:Profile
	{
		public AnnouncementMapperProfile()
		{
			CreateMap<Announcement,AnnouncementViewModel>().ReverseMap();
		}
	}
	public static class AnnouncementMapper
	{
		internal static IMapper Mapper { get; }

		static AnnouncementMapper()
		{
			Mapper = new MapperConfiguration(cfg => cfg.AddProfile<AnnouncementMapperProfile>())
			   .CreateMapper();
		}

		public static AnnouncementViewModel ToModel(this Announcement announcement)
		{
			return Mapper.Map<AnnouncementViewModel>(announcement);
		}

		public static Announcement AddModel(this AnnouncementViewModel announcement)
		{
			return Mapper.Map<Announcement>(announcement);
		}
	}
}
