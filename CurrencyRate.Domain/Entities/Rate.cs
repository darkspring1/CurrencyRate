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
        public DateTime Date { get; private set; }
    }
}
