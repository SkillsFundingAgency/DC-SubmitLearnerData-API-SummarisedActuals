using Autofac;
using ESFA.DC.Api.Common.Ioc.Modules;
using ESFA.DC.Api.Common.Secure;
using ESFA.DC.PublicApi.FCS.Ioc;
using Microsoft.AspNetCore.Hosting;

namespace ESFA.DC.PublicApi.FCS
{
    public class Startup : SecureBaseStartup
    {
        public Startup(IWebHostEnvironment env)
            : base(env)
        {
        }

        public override void ConfigureContainer(ContainerBuilder containerBuilder)
        {
            containerBuilder.SetupConfigurations(Configuration);
            containerBuilder.RegisterModule<ServiceRegistrations>();
            containerBuilder.RegisterModule<LoggerRegistrations>();
        }
    }
}
