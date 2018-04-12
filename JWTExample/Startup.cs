using JWTExample.API.Config;
using JWTExample.BusinessLogic.Config;
using JWTExample.BusinessLogic.Implementation;
using JWTExample.BusinessLogic.Interfaces;
using JWTExample.DataAccess;
using JWTExample.DataAccess.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace JWTExample.API
{
    public class Startup
    {
        public Startup(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", false, true);
            Configuration = builder.Build();
        }
        public IConfigurationRoot Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<ExampleContext>(
                options => { options.UseSqlServer(Configuration.GetConnectionString("LocalDb")); });

            IdentityConfiguration.AddIdentity(services);
            services.AddCors();
            services.AddMvc();
            IdentityConfiguration.ConfigureIdentity(services);
            services.AddSingleton(sp => AutoMapperConfig.Initialize());
            services.AddSingleton(_ => Configuration as IConfigurationRoot);
            services.AddScoped<PasswordHasher<User>>();
            services.AddScoped<IAuthService, AuthService>();
            JwtConfiguration.AddJwtAuthorization(Configuration, services);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            app.UseAuthentication();
            app.UseMvc();
            app.UseCors(builder => builder.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());
        }
    }
}
