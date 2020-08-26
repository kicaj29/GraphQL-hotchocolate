using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using aspnetcore.Adapters;
using aspnetcore.Core;
using aspnetcore.GraphQL;
using aspnetcore.GraphQL.Filters;
using aspnetcore.GraphQL.Mutations;
using HotChocolate;
using HotChocolate.AspNetCore;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using HotChocolate.Subscriptions;
using aspnetcore.GraphQL.Subscriptions;

namespace aspnetcore
{
    public class Startup
    {
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDataLoaderRegistry();
            services.AddSingleton<IAuthorService, InMemoryAuthorService>();
            services.AddSingleton<IBookService, InMemoryBookService>();
            services.AddErrorFilter<BookNotFoundExceptionFilter>();

            // Add in-memory event provider
            services.AddInMemorySubscriptionProvider();

            services.AddGraphQL(s => SchemaBuilder.New()
                        .AddServices(s)
                        .AddQueryType<Query>()
                        //.AddQueryType<QueryType>()
                        .AddMutationType<Mutation>()
                        .AddSubscriptionType<SubscriptionType>()
                        .Create()
                    );

            /*services.AddCors(options => {
                options.AddPolicy("default",
                builder => {
                    builder.WithOrigins("*")
                    .AllowAnyHeader()
                    .AllowAnyMethod();
                });
            });*/

            services.AddCors(options => {
                options.AddPolicy("default",
                builder => {
                    builder
                    .AllowAnyHeader()
                    .AllowAnyMethod()
                    .AllowAnyOrigin();
                });
            });

            /*services.AddCors(options => {
                options.AddPolicy("default",
                    builder => {
                        builder.WithOrigins("http://localhost:4200")
                        .AllowAnyHeader()
                        .AllowAnyMethod();
                    });
            });*/
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            // if path is specified then playground UI crashes
            // PathString graphQLPath = new PathString("/Foo/Bar");
            // app.UseGraphQL(new QueryMiddlewareOptions { Path = graphQLPath, EnableSubscriptions = false });

            app.UseCors("default");
            app.UseWebSockets();
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
