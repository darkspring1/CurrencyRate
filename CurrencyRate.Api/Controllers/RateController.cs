using System.Collections.Generic;
using System.Threading.Tasks;
using CurrencyRate.Domain.Entities;
using CurrencyRate.Domain.Services;
using Microsoft.AspNetCore.Mvc;

namespace CurrencyRate.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RateController : ControllerBase
    {
        private readonly RateService _rateService;

        public RateController(RateService rateService)
        {
            _rateService = rateService;
        }

        // GET api/values
        [HttpGet]
        public async Task<Rate[]> Get()
        {
            var result = await _rateService.GetRatesAsync();

            return result.Data;
        }

        // GET api/values/5
        [HttpGet("{id}")]
        public ActionResult<string> Get(int id)
        {
            return "value";
        }

        // POST api/values
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
