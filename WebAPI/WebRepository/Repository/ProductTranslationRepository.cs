using System;
using System.Collections.Generic;
using System.Text;
using WebData.Entities;
using WebRepository.Infrastructure;

namespace WebRepository.Repository
{
    public interface IProductTranslationRepository : IRepository<ProductTranslation>
    {

    }

    public class ProductTranslationRepository : Repository<ProductTranslation>, IProductTranslationRepository
    {
        public ProductTranslationRepository(IFactory factory) : base(factory)
        {

        }
    }
}
