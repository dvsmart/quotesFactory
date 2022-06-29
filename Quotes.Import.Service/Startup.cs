using Microsoft.OpenApi.Models;
using Quotes.Import.Service.Attributes;
using Quotes.Import.Service.OperationFilters;
using Quotes.Import.Service.Services;
using System.Configuration;
using System.Reflection;

namespace Quotes.Import.Service
{
    public class Startup
    {
        public const string TestEnvironment = "Test";
        private readonly IWebHostEnvironment _hostEnvironment;

        /// <summary>
        /// Initializes a new instance of the <see cref="Startup"/> class.
        /// </summary>
        public Startup(IWebHostEnvironment hostEnvironment, IConfiguration configuration)
        {
            _hostEnvironment = hostEnvironment;
            Configuration = configuration;
        }
         
        public IConfiguration Configuration { get; }

        /// <summary>
        /// This method gets called by the runtime. Use this method to add services to the container.
        /// </summary>
        public void ConfigureServices(IServiceCollection services)
        {
            // Add services to the container.

            // Add basic authentication
            services.AddControllers(opt =>
            {
                opt.Filters.Add<ApiKeyAttribute>();
            });
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            services.AddEndpointsApiExplorer();

            // Add basic authentication to swagger.
            services.AddSwaggerGen(c =>
            {
                c.OperationFilter<ApiKeyHeaderParameterFilter>();
            });
            var config = new ServiceAuthenticationConfig
            {
                ApiKey = Configuration.GetValue<string>("Api.Key")
            };
            services.AddSingleton<ServiceAuthenticationConfig>(config);
            services.AddScoped<IQuoteGroupsService, QuoteGroupsService>();


        }

        /// <summary>
        /// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        /// </summary>
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        { 
            // Configure the HTTP request pipeline.
            if (env.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();

            app 
                .UseHsts()
                .UseRouting() 
                .UseStaticFiles()
                .UseEndpoints(endpoints => { endpoints.MapControllers(); });
        }
    }
}
