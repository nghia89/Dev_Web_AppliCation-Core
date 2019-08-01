using System;
using System.Collections.Generic;
using System.Text;

namespace WebAppCore.Application.ViewModels.Common
{
   public class SlideShowViewModel
    {
        public int Id { set; get; }
        public string Name { set; get; }
        public string Description { set; get; }
        public string Image { set; get; }
        public string Url { set; get; }
        public int? DisplayOrder { set; get; }
        public bool Status { set; get; }
        public string Content { set; get; }
        public string GroupAlias { get; set; }
    }
}
