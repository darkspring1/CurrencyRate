using CurrencyRate.Api.Controllers;
using Microsoft.AspNetCore.Mvc;

namespace CurrencyRate.Api.Test
{
    /// <summary>
    /// Утилиты для тестирования
    /// </summary>
    public static class MvcUtils
    {

        public static bool IsResultSucceded(ActionResult result)
        {
            return result is OkObjectResult;
        }

        public static T GetSuccessResultData<T>(ActionResult result) where T : class
        {
            var okResult = (OkObjectResult)result;
            return (T)okResult.Value;
        }

        public static FaultedResultWrap GetFaultedResult(ActionResult result)
        {
            var objResult = (BadRequestObjectResult)result;
            return (FaultedResultWrap)objResult.Value;
        }

        public static bool IsResultFaulted(ActionResult result)
        {
            var objResult = (BadRequestObjectResult)result;
            return objResult.Value is FaultedResultWrap;
        }

    }
}
