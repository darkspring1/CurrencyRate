using System.Threading.Tasks;
using CurrencyRate.Api.ApplicationServices;
using CurrencyRate.Api.Errors;
using CurrencyRate.Domain.Errors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace CurrencyRate.Api.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    public class RatesController : BaseController
    {
        const string REPORT_TYPE_TXT = "txt";
        const string REPORT_TYPE_JSON = "json";
        private readonly ApplicationRateService _rateService;

        public RatesController(ApplicationRateService rateService, ILogger<RatesController> logger): base(logger)
        {
            _rateService = rateService;
        }

        // GET api/values
        [HttpGet]
        public Task<ActionResult> Get(int year, int month, string type)
        {

            if (string.IsNullOrEmpty(type))
            {
                return Task.FromResult(FaultedActionResult(BlErrors.Error1002(nameof(type))));
            }

            var lowerType = type.ToLower();

            if (lowerType == REPORT_TYPE_TXT)
            {
                //Response.Headers.Add("Content-Type: text/plain")
                return ActionResultAsync(_rateService.GetTxtReportAsync(year, month));
            }

            else if (lowerType == REPORT_TYPE_JSON)
            {
                return ActionResultAsync(_rateService.GetReportAsync(year, month));
            }

            return Task.FromResult(FaultedActionResult(ApiErrors.Error2001(type)));
        }

      
    }
}
