using AutoMapper;
using System;
using System.Collections.Generic;
using System.Text;
using WebAppCore.Application.ViewModels.Product;
using WebAppCore.Data.Entities;

namespace WebAppCore.Application.Mappers
{
	public class WholePriceMapperProfile:Profile
	{
		public WholePriceMapperProfile()
		{
			CreateMap<WholePrice,WholePriceViewModel>().ReverseMap();
		}
	}
	public static class WholePriceMapper
	{
		internal static IMapper Mapper { get; }

		static WholePriceMapper()
		{
			Mapper = new MapperConfiguration(cfg => cfg.AddProfile<WholePriceMapperProfile>())
			   .CreateMapper();
		}

		public static WholePriceViewModel ToModel(this WholePrice wholePrice)
		{
			return Mapper.Map<WholePriceViewModel>(wholePrice);
		}
	}
}
