using OnlineShop.Data.Entities;
using OnlineShop.Infrastructure.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace OnlineShop.Data.IRepositories
{
    public interface IProductCategoryRepository : IRepository<ProductCategory,int>
    {
        List<ProductCategory> GetByAlias(string alias);
    }
}
