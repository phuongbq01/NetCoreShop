using OnlineShop.Data.Entities;
using OnlineShop.Data.IRepositories;
using System;
using System.Collections.Generic;
using System.Text;

namespace OnlineShop.Data.EF.Repositories
{
    public class AnnouncementUserRepository : EFRepository<AnnouncementUser, int>, IAnnouncementUserRepository
    {
        public AnnouncementUserRepository(AppDbContext context) : base(context)
        {
        }
    }
}
