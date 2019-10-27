using System;
using System.Collections.Generic;
using System.Text;
using WebAppCore.Data.Enums;

namespace WebAppCore.Application.ViewModels.Common
{
    public class SystemConfigViewModel
    {
		public int Id { get; set; }
		public string Title { get; set; }
		public string Keywords { get; set; }
		public string Description { get; set; }
		public string Copyright { get; set; }
		public string Author { get; set; }
		public string Logo { get; set; }
	}
}
