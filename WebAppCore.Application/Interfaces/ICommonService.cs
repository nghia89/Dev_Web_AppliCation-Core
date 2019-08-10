using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using WebAppCore.Application.ViewModels.Common;

namespace WebAppCore.Application.Interfaces
{
   public interface ICommonService
    {
        FooterViewModel GetFooter();
        Task<List<SlideShowViewModel>> GetSlides(string groupAlias);
        SystemConfigViewModel GetSystemConfig(string code);
    }
}
