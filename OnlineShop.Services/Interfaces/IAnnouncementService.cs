using OnlineShop.Services.ViewModels.system;
using OnlineShop.Utilities.DTO;
using System;
using System.Collections.Generic;
using System.Text;

namespace OnlineShop.Services.Interfaces
{
    public interface IAnnouncementService : IDisposable
    {
        PagedResult<AnnouncementViewModel> GetAllUnReadPaging(Guid userId, int pageIndex, int pageSize);

        bool MarkAsRead(Guid userId, string id);

        void Save();
    }
}
