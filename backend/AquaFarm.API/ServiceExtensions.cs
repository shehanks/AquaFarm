using AquaFarm.Application.Mappings;
using AquaFarm.Application.Services;
using AquaFarm.Application.Services.Contracts;
using AquaFarm.Infrastructure.Data;
using AquaFarm.Infrastructure.Repositories;
using AquaFarm.Infrastructure.Repositories.Contracts;
using AquaFarm.Infrastructure.UnitOfWork;
using Microsoft.EntityFrameworkCore;

namespace AquaFarm.API
{
    public static class ServiceExtensions
    {
        public static void AddDatabaseServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<AquaFarmDbContext>(options =>
                options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));
        }

        public static IServiceCollection ConfigureServices(this IServiceCollection services)
        {
            // Register AutoMapper with profiles
            services.AddAutoMapper(cfg =>
            {
                cfg.AddProfile<MappingProfile>();
            });

            // Generic repository
            services.AddScoped(typeof(IRepositoryBase<>), typeof(RepositoryBase<>));

            // Specific repositories
            services.AddScoped<IFishFarmRepository, FishFarmRepository>();
            services.AddScoped<IWorkerRepository, WorkerRepository>();

            // UnitOfWork
            services.AddScoped<IUnitOfWork, UnitOfWork>();

            // Application services
            services.AddScoped<IFishFarmService, FishFarmService>();
            services.AddScoped<IWorkerService, WorkerService>();
            services.AddScoped<IFileService, FileService>();

            return services;
        }
    }
}
