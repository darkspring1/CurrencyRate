using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace CurrencyRate.Cnb.Test.TestServices
{
    class TestCnbService : CnbService
    {
        private readonly string _response;

        public TestCnbService(string response, CnbSettings settings, ILogger<CnbService> logger) : base(settings, logger)
        {
            _response = response;
        }

        protected override Task<string> SendAsync(string urlPath)
        {
            return Task.FromResult(_response);
        }
    }
}
