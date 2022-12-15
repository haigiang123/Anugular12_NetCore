using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebAPI.Services;

namespace WebAPI.Controllers
{
    public class TestMVC : Controller
    {
        private readonly IScope _scope1;
        private readonly IScope _scope2;
        private readonly ITransient _transient1;
        private readonly ISingleton _singleton1;
        private readonly ITransient _transient2;
        private readonly ISingleton _singleton2;

        public TestMVC(IScope scope1, ITransient transient1, ISingleton singleton1,
            IScope scope2, ITransient transient2, ISingleton singleton2)
        {
            _scope1 = scope1;
            _transient1 = transient1;
            _singleton1 = singleton1;
            _scope2 = scope2;
            _transient2 = transient2;
            _singleton2 = singleton2;
        }

        public IActionResult Index()
        {
            ViewBag.Scope1 = _scope1.GuildPro;
            ViewBag.Scope2 = _scope2.GuildPro;

            ViewBag.Transient1 = _transient1.GuildPro;
            ViewBag.Transient2 = _transient2.GuildPro;

            ViewBag.Singleton1 = _singleton1.GuildPro;
            ViewBag.Singleton2 = _singleton2.GuildPro;

            return View();
        }

    }
}
