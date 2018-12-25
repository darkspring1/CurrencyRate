using System;
using System.Linq;
using System.Threading.Tasks;
using CurrencyRate.Abstractions;
using CurrencyRate.Domain.Errors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace CurrencyRate.Api.Controllers
{

    /// <summary>
    /// Базовый контроллер
    /// </summary>
    public class BaseController : Controller
    {
        
        /// <summary>
        /// Логгер
        /// </summary>
        protected ILogger Logger { get; private set; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="logger"></param>
        public BaseController(ILogger logger)
        {
            Logger = logger;
        }

        protected Guid GetGuidFromClaim(string claimType)
        {
            return Guid.Parse(GetClaimValue(claimType));
        }

        protected string GetClaimValue(string claimType)
        {
            return User.FindFirst(claimType).Value;
        }

        protected ActionResult ActionResult<T>(T result)
        {
            return Ok(result);
        }

        /// <summary>
        /// логирует, и помещает ошибки в калсс-обёртку
        /// </summary>
        /// <param name="errors"></param>
        /// <returns></returns>
        protected FaultedResultWrap GetFaultedResultWrap(params Error[] errors)
        {
            if (errors == null || !errors.Any(err => err != null))
            {
                errors = new[] { BlErrors.Error1001 };
            }

            foreach (var error in errors)
            {
                Logger.LogError($"code: {error.Code}, message: {error.Message}");
            }

           

            return new FaultedResultWrap(errors);
        }

        /// <summary>
        /// Не успешный ответ
        /// </summary>
        /// <param name="errors"></param>
        /// <returns></returns>
        protected ActionResult FaultedActionResult(params Error[] errors)
        {
            return BadRequest(GetFaultedResultWrap(errors));
        }
        
        /// <summary>
        /// Успешный результат
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="serviceResult"></param>
        /// <returns></returns>
        protected ActionResult SuccessActionResult<T>(T data)
        {
            if (data == null)
            {
                return NoContent();
            }

            return Ok(data);
        }

        /// <summary>
        /// Успешный результат
        /// </summary>
        /// <returns></returns>
        protected ActionResult SuccessActionResult()
        {
            return NoContent();
        }

        /// <summary>
        /// Не успешный ответ
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="serviceResult"></param>
        /// <returns></returns>
        protected ActionResult FaultedActionResult<T>(IServiceResult<Error, T> serviceResult)
        {
            return FaultedActionResult(serviceResult.Errors);
        }

        /// <summary>
        /// Не успешный ответ
        /// </summary>
        /// <param name="serviceResult"></param>
        /// <returns></returns>
        protected ActionResult FaultedActionResult(IServiceResult<Error> serviceResult)
        {
            return FaultedActionResult(serviceResult.Errors);
        }

        protected async Task<ActionResult> ActionResultAsync<T>(Task<IServiceResult<Error, T>> serviceResultTask)
        {
            var serviceResult = await serviceResultTask;
            return ActionResult(serviceResult);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="serviceResult"></param>
        /// <returns></returns>
        protected ActionResult ActionResult<T>(IServiceResult<Error, T> serviceResult)
        {
            if (!serviceResult.IsSuccess)
            {
                return FaultedActionResult(serviceResult);
            }

            if (serviceResult.Data == null)
            {
                return NoContent();
            }

            return Ok(serviceResult.Data);
        }

        protected async Task<ActionResult> ActionResultAsync(Task<IServiceResult<Error>> serviceResultTask)
        {
            return ActionResult(await serviceResultTask);
        }

        protected ActionResult ActionResult(IServiceResult<Error> serviceResult)
        {
            if (!serviceResult.IsSuccess)
            {
                return FaultedActionResult(serviceResult);
            }

            return NoContent();
        }
    }
}
