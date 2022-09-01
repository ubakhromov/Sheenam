//===================================================
// Copyright (c) Coalition of Good-Hearted Engineers 
// Free To Use Comfort and Peace
// ==================================================

using Microsoft.OpenApi.Models;
using Sheenam.Api.Brokers.Storages;

namespace Sheenam.Api
{
    public class Startup
    {
        public Startup(IConfiguration configuration) => Configuration = configuration;

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            var apiInfo = new OpenApiInfo();

            services.AddDbContext<StorageBroker>();
            services.AddControllers();
            services.AddTransient<IStorageBroker, StorageBroker>();

            services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc(
                name: "v1",
                info: apiInfo);
            });

        }
        
        public void Configure(IApplicationBuilder app, IWebHostEnvironment environment)
        {
            if (environment.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();

                app.UseSwaggerUI(options =>
                {
                    options.SwaggerEndpoint(
                    url: "/swagger/v1/swagger.json",
                    name: "Sheenam.Api v1");
                    
                });                   
                    
            }

            app.UseHttpsRedirection();
            app.UseRouting();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            endpoints.MapControllers());

        }
            
    }
}
