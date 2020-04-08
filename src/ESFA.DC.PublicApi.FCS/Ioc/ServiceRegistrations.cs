using System;
using System.Collections.Generic;
using Autofac;
using ESFA.DC.Api.Common.Settings;
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
            builder.RegisterType<SummarisedActualsRepository>().As<ISummarisedActualsRepository>();

            // Db contexts
            builder.RegisterType<SummarisationContext>().As<ISummarisationContext>().ExternallyOwned();

           builder.Register(context =>
                {
                    var connectionStrings = context.Resolve<ConnectionStrings>();
                    var optionsBuilder = new DbContextOptionsBuilder<SummarisationContext>();
                    optionsBuilder.UseSqlServer(
                        connectionStrings.KeyValues["SummarisedActualsDatastore"],
                        options => options.EnableRetryOnFailure(3, TimeSpan.FromSeconds(3), new List<int>()));

                    return optionsBuilder.Options;
                })
                .As<DbContextOptions<SummarisationContext>>()
                .SingleInstance();
        }
    }
}