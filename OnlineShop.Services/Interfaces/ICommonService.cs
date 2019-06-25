using OnlineShop.Services.ViewModels.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace OnlineShop.Services.Interfaces
{
    public interface ICommonService : IDisposable
    {
        FooterViewModel GetFooter();
        List<SlideViewModel> GetSlides(string groupAlias);
        SystemConfigViewModel GetSystemConfig(string code);
    }
}
