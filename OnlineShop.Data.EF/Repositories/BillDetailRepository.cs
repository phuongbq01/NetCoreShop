using OnlineShop.Data.Entities;
using OnlineShop.Data.IRepositories;

namespace OnlineShop.Data.EF.Repositories
{
    public class BillDetailRepository : EFRepository<BillDetail, int>, IBillDetailRepository
    {
        public BillDetailRepository(AppDbContext context) : base(context)
        {
        }
    }
}
