using System;

namespace CurrencyRate.Cnb
{
    public class CnbRate
    {

        public CnbRate(DateTime date, decimal value, string code, int amount)
        {
            Code = code;
            Amount = amount;
            Date = date;
            Value = value;
        }

        public decimal Value { get; }
        public int Amount { get; }
        public string Code { get; }
        public DateTime Date { get; }
    }
}
