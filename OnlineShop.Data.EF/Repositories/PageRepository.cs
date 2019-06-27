using OnlineShop.Data.Entities;
using OnlineShop.Data.IRepositories;
using System;
using System.Collections.Generic;
using System.Text;

namespace OnlineShop.Data.EF.Repositories
{
    public class PageRepository : EFRepository<Page, int>, IPageRepository
    {
        public PageRepository(AppDbContext context) : base(context)
        {
        }
    }
}
