using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Vaxometer_DataRefresh.Manager;
using Vaxometer_DataRefresh.Repository;
using Vaxometer_DataRefresh.Repository.DbSettings;

namespace Vaxometer_DataRefresh.IocExtensions
{
    public static class IServiceCollectionExtension
    {
        public static IServiceCollection AddMongoOperations(this IServiceCollection services, IConfiguration configuration)
        {

            services.Configure<VexoDatabaseSettings>(configuration.GetSection(nameof(VexoDatabaseSettings)));
            services.AddSingleton<IVexoDatabaseSettings>(x => x.GetRequiredService<IOptions<VexoDatabaseSettings>>().Value);
            services.AddScoped<IVexoDataService, VexoDataService>();
            services.AddScoped(typeof(IMongoRepository<>), typeof(MongoRepository<>));
            

            return services;
        }
    }
}
