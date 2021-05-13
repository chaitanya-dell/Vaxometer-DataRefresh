using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Vaxometer_DataRefresh.ApplicationSettings;
using Vaxometer_DataRefresh.IocExtensions;
using Vaxometer_DataRefresh.Manager;
using Vaxometer_DataRefresh.Middlewares;
using Vaxometer_DataRefresh.Repository;
using Vaxometer_DataRefresh.Repository.DbSettings;

namespace Vaxometer_DataRefresh
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();


            services.Configure<VexoDatabaseSettings>(Configuration.GetSection(nameof(VexoDatabaseSettings)));
            services.AddSingleton<IVexoDatabaseSettings>(x => x.GetRequiredService<IOptions<VexoDatabaseSettings>>().Value);
            services.AddControllers().AddNewtonsoftJson(x => x.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore);
            services.AddHttpContextAccessor();
            services.AddHttpClient();
            services.AddOptions();
            services.AddMemoryCache();
            services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
            services.AddMongoOperations(Configuration);
            services.Configure<ApplicationUrls>(Configuration.GetSection(nameof(ApplicationUrls)));
            services.AddSingleton<IApplicationUrls>(x => x.GetRequiredService<IOptions<ApplicationUrls>>().Value);
            services.AddScoped<IVexoManager, VexoManager>();
            services.AddScoped<ICowinRepository, CowinRepository>();
            services.AddScoped<IDataRepository, DataRepository>();
            services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo
                {
                    Title = "This is the Vaxometer Data Refresh Api",
                    Version = "v1",
                    Description = "This is a protected api for data refresh",
                });
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();
            app.UseRouting();
            app.UseGlobalExceptionHandlerMiddleware();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            app.UseSwagger();
            app.UseSwaggerUI(options => options.SwaggerEndpoint("/swagger/v1/swagger.json", "Vaxometer: Powered by Cowin Open API"));
        }
    }
}
