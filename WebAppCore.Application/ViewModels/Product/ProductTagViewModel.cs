using System;
using System.Collections.Generic;
using System.Text;
using WebAppCore.Data.Entities;

namespace WebAppCore.Application.ViewModels.Product
{
	public  class ProductTagViewModel
	{
		public int Id { get; set; }

		public int ProductId { get; set; }

		public string TagId { set; get; }

		public ProductViewModel Products { set; get; }

		public virtual Tag Tag { set; get; }
	}
}
