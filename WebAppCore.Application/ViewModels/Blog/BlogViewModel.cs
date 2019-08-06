using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using WebAppCore.Data.Enums;
using WebAppCore.Utilities.Extensions;

namespace WebAppCore.Application.ViewModels.Blog
{
	public class BlogViewModel
	{
		public int Id { set; get; }
		[Required]
		[MaxLength(256)]
		public string Name { set; get; }


		[MaxLength(256)]
		public string Image { set; get; }

		[MaxLength(500)]
		public string Description { set; get; }

		public string Content { set; get; }

		public bool? HomeFlag { set; get; }
		public bool? HotFlag { set; get; }
		public int? ViewCount { set; get; }

		public string Tags { get; set; }

		public List<BlogTagViewModel> BlogTags { set; get; }
		public DateTime DateCreated { set; get; }
		public DateTime DateModified { set; get; }
		public Status Status { set; get; }

		[MaxLength(256)]
		public string SeoPageTitle { set; get; }

		[MaxLength(256)]
		public string SeoAlias { set; get; }

		[MaxLength(256)]
		public string SeoKeywords { set; get; }

		[MaxLength(256)]
		public string SeoDescription { set; get; }

		public string TimeAgo { get; set; }

		public static BlogViewModel form(BlogViewModel model)
		{
			if(model == null) return null;
			BlogViewModel result = new BlogViewModel {
				Id = model.Id,
				Name = model.Name,
				BlogTags = model.BlogTags,
				Content = model.Content,
				DateCreated = model.DateCreated,
				DateModified = model.DateModified,
				Description = model.Description,
				HomeFlag = model.HomeFlag,
				HotFlag = model.HotFlag,
				Image = model.Image,
				SeoAlias = model.SeoAlias,
				SeoDescription = model.SeoDescription,
				SeoKeywords = model.SeoKeywords,
				SeoPageTitle = model.SeoPageTitle,
				Status = model.Status,
				Tags = model.Tags,
				TimeAgo = RelativeDate.TimeAgo(model.DateCreated)
			};
			return result;
		}
	}
}
