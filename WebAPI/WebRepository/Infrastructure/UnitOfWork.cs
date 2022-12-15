using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using WebData.EF;

namespace WebRepository.Infrastructure
{
    public class UnitOfWork : IUnitOfWork
    {
        //private WebAPIDbContext _dbContext;
        private IFactory _factory;

        public UnitOfWork(IFactory factory)
        {
            _factory = factory;
        }

        public async Task<int> SaveChange()
        {
            return await _factory.DbContext.SaveChangesAsync();
        }
    }
}
