using OnlineShop.Data.Entities;
using OnlineShop.Data.IRepositories;
using OnlineShop.Infrastructure.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace OnlineShop.Data.EF.Repositories
{
    public class ContactRepository : EFRepository<Contact, string>, IContactRepository
    {
        public ContactRepository(AppDbContext context) : base(context)
        {
        }
    }
}
