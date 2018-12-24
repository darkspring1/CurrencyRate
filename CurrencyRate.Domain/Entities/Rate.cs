using System;

namespace CurrencyRate.Domain.Entities
{
    public class Rate
    {
        private Rate()
        {

        }

        public Guid Id { get; private set; }
        public string Code { get; private set; }
        public decimal Value { get; private set; }
        public int Amount { get; private set; }
        public DateTime Date { get; private set; }

        public void SetValue(decimal value)
        {
            Value = value;
        }

        public static Rate Create(string code, decimal value, int amount, DateTime date)
        {
            return new Rate
            {
                Id = Guid.NewGuid(),
                Code = code,
                Value = value,
                Amount = amount,
                Date = date
            };

        }
    }
}
