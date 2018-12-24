using System.Threading.Tasks;
using CurrencyRate.Api.ApplicationServices;
using Microsoft.AspNetCore.Mvc;

namespace CurrencyRate.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RatesController : ControllerBase
    {
        private readonly ApplicationRateService _rateService;

        public RatesController(ApplicationRateService rateService)
        {
            _rateService = rateService;
        }

        // GET api/values
        [HttpGet]
        public async Task<object> Get(int year, int month)
        {
            var result = await _rateService.GetRatesAsync(year, month, "txt");

            return result.Data;
        }

      
    }
}
