using System;
using System.IO;
using System.IO.Compression;
using System.Reflection;
using System.Text;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using ESFA.DC.PublicApi.FCS.Extensions;
using ESFA.DC.PublicApi.FCS.Filters;
using ESFA.DC.PublicApi.FCS.Ioc;
using ESFA.DC.PublicApi.FCS.Settings;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.AspNetCore.ResponseCompression;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;

namespace ESFA.DC.PublicApi.FCS
{
    public class Startup
    {
        private readonly IHostingEnvironment _environment;

        public Startup(IHostingEnvironment env)
        {
            _environment = env;

            var builder = new ConfigurationBuilder();

            builder.SetBasePath(Directory.GetCurrentDirectory());

            if (env.IsDevelopment())
            {
                builder.AddJsonFile($"appsettings.{Environment.UserName}.json");
            }
            else
            {
                builder.AddJsonFile("appsettings.json");
            }

            Configuration = builder.Build();
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public IServiceProvider ConfigureServices(IServiceCollection services)
        {
            services.Configure<GzipCompressionProviderOptions>(options =>
            {
                options.Level = CompressionLevel.Fastest;
            });

            // Add Response compression services
            services.AddResponseCompression(options =>
            {
                options.Providers.Add<GzipCompressionProvider>();
                options.EnableForHttps = true;
                options.MimeTypes = new string[]
                {
                    "application/json",
                };
            });

            if (_environment.IsDevelopment())
            {
                services.AddMvc(opts => { opts.Filters.Add(new AllowAnonymousFilter()); })
                    .SetCompatibilityVersion(CompatibilityVersion.Version_3_0);
            }
            else
            {
                services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_3_0);
            }

            var authSettings = Configuration.GetConfigSection<AuthSettings>();

            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = "Jwt";
                options.DefaultChallengeScheme = "Jwt";
            }).AddJwtBearer("Jwt", options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateAudience = false,

                    // ValidAudience = "the audience you want to validate",
                    ValidateIssuer = false,

                    // ValidIssuer = "the issuer you want to validate",
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(authSettings.SecurityHash)),

                    ValidateLifetime = true, //validate the expiration and not before values in the token

                    ClockSkew = TimeSpan.FromMinutes(5) //5 minute tolerance for the expiration date
                };
            });

            services.AddSwaggerDocument(options =>
            {
                options.OperationProcessors.Add(new SwaggerOperationFilter());
            });

            services.AddApiVersioning(
                o =>
                {
                    o.ReportApiVersions = true;

                    // o.AssumeDefaultVersionWhenUnspecified = true;
                    // o.DefaultApiVersion= new ApiVersion(1,0);
                });

            services.AddMvc()
                .AddJsonOptions(options => {
                    options.SerializerSettings.NullValueHandling = NullValueHandling.Ignore;
                });

            services.AddMvc(options =>
            {
                options.Filters.Add(typeof(LoggingFilter));
                options.Filters.Add(typeof(ExceptionFilter));
            });

            
            services.AddMvc();

            return ConfigureAutofac(services);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            app.UseResponseCompression();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseAuthentication();

            app.UseSwagger();
            app.UseSwaggerUi3();

            app.UseHttpsRedirection();
            app.UseMvc();
        }

        private IServiceProvider ConfigureAutofac(IServiceCollection services)
        {
            var containerBuilder = new ContainerBuilder();

            containerBuilder.SetupConfigurations(Configuration);

            containerBuilder.RegisterModule<ServiceRegistrations>();
            containerBuilder.RegisterModule<LoggerRegistrations>();

            containerBuilder.Populate(services);
            var applicationContainer = containerBuilder.Build();

            return new AutofacServiceProvider(applicationContainer);
        }
    }
}
