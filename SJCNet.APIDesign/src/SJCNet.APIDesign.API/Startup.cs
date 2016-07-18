using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using SJCNet.APIDesign.Data;
using SJCNet.APIDesign.Data.Repository;
using SJCNet.APIDesign.Model;
using Swashbuckle.Swagger.Model;

namespace SJCNet.APIDesign.API
{
    public class Startup
    {
        public Startup(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
                .AddEnvironmentVariables();

            // Setup the database
            using (var db = new DataContext())
            {
                db.Database.EnsureDeleted();
                db.Database.EnsureCreated();

                var dataSeed = new DataSeeder();
                dataSeed.SeedData(db);
            }

            Configuration = builder.Build();
        }

        public IConfigurationRoot Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            // Add framework services.
            services.AddEntityFrameworkSqlite().AddDbContext<DataContext>();
            services.AddMvc();

            // Swagger
            services.AddSwaggerGen(options =>
            {
                options.SingleApiVersion(new Info()
                {
                    Version = "v1",
                    Title = "Example API",
                    Description = "An example .Net Core Web Api project."
                });
            });

            // Dependency Injection
            services.AddSingleton(new DataContext());
            services.AddScoped<IRepository<Product>, ProductsRepository>();
            services.AddScoped<IRepository<Order>, OrdersRepository>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            loggerFactory.AddConsole(Configuration.GetSection("Logging"));
            loggerFactory.AddDebug();

            app.UseSwagger();
            app.UseSwaggerUi();

            app.UseMvc();
        }
    }
}
