using System;
using System.Collections.Generic;
using System.Text;
using WebData.Entities;
using WebRepository.Infrastructure;

namespace WebRepository.Repository
{
    public interface IProductImageRepository : IRepository<ProductImage>
    {

    }

    public class ProductImageRepository : Repository<ProductImage>, IProductImageRepository
    {
        public ProductImageRepository(IFactory factory) : base(factory)
        {

        }
    }
}
