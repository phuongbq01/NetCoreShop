using System;
using System.Collections.Generic;
using System.Text;

namespace OnlineShop.Infrastructure.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        void Commit();
    }
}
