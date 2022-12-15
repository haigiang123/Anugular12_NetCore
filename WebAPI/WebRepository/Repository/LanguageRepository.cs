using System;
using System.Collections.Generic;
using System.Text;
using WebData.Entities;
using WebRepository.Infrastructure;

namespace WebRepository.Repository
{
    public interface ILanguageRepository : IRepository<Language>
    {

    }

    public class LanguageRepository : Repository<Language>, ILanguageRepository
    {
        public LanguageRepository(IFactory factory) : base(factory)
        {

        }
    }

}
