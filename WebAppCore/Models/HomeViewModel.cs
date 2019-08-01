using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebAppCore.Application.ViewModels.Blog;
using WebAppCore.Application.ViewModels.Common;
using WebAppCore.Application.ViewModels.Product;

namespace WebAppCore.Models
{
    public class HomeViewModel
    {
        public List<BlogViewModel> LastestBlogs { get; set; }
        public List<SlideShowViewModel> HomeSlides { get; set; }
        public List<ProductViewModel> HotProducts { get; set; }
        public List<ProductViewModel> TopSellProducts { get; set; }

        public List<ProductCategoryViewModel> HomeCategories { set; get; }

        public string Title { set; get; }
        public string MetaKeyword { set; get; }
        public string MetaDescription { set; get; }
    }
}
