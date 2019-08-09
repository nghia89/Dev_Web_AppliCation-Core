using AutoMapper;
using System;
using System.Collections.Generic;
using System.Text;
using WebAppCore.Application.ViewModels.System;
using WebAppCore.Data.Entities;

namespace WebAppCore.Application.Mappers
{
	public class FunctionMapperProfile:Profile
	{
		public FunctionMapperProfile()
		{
			CreateMap<Function,FunctionViewModel>().ReverseMap();
		}
	}
	public static class FunctionMapper
	{
		internal static IMapper Mapper { get; }

		static FunctionMapper()
		{
			Mapper = new MapperConfiguration(cfg => cfg.AddProfile<FunctionMapperProfile>())
			   .CreateMapper();
		}

		public static FunctionViewModel ToModel(this Function function)
		{
			return Mapper.Map<FunctionViewModel>(function);
		}

		public static Function AddModel(this FunctionViewModel function)
		{
			return Mapper.Map<Function>(function);
		}
	}
}
