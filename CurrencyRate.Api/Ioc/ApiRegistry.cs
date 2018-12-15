using CurrencyRate.Api.Settings;
using CurrencyRate.Domain.Persistent;
using CurrencyRate.Domain.Persistent.Ef;
using StructureMap;

namespace CurrencyRate.Api.Ioc
{
    public class ApiRegistry : Registry
    {
        public ApiRegistry(ApiSettings settings)
        {
            For<DataContext>()
                .Use<DataContext>()
                .Ctor<string>()
                .Is(settings.ConnectionString);

            For<IUnitOfWork>().Use<UnitOfWork>();
        }
    }
}
