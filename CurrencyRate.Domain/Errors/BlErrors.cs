namespace CurrencyRate.Domain.Errors
{
    public static class BlErrors
    {
        /// <summary>
        /// Unknown error
        /// </summary>
        public static Error Error1001 => new Error(1001, "unknown error");

        /// <summary>
        /// Required
        /// </summary>
        /// <param name="requiredName"></param>
        /// <returns></returns>
        public static Error Error1002(string requiredName) => new Error(1002, $"'{requiredName}' is required");

        /// <summary>
        /// Has invalid value
        /// </summary>
        /// <param name="name"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public static Error Error1003(string name, string value) => new Error(1003, $"'{name}' has invalid value '{value}'");
    }
}
