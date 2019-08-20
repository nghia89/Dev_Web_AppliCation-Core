using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebAppCore.Application.ViewModels.Product;
using WebAppCore.Infrastructure.Enums;
using WebAppCore.Utilities.Dtos;

namespace WebAppCore.Models.ProductViewModels
{
	public class OldProductViewModels
	{
		public PagedResult<ProductViewModel> Data { get; set; }

		public ProductCategoryViewModel Category { set; get; }

		public string SortType { set; get; }

		public int? PageSize { set; get; }

		public List<SelectListItem> SortTypes { get; } = new List<SelectListItem>
		{
			new SelectListItem(){Value = "lastest",Text = "Mới nhất"},
			new SelectListItem(){Value = "price",Text = "Giá"},
			new SelectListItem(){Value = "name",Text = "Tên"},
		};

		public List<SelectListItem> PageSizes { get; } = new List<SelectListItem>
		{
			new SelectListItem(){Value = "12",Text = "12"},
			new SelectListItem(){Value = "22",Text = "22"},
			new SelectListItem(){Value = "42",Text = "42"},
		};

		public List<PriceCheckBox> SortPrice { get; } = new List<PriceCheckBox> {
			new PriceCheckBox(){Id=1,Selected=false,Name="Dưới 500",Value=(int)PriceEnum.DUOI_500},
			new PriceCheckBox(){Id=2,Selected=false,Name="Từ 1 - 2 triệu",Value=(int)PriceEnum.TU_1TR_DEN_2TR},
			new PriceCheckBox(){Id=3,Selected=false,Name="Từ 2 - 4 triệu",Value=(int)PriceEnum.TU_2TR_DEN_4TR},
			new PriceCheckBox(){Id=4,Selected=false,Name="Từ 4 - 6 triệu",Value=(int)PriceEnum.TU_4TR_DEN_6TR},
			new PriceCheckBox(){Id=5,Selected=false,Name="Từ 6 - 10 triệu",Value=(int)PriceEnum.TU_6TR_DEN_10TR},
			new PriceCheckBox(){Id=6,Selected=false,Name="Trên 10 triệu",Value=(int)PriceEnum.TREN_10TR}
		};
	}
}
