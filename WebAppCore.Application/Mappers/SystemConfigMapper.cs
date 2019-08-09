using AutoMapper;
using System;
using System.Collections.Generic;
using System.Text;
using WebAppCore.Application.ViewModels.Common;
using WebAppCore.Data.Entities;

namespace WebAppCore.Application.Mappers
{
	public class SystemConfigMapperProfile:Profile
	{
		public SystemConfigMapperProfile()
		{
			CreateMap<SystemConfig,SystemConfigViewModel>().ReverseMap();
		}
	}
	public static class SystemConfigMapper
	{
		internal static IMapper Mapper { get; }

		static SystemConfigMapper()
		{
			Mapper = new MapperConfiguration(cfg => cfg.AddProfile<SystemConfigMapperProfile>())
			   .CreateMapper();
		}

		public static SystemConfigViewModel ToModel(this SystemConfig systemConfig)
		{
			return Mapper.Map<SystemConfigViewModel>(systemConfig);
		}
		public static SystemConfig AddModel(this SystemConfigViewModel addModel)
		{
			return Mapper.Map<SystemConfig>(addModel);
		}
	}
}
