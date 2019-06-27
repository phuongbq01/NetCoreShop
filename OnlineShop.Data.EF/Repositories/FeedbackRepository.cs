using OnlineShop.Data.Entities;
using OnlineShop.Data.IRepositories;
using System;
using System.Collections.Generic;
using System.Text;

namespace OnlineShop.Data.EF.Repositories
{
    public class FeedbackRepository : EFRepository<Feedback, int>, IFeedbackRepository
    {
        public FeedbackRepository(AppDbContext context) : base(context)
        {
        }
    }
}
