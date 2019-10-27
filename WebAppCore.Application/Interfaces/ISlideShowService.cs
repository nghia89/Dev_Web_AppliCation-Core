using System;
using System.Collections.Generic;
using System.Text;
using WebAppCore.Application.ViewModels.Common;
using WebAppCore.Utilities.Dtos;

namespace WebAppCore.Application.Interfaces
{
    public interface ISlideShowService
    {
        void Create(SlideShowViewModel slideVM);
        void Update(SlideShowViewModel slideVM);
		SlideShowViewModel GetById(int id);
		PagedResult<SlideShowViewModel> GetAllPaging(int pageIndex, int pageSize);

        void DeleteDetail(int id);

        void Save();
    }
}
