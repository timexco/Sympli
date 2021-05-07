using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using Sympli.Common;
using Sympli.Common.AWS;
using Sympli.SearchRankingAnalyser;
using Sympli.SearchRankingAnalyser.Bing;
using Sympli.SearchRankingAnalyser.Google;
using Sympli.SearchRankingAnalyser.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Sympli.API
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
            services.AddControllers()
                    .AddJsonOptions(options =>
                    {
                        options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
                        options.JsonSerializerOptions.IgnoreNullValues = true;
                    });
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Sympli.API", Version = "v1" });
            });

            RegisterServices(services);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Sympli.API v1"));
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }

        public void RegisterServices(IServiceCollection services)
        {
            services.AddHttpClient();
            services.AddScoped(typeof(IStorageClient), typeof(S3Client));

            services.AddScoped(typeof(IHtmlRankingAnalyser), typeof(GoogleHtmlRankingAnalyser));
            services.AddScoped(typeof(IHtmlRankingAnalyser), typeof(BingHtmlRankingAnalyser));

            services.AddScoped(typeof(IPageFetcherRouteResolver), typeof(GooglePageFetcherRouteResolver));
            services.AddScoped(typeof(IPageFetcherRouteResolver), typeof(BingPageFetcherRouteResolver));

            services.AddScoped(typeof(IPageFetcherSequenceResolver), typeof(GooglePageFetcherSequenceResolver));
            services.AddScoped(typeof(IPageFetcherSequenceResolver), typeof(BingPageFetcherSequenceResolver));

            services.AddScoped(typeof(PageFetcher));
            services.AddScoped(typeof(PageRankingProcessor));
        }

    }
}
