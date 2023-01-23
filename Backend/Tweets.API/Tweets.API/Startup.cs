using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Tweets.API.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using System.IO;
using System.Reflection;
using Tweets.API.Provider;
using Tweets.API.Provider.IProvider;
using Tweets.API.Repository;
using Tweets.API.Repository.IRepository;

namespace Tweets.API
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
        

            services.AddControllers();
            services.Configure<TweetDatabaseSetting>(Configuration.GetSection(nameof(TweetDatabaseSetting)));

            services.AddSingleton<ITweetDatabaseSetting>(sp => sp.GetRequiredService<IOptions<TweetDatabaseSetting>>().Value);
            services.AddScoped<IUserProvider, UserProvider>();
            services.AddScoped<ITweetProvider, TweetProvider>();
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<ITweetRepository, TweetRepository>();
            services.AddAuthentication(opt => 
            { 
                opt.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                opt.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options => {
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = false,
                        ValidateAudience = false,
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("superSecretKey@2"))
                    };
            });
            services.AddCors(options => {
                options.AddPolicy("EnableCORS", builder =>
                 {
                     builder.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod();
                 });
            });
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Tweets.API", Version = "v1" });

                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Description =
                       "JWT Authorization header using the Bearer scheme. \r\n\r\n " +
                       "Enter 'Bearer' [space] and then your token in the text input below.\r\n\r\n" +
                       "Example: \"Bearer 12345abcdef\"",
                    Name = "Authorization",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "Bearer"
                });

                c.AddSecurityRequirement(new OpenApiSecurityRequirement()
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            },
                            Scheme = "oauth2",
                            Name = "Bearer",
                            In = ParameterLocation.Header,

                        },
                        new List<string>()
                    }
                });

                var xmlCommentFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlCommentFileFullPath = Path.Combine(AppContext.BaseDirectory, xmlCommentFile);
                c.IncludeXmlComments(xmlCommentFileFullPath);
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env,ILoggerFactory loggerFactory)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            loggerFactory.AddLog4Net();

            app.UseSwagger();
            
            app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Tweets.API v1"));

            //app.UseHttpsRedirection();

            app.UseCors("EnableCORS");

            app.UseRouting();
        

            app.UseAuthentication();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
