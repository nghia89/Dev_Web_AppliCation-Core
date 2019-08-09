using AutoMapper;
using System;
using System.Collections.Generic;
using System.Text;
using WebAppCore.Application.ViewModels.Product;
using WebAppCore.Data.Entities;

namespace WebAppCore.Application.Mappers
{
	public class ProductQuantityMapperProfile:Profile
	{
		public ProductQuantityMapperProfile()
		{
			CreateMap<ProductQuantity,ProductQuantityViewModel>().ReverseMap();
		}
	}
	public static class ProductQuantityMapper
	{
		internal static IMapper Mapper { get; }

		static ProductQuantityMapper()
		{
			Mapper = new MapperConfiguration(cfg => cfg.AddProfile<ProductQuantityMapperProfile>())
			   .CreateMapper();
		}

		public static ProductQuantityViewModel ToModel(this ProductQuantity productQuantity)
		{
			return Mapper.Map<ProductQuantityViewModel>(productQuantity);
		}
	}
}
