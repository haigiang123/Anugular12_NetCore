using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WebData.EF;
using WebData.Entities;
using WebRepository.Infrastructure;

namespace WebRepository.Repository
{
    public interface ICategoryRepository : IRepository<Category>
    {
        
    }

    public class CategoryRepository : Repository<Category>, ICategoryRepository
    {
        public CategoryRepository(IFactory factory) : base(factory)
        {
            
        }
    }

}
