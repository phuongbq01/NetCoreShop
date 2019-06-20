using OnlineShop.Data.Entities;
using OnlineShop.Data.IRepositories;
using System;
using System.Collections.Generic;
using System.Text;

namespace OnlineShop.Data.EF.Repositories
{
    public class FunctionRepository : EFRepository<Function, string>, IFunctionRepository
    {
        public FunctionRepository(AppDbContext context) : base(context)
        {
        }
    }
}
