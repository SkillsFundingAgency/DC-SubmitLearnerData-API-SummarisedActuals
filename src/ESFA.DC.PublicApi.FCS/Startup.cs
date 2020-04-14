using Autofac;
using ESFA.DC.Api.Common;
using ESFA.DC.Api.Common.Ioc.Modules;
using ESFA.DC.PublicApi.FCS.Ioc;
using Microsoft.AspNetCore.Hosting;
using StartupBase = ESFA.DC.Api.Common.StartupBase;

namespace ESFA.DC.PublicApi.FCS
{
    public class Startup : StartupBase
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
