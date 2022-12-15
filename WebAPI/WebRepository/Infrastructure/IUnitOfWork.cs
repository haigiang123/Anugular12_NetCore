using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace WebRepository.Infrastructure
{
    public interface IUnitOfWork
    {
        Task<int> SaveChange();
    }
}
