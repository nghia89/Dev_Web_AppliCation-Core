using AutoMapper;
using System;
using System.Collections.Generic;
using System.Text;
using WebAppCore.Application.ViewModels.Product;
using WebAppCore.Data.Entities;

namespace WebAppCore.Application.Mappers
{
	public class ProductTagMapperProfile:Profile
	{
		public ProductTagMapperProfile()
		{
			CreateMap<ProductTag,ProductTagViewModel>().ReverseMap();
		}
	}
	public static class ProductTagMapper
	{
		internal static IMapper Mapper { get; }

		static ProductTagMapper()
		{
			Mapper = new MapperConfiguration(cfg => cfg.AddProfile<ProductTagMapperProfile>())
			   .CreateMapper();
		}

		public static ProductTagViewModel ToModel(this ProductTag ProductTag)
		{
			return Mapper.Map<ProductTagViewModel>(ProductTag);
		}
	}
}
