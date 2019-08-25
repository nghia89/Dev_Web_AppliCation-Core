using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using WebAppCore.Data.Enums;

namespace WebAppCore.Application.ViewModels.System
{
   public class AnnouncementViewModel
    {
        public string Id { get; set; }
        [Required]
        [StringLength(250)]
        public string Title { set; get; }

        [StringLength(250)]
        public string Content { set; get; }

        public Guid UserId { set; get; }

        public DateTime DateCreated { set; get; }
        public DateTime DateModified { set; get; }
        public Status Status { set; get; }
		public string Avartar { get; set; }
	}
}
