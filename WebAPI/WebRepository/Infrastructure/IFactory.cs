using System;
using System.Collections.Generic;
using System.Text;
using WebData.EF;

namespace WebRepository.Infrastructure
{
    public interface IFactory : IDisposable
    {
        //WebAPIDbContext Init();
        WebAPIDbContext DbContext { get; }
    }
}
