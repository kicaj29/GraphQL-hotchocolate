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
using Microsoft.AspNetCore.Authentication;
using aspnetcore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using HotChocolate.AspNetCore.Interceptors;
using System.Security.Claims;

namespace aspnetcore
{
    public class Startup
    {
        private void AddPolicies(IServiceCollection services)
        {
            services.AddAuthorization(a =>
            {
                a.AddPolicy("super-boss-policy", builder =>
                    builder
                        .RequireAuthenticatedUser()
                        .RequireRole("Managers")
                        .RequireRole("Senior Managers")
                        );

            });
        }

        private void ConfigureBasicAuthentication(IServiceCollection services)
        {
            services.AddAuthentication("BasicAuthentication")
                .AddScheme<AuthenticationSchemeOptions, BasicAuthenticationHandler>("BasicAuthentication", null);
            this.AddPolicies(services);
        }

        private void ConfigureJwtAuthentication(IServiceCollection services)
        {
            services.AddScoped<IIdentityService, IdentityService>();
            services.AddHttpContextAccessor();
            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateAudience = true,
                    ValidateIssuer = true,
                    ValidateIssuerSigningKey = true,
                    ValidAudience = "audience",
                    ValidIssuer = "issuer",
                    RequireSignedTokens = false,
                    IssuerSigningKey =
                        new SymmetricSecurityKey(Encoding.UTF8.GetBytes("secretsecretsecret"))
                };

                options.RequireHttpsMetadata = false;
                options.SaveToken = true;
            });
            this.AddPolicies(services);

        }
        /// <summary>
        /// This function is used to create CurrentUser instance that later can be used as paramter in 
        /// GrapQL endpoints.
        /// </summary>
        /// <returns></returns>
        private static OnCreateRequestAsync AuthenticationInterceptor()
        {
            return (context, builder, cancelationToken) =>
            {
                if (context.GetUser().Identity.IsAuthenticated)
                {
                    builder.SetProperty("currentUser",
                        new CurrentUser(context.User.FindFirstValue(ClaimTypes.NameIdentifier),
                            context.User.Claims.Select(x => new Tuple<string, string>(x.Type, x.Value)).ToList()));
                    
                }

                return Task.CompletedTask;
            };
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            // this.ConfigureBasicAuthentication(services);
            this.ConfigureJwtAuthentication(services);

            services.AddQueryRequestInterceptor(AuthenticationInterceptor());

            services.AddDataLoaderRegistry();
            services.AddSingleton<IAuthorService, InMemoryAuthorService>();
            services.AddSingleton<IBookService, InMemoryBookService>();
            services.AddErrorFilter<BookNotFoundExceptionFilter>();

            // Add in-memory event provider (needed for Subscriptions)
            services.AddInMemorySubscriptionProvider();

            services.AddGraphQL(s => SchemaBuilder.New()
                        .AddServices(s)
                        .AddAuthorizeDirectiveType()
                        .AddQueryType<Query>()
                        // .AddQueryType<QueryType>()
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

            /*services.AddQueryRequestInterceptor(async (context, builder, ct) =>
            {
                if (context.User.Identity.IsAuthenticated)
                {
                    
                }
                
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

            // !!!--------UseAuthentication must be called before UseGraphQL-----------!!!
            app.UseAuthentication();
            app.UseGraphQL();
           


            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
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
