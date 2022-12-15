using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebAPI.Services
{
    public class SomeDI : IScope, ITransient, ISingleton
    {
        private readonly Guid _guild;

        public SomeDI()
        {
            _guild = Guid.NewGuid();
        }

        public Guid GuildPro
        {
            get
            {
                return _guild;
            }
        }

    }
}
