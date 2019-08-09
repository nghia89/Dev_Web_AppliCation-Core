using AutoMapper;
using System;
using System.Collections.Generic;
using System.Text;
using WebAppCore.Application.ViewModels.System;
using WebAppCore.Data.Entities;

namespace WebAppCore.Application.Mappers
{
	public class AnnouncementUserMapperProfile:Profile
	{
		public AnnouncementUserMapperProfile()
		{
			CreateMap<AnnouncementUser,AnnouncementUserViewModel>().ReverseMap();
		}
	}
	public static class AnnouncementUserMapper
	{
		internal static IMapper Mapper { get; }

		static AnnouncementUserMapper()
		{
			Mapper = new MapperConfiguration(cfg => cfg.AddProfile<AnnouncementUserMapperProfile>())
			   .CreateMapper();
		}

		public static AnnouncementUserViewModel ToModel(this AnnouncementUser announcementUser)
		{
			return Mapper.Map<AnnouncementUserViewModel>(announcementUser);
		}

		public static AnnouncementUser AddModel(this AnnouncementUserViewModel announcementUser)
		{
			return Mapper.Map<AnnouncementUser>(announcementUser);
		}
	}
}
