using System;
using System.Collections.Generic;
using System.Text;
using WebData.Entities;
using WebRepository.Infrastructure;

namespace WebRepository.Repository
{
    public interface IProductInCategoryRepository : IRepository<ProductInCategory>
    {

    }

    public class ProductInCategoryRepository : Repository<ProductInCategory>, IProductInCategoryRepository
    {
        public ProductInCategoryRepository(IFactory factory) : base(factory)
        {

        }
    }
}
