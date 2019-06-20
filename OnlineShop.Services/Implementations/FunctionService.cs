
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using OnlineShop.Data.IRepositories;
using OnlineShop.Services.Interfaces;
using OnlineShop.Services.ViewModels.system;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace OnlineShop.Services.Implementations
{
    public class FunctionService : IFunctionService
    {
        private IFunctionRepository _functionRepository;
        IMapper _mapper;

        public FunctionService(IFunctionRepository functionRepository, IMapper mapper)
        {
            _functionRepository = functionRepository;
            _mapper = mapper;
        }

        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }

        public Task<List<FunctionViewModel>> GetAll()
        {
            return _functionRepository.FindAll().ProjectTo<FunctionViewModel>(_mapper.ConfigurationProvider).ToListAsync();
        }

        public List<FunctionViewModel> GetAllByPermission(Guid userId)
        {
            throw new NotImplementedException();
        }
    }
}
