using System;
using System.Collections.Generic;
using System.Text;
using WebAppCore.Application.ViewModels.Common;

namespace WebAppCore.Application.Interfaces
{
   public interface ICommonService
    {
        FooterViewModel GetFooter();
        List<SlideViewModel> GetSlides(string groupAlias);
        SystemConfigViewModel GetSystemConfig(string code);
    }
}
