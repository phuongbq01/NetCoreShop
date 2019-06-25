using OnlineShop.Data.Entities;
using OnlineShop.Data.IRepositories;

namespace OnlineShop.Data.EF.Repositories
{
    public class SystemConfigRepository : EFRepository<SystemConfig, string>, ISystemConfigRepository
    {
        public SystemConfigRepository(AppDbContext context) : base(context)
        {
        }
    }
}
