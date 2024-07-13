
using Clinic.API.ExtensionMethods;
using Clinic.Application.Contracts;
using Hangfire;
using Hangfire.States;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using PasySkyTask.API.ExtensionMethods;
using PasySkyTask.Domain.IRepositories;
using PasySkyTask.Infrastructure.Repositories;
using PaySkyTask.API.ExceptionHandlers;
using PaySkyTask.API.Mapping;
using PaySkyTask.API.Middlware;
using PaySkyTask.Application.Services;
using PaySkyTask.Domain.Entities;
using PaySkyTask.Domain.IRepositories;
using PaySkyTask.Infrastructure.BaseContext;
using Serilog;

namespace PaySkyTask
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Host.UseSerilog((context, configuration) =>
          configuration.ReadFrom.Configuration(context.Configuration));

            builder.Services.AddExceptionHandler<ResourceNotFoundExceptionHandler>();
            builder.Services.AddExceptionHandler<AuthorizationExceptionHandler>();
            builder.Services.AddExceptionHandler<GlobalExceptionHandler>();
            builder.Services.AddScoped(typeof(UserContextService));
            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen(options =>
            {
                options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Name = "Authorization",
                    Type = SecuritySchemeType.ApiKey,
                    BearerFormat = "JWT",
                    In = ParameterLocation.Header,
                    Description = "Enter your token"
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
                            },
                            Name = "Bearer",
                            In = ParameterLocation.Header
                        },
                        new List<string>()
                    }
                });
            });


            builder.Services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
            builder.Services.AddScoped(typeof(IVacancyRepository), typeof(VacancyRepository));
            builder.Services.AddScoped(typeof(IApplicationRepository), typeof(ApplicationRepository));
            builder.Services.AddScoped(typeof(IVacancyRepository), typeof(VacancyRepository));
            builder.Services.AddServices(builder.Configuration);
            builder.Services.AddMemoryCache();


            builder.Services.AddIdentity<User, IdentityRole>()
                           .AddEntityFrameworkStores<ApplicationDbContext>()
                           .AddDefaultTokenProviders();

            builder.Services.AddDbContext<ApplicationDbContext>(options =>
                   options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnectionForSql"),
                   b => b.MigrationsAssembly("PasySkyTask.Infrastructure")));

            builder.Services.AddHangfire(config =>
           config.SetDataCompatibilityLevel(CompatibilityLevel.Version_170)
                 .UseSimpleAssemblyNameTypeSerializer()
                 .UseRecommendedSerializerSettings()
                 .UseSqlServerStorage(builder.Configuration.GetConnectionString("DefaultConnectionForSql")));



            builder.Services.AddJwtAuthentication(builder);


            
            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            

            //app.UseHttpsRedirection();

            app.UseMiddleware<UserScopeMiddleware>();
            app.UseHangfireServer();

            app.UseExceptionHandler(opt => { });
            app.UseSerilogRequestLogging();
            app.UseAuthentication();

            app.UseAuthorization();

            app.MapGet("/", () => app.Configuration.AsEnumerable());
            app.UseHangfireDashboard("/hangfire", new DashboardOptions
            {
               
                AppPath = "/",
                DashboardTitle = "Hangfire"
            });

            app.MapControllers();

            app.Run();
        }
    }
}
