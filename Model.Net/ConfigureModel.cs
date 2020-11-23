using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

using Shared.Services;

namespace Model
{
    public static class ConfigureModel
    {
        public static void ConfigureCommonServices(this IServiceCollection services, IConfiguration configuration)
        {
            // Custom content
            services.AddSingleton<ICategoriesService, CategoriesService>();
            services.AddSingleton<IProjectsService, ProjectsService>();
            services.AddSingleton<IImagesService, ImagesService>();
            services.AddSingleton<IClientsService, ClientsService>();
            services.AddSingleton<IBusinessService, BusinessService>();
        }
    }
}
