using System;
using System.Collections.Generic;
using System.Text;
using WebData.Entities;
using WebRepository.Infrastructure;

namespace WebRepository.Repository
{
    public interface ICategoryTranslationRepository : IRepository<CategoryTranslation>
    {

    }

    public class CategoryTranslationRepository : Repository<CategoryTranslation>, ICategoryTranslationRepository
    {
        public CategoryTranslationRepository(IFactory factory) : base(factory)
        {

        }
    }
}
