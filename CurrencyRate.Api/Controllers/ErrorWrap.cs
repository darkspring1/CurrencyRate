using CurrencyRate.Domain.Errors;

namespace CurrencyRate.Api.Controllers
{
    /// <summary>
    /// Класс-обёртка для возврата ошибок клиенту
    /// </summary>
    public class ErrorWrap
    {
        /// <summary>
        /// Человекочитаемое описание ошибки
        /// </summary>
        public string Message { get; }

        ///<summary>
        ///Код ошибки
        ///</summary> 
        public int Code { get; }

        

        /// <summary>
        /// Класс-обёртка для возврата ошибок клиенту
        /// </summary>
        /// <param name="error"></param>
        public ErrorWrap(Error error)
        {
            Message = error.Message;
            Code = error.Code;
        }
    }
}
