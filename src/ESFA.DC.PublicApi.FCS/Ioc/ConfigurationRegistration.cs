using Autofac;
using ESFA.DC.PublicApi.FCS.Extensions;
using ESFA.DC.PublicApi.FCS.Settings;
using ESFA.DC.WebApi.External.Settings;
using Microsoft.Extensions.Configuration;

namespace ESFA.DC.PublicApi.FCS.Ioc
{
    public static class ConfigurationRegistration
    {
        public static void SetupConfigurations(this ContainerBuilder builder, IConfiguration configuration)
        {
            builder.Register(c => configuration.GetConfigSection<ConnectionStrings>())
                .As<ConnectionStrings>().SingleInstance();

            builder.Register(c => configuration.GetConfigSection<AuthSettings>())
                .As<AuthSettings>().SingleInstance();

            builder.Register(c => configuration.GetConfigSection<SwaggerSettings>())
                .As<SwaggerSettings>().SingleInstance();
        }
    }
}