using AutoMapper;
using WebAppCore.Application.ViewModels.Product;
using WebAppCore.Data.Entities;

namespace WebAppCore.Application.Mappers
{
	public class SizeMapperProfile:Profile
	{
		public SizeMapperProfile()
		{
			CreateMap<Size,SizeViewModel>().ReverseMap();
		}
	}
	public static class SizeMapper
	{
		internal static IMapper Mapper { get; }

		static SizeMapper()
		{
			Mapper = new MapperConfiguration(cfg => cfg.AddProfile<SizeMapperProfile>())
			   .CreateMapper();
		}

		public static SizeViewModel ToModel(this Size bill)
		{
			return Mapper.Map<SizeViewModel>(bill);
		}
		public static Size AddModel(this SizeViewModel addModel)
		{
			return Mapper.Map<Size>(addModel);
		}
	}
}
