using AutoMapper;
using WebAppCore.Application.ViewModels.Product;
using WebAppCore.Data.Entities;

namespace WebAppCore.Application.Mappers
{
	public class ColorMapperProfile:Profile
	{
		public ColorMapperProfile()
		{
			CreateMap<Color,ColorViewModel>().ReverseMap();
		}
	}
	public static class ColorMapper
	{
		internal static IMapper Mapper { get; }

		static ColorMapper()
		{
			Mapper = new MapperConfiguration(cfg => cfg.AddProfile<ColorMapperProfile>())
			   .CreateMapper();
		}

		public static ColorViewModel ToModel(this Color bill)
		{
			return Mapper.Map<ColorViewModel>(bill);
		}
		public static Color AddModel(this ColorViewModel addModel)
		{
			return Mapper.Map<Color>(addModel);
		}
	}
}
