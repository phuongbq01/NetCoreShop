using OnlineShop.Data.Entities;
using OnlineShop.Data.IRepositories;
using System;
using System.Collections.Generic;
using System.Text;

namespace OnlineShop.Data.EF.Repositories
{
    public class FooterRepository : EFRepository<Footer, string>, IFooterRepository
    {
        public FooterRepository(AppDbContext context) : base(context)
        {
        }
    }
}
