using System;
using System.Collections.Generic;
using Autofac;
using ESFA.DC.PublicApi.FCS.Interfaces;
using ESFA.DC.PublicApi.FCS.Services;
using ESFA.DC.PublicApi.FCS.Settings;
using ESFA.DC.Summarisation.Model;
using ESFA.DC.Summarisation.Model.Interface;
using Microsoft.EntityFrameworkCore;

namespace ESFA.DC.PublicApi.FCS.Ioc
{
    public class ServiceRegistrations : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            //builder.RegisterType<DateTimeProvider.DateTimeProvider>().As<IDateTimeProvider>();
            builder.RegisterType<SummarisedActualsRepository>().As<ISummarisedActualsRepository>();

            // Db contexts
            builder.RegisterType<SummarisationContext>().As<ISummarisationContext>().ExternallyOwned();

            builder.RegisterType<WebApiExternalIdentityContext>().As<IWebApiExternalIdentityContext>().InstancePerLifetimeScope();
            
            builder.Register(context =>
                {
                    var connectionStrings = context.Resolve<ConnectionStrings>();
                    var optionsBuilder = new DbContextOptionsBuilder<WebApiExternalIdentityContext>();
                    optionsBuilder.UseSqlServer(
                        connectionStrings.WebApiExternalIdentity,
                        options => options.EnableRetryOnFailure(3, TimeSpan.FromSeconds(3), new List<int>()));

                    return optionsBuilder.Options;
                })
                .As<DbContextOptions<WebApiExternalIdentityContext>>()
                .SingleInstance();

            builder.Register(context =>
                {
                    var connectionStrings = context.Resolve<ConnectionStrings>();
                    var optionsBuilder = new DbContextOptionsBuilder<SummarisationContext>();
                    optionsBuilder.UseSqlServer(
                        connectionStrings.SummarisedActualsDatastore,
                        options => options.EnableRetryOnFailure(3, TimeSpan.FromSeconds(3), new List<int>()));

                    return optionsBuilder.Options;
                })
                .As<DbContextOptions<SummarisationContext>>()
                .SingleInstance();
        }
    }
}