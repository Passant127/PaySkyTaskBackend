using Clinic.API.ExtensionMethods;

using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using PaySkyTask.API.Mapping;
using PaySkyTask.Application.ExtensionMethods;
using PaySkyTask.Core.Result;
using System.Diagnostics;

namespace PasySkyTask.API.ExtensionMethods;


public static class PaySkyTaskService
{
    public static IServiceCollection AddServices(this IServiceCollection services, IConfiguration configurations)
    {
        services.AddAutoMapper(typeof(MappingProfiles));

        services.AddApplicationService();

        services.Configure<ApiBehaviorOptions>(options =>
        {
            options.InvalidModelStateResponseFactory = (apiActionContext) =>
            {
                Result validationError = new Result();
                var errors = apiActionContext.ModelState
                .Where(P => P.Value.Errors.Count > 0)
                .SelectMany(P => P.Value.Errors)
                .Select(E => E.ErrorMessage);
                foreach (var error in errors)
                {
                    validationError = Result.Invalid(new List<ValidationError>
                    {
                        new ValidationError
                        {
                            ErrorMessage = error
                        }
                    });
                }

                return new BadRequestObjectResult(validationError);
            };
        });

        return services;
    }
}
