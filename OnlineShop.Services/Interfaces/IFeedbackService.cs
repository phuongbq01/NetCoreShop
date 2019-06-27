using OnlineShop.Services.ViewModels.Common;
using OnlineShop.Utilities.DTO;
using System;
using System.Collections.Generic;
using System.Text;

namespace OnlineShop.Services.Interfaces
{
    public interface IFeedbackService : IDisposable
    {
        void Add(FeedbackViewModel feedbackVm);

        void Update(FeedbackViewModel feedbackVm);

        void Delete(int id);

        List<FeedbackViewModel> GetAll();

        PagedResult<FeedbackViewModel> GetAllPaging(string keyword, int page, int pageSize);

        FeedbackViewModel GetById(int id);

        void SaveChanges();
    }
}
