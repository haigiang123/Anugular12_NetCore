using System;
using System.Collections.Generic;
using System.Text;
using WebData.EF;
using WebData.Entities;
using WebRepository.Infrastructure;

namespace WebRepository.Repository
{
    public interface IProductRepository : IRepository<Product>
    {
       
    }

    public class ProductRepository : Repository<Product>, IProductRepository
    {
        public ProductRepository(IFactory factory) : base(factory)
        {
            
        }
    }
}
