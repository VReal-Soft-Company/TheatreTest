using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting; 
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models; 
using System; 
using System.IO; 
using System.Reflection;
using System.Threading.Tasks;
using TestTheare.Shared.Data.Settings;
using TestTheatre.API.Middleware;
using TestTheatre.BLL.DTO.Automapper;
using TestTheatre.BLL.Services;
using TestTheatre.DAL.Context; 
using Microsoft.AspNetCore.Authentication.JwtBearer; 
using Microsoft.IdentityModel.Tokens; 
using System.Text;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http;
using TestTheatre.BLL.Helpers;
using TestTheatre.API.Seeders;

namespace TestTheatre.API
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
            var connectionString = Configuration.GetConnectionString("MainDb");
            services.AddDbContext<ApplicationDataContext>(options =>
            { 
                options.UseMySql(
                          connectionString,
                          new MySqlServerVersion(new Version(5, 5, 62))
                      );
            });
            
            services.AddCors(options =>
            {
                options.AddPolicy("CORSPolicy", builder =>
                {
                    builder.AllowAnyOrigin();
                    builder.AllowAnyHeader();
                    builder.AllowAnyMethod();
                    builder.AllowCredentials(); 
                });
            });
            services.AddMvcCore()
                            .AddAuthorization() 
                            .AddApiExplorer(); 
            services.AddAutoMapper(typeof(ShowProfile));
            services.AddAutoMapper(typeof(UserProfile));
            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
            })
                      .AddJwtBearer(config =>
            {
                config.RequireHttpsMetadata = false;
                config.SaveToken = true;
                config.TokenValidationParameters = new TokenValidationParameters()
                {
                    ValidIssuer = Configuration.GetSection("JwtAuthentication")["JwtIssuer"],
                    ValidAudience = Configuration.GetSection("JwtAuthentication")["JwtAudience"],
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration.GetSection("JwtAuthentication")["JwtKey"])),
                    ClockSkew = TimeSpan.Zero,
                };
                config.Events = new JwtBearerEvents()
                {
                    OnMessageReceived = context =>
                    {
                        var accessToken = context.Request.Query["access_token"];

                        if (!string.IsNullOrEmpty(accessToken))
                        {
                            context.Token = accessToken;
                        }
                        return Task.CompletedTask;
                    }
                };
            });
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddTransient<IActionContextAccessor, ActionContextAccessor>();  
            services.Configure<JWTAuthentication>(Configuration.GetSection("JwtAuthentication"));
            services.AddScoped(sp => sp.GetService<IOptionsSnapshot<JWTAuthentication>>().Value);

            services.AddControllers();
            services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("v1", new OpenApiInfo()
                {
                    Version = "v1",
                    Title = "ShowTest API",
                    Description = "ShowTest server API.",
                    Contact = new OpenApiContact()
                    {
                        Name = "VRealSoft",
                        Email = string.Empty,
                    },
                    License = new OpenApiLicense()
                    {
                        Name = "Use under License",
                    }
                }) ;

                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                options.IncludeXmlComments(xmlPath);

                options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme()
                {
                    Name = "Authorization",
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "Bearer",
                    BearerFormat = "JWT",
                    In = ParameterLocation.Header,
                    Description = "Enter 'Bearer' [space] and then your valid token in the text input below.\r\n\r\nExample: \"Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9\"",
                });
                options.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                          new OpenApiSecurityScheme
                            {
                                Reference = new OpenApiReference
                                {
                                    Type = ReferenceType.SecurityScheme,
                                    Id = "Bearer"
                                }
                            },
                            new string[] {}

                    }
                });
            });
            AddSelfServices(services);
        }

        private void AddSelfServices(IServiceCollection services)
        {
            services.AddTransient<IJWTHelper, JWTHelper>();


            services.AddTransient<IUserService, UserService>();
            services.AddTransient<IShowService, ShowService>();
            services.AddTransient<IUserShowService, UserShowService>();
        }
        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ApplicationDataContext context, IUserService userService, IShowService showService)
        {
            context.Database.Migrate();
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "TestTheatre.API v1"));
            }
            DatabaseSeed.SeedAsync(showService, userService).GetAwaiter().GetResult();
            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseMiddleware<ExceptionMiddleware>();

            app.UseAuthentication();
            app.UseAuthorization();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
