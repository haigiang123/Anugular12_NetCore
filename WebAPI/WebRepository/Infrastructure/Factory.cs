using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using WebData.EF;

namespace WebRepository.Infrastructure
{
    public class Factory : Disposable, IFactory
    {
        //private Func<WebAPIDbContext> _instanceFunc;
        private WebAPIDbContext _dbContext;
        public WebAPIDbContext DbContext 
        { 
            get
            {
                return _dbContext;
            }
        }

        public Factory(WebAPIDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        protected override void DisposeCore()
        {
            if (DbContext != null)
            {
                DbContext.Dispose();
            }
        }
    }
}
