using OnlineShop.Data.Entities;
using OnlineShop.Infrastructure.Interfaces;

namespace OnlineShop.Data.IRepositories
{
    public interface IProductQuantityRepository : IRepository<ProductQuantity, int>
    {
    }
}
