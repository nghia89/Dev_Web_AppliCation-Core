using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using WebAppCore.Application.ViewModels.System;
using WebAppCore.Data.Entities;

namespace WebAppCore.Application.Interfaces
{
	public interface IAnnouncementUserService
	{
		Task Add(AnnouncementUserViewModel announcement);
	}
}
