using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using WebAppCore.Application.ViewModels.System;
using WebAppCore.Data.Entities;
using WebAppCore.Utilities.Dtos;

namespace WebAppCore.Application.Interfaces
{
  public  interface IAnnouncementService
    {
        PagedResult<AnnouncementViewModel> GetAllUnReadPaging(Guid userId, int pageIndex, int pageSize);

        bool MarkAsRead(Guid userId, string id);

		Task<bool> AnnounSendUser(Guid userId,AnnouncementViewModel content,List<AppUserViewModel> appUsers);
    }
}
