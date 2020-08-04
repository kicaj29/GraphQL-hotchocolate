using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using aspnetcore.Adapters;
using aspnetcore.Core;
using aspnetcore.GraphQL;
using HotChocolate;
using HotChocolate.AspNetCore;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace aspnetcore
{
    public class Startup
    {
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {

            services.AddSingleton<IAuthorService, InMemoryAuthorService>();
            services.AddSingleton<IBookService, InMemoryBookService>();

            services.AddGraphQL(s => SchemaBuilder.New()
                        .AddServices(s)
                        .AddType<AuthorType>()
                        .AddQueryType<Query>()
                        .Create()
                    );

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            // if path is specified then playground UI crashes
            // PathString graphQLPath = new PathString("/Foo/Bar");
            // app.UseGraphQL(new QueryMiddlewareOptions { Path = graphQLPath, EnableSubscriptions = false });
            app.UseGraphQL();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                //app.UsePlayground(graphQLPath);
                app.UsePlayground();
            }
          
            

            /*app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapGet("/", async context =>
                {
                    await context.Response.WriteAsync("Hello World!");
                });
            });*/
        }
    }
}
