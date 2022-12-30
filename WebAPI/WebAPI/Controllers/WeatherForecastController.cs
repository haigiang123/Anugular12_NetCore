using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApplication.OtherService;
using WebViewModel.Common;
using WebViewModel.OtherService;

namespace WebAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {

        private readonly IProductPaternService _productPaternService;
        private readonly ILogger<WeatherForecastController> _logger;

        public WeatherForecastController(IProductPaternService productPaternService, ILogger<WeatherForecastController> logger)
        {
            _productPaternService = productPaternService;
            _logger = logger;
        }

        private static readonly string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        /// <summary>
        /// Gets the list of all Products.
        /// </summary>
        /// <returns>The list of Employees.</returns>
        /// <response code="200">Returns the items</response>
        [HttpGet]
        public PagedResult<ProductVm> Get()
        {
            GetManageProductPagingRequest req = new GetManageProductPagingRequest
            {

            };

            var a = _productPaternService.GetAllPaging(req);

            return a;
            //var rng = new Random();
            //return Enumerable.Range(1, 5).Select(index => new WeatherForecast
            //{
            //    Date = DateTime.Now.AddDays(index),
            //    TemperatureC = rng.Next(-20, 55),
            //    Summary = Summaries[rng.Next(Summaries.Length)]
            //})
            //.ToArray();
        }
    }
}
