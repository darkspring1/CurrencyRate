using CurrencyRate.Api.Ioc;
using CurrencyRate.Api.Settings;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using StructureMap;
using System;

namespace CurrencyRate.Api
{
    public class Startup
    {
        private readonly ApiSettings _settings;

        IServiceProvider ConfigureIoC(IServiceCollection services)
        {
            var apiRegistry = new ApiRegistry(_settings);
            var container = new Container(apiRegistry);

            container.Name = $"{nameof(Startup)}.{nameof(ConfigureIoC)}";
            container.Configure(x => {
                x.Populate(services);
            });

            return container.GetInstance<IServiceProvider>();
        }

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;

            _settings = new ApiSettings(configuration);
        }

        public IConfiguration Configuration { get; }

        

        // This method gets called by the runtime. Use this method to add services to the container.
        public IServiceProvider ConfigureServices(IServiceCollection services)
        {
            services
                .AddMvc()
                .SetCompatibilityVersion(CompatibilityVersion.Version_2_1)
                .AddControllersAsServices();

            return ConfigureIoC(services);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                //app.UseHsts();
            }

            //app.UseHttpsRedirection();
            app.UseMvc();
        }
    }
}
