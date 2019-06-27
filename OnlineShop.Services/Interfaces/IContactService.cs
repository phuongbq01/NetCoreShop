using OnlineShop.Services.ViewModels.Common;
using OnlineShop.Utilities.DTO;
using System;
using System.Collections.Generic;
using System.Text;

namespace OnlineShop.Services.Interfaces
{
    public interface IContactService : IDisposable
    {
        void Add(ContactViewModel contactVm);

        void Update(ContactViewModel contactVm);

        void Delete(string id);

        List<ContactViewModel> GetAll();

        PagedResult<ContactViewModel> GetAllPaging(string keyword, int page, int pageSize);

        ContactViewModel GetById(string id);

        void SaveChanges();
    }
}
