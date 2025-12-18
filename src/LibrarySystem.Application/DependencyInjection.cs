using System.Reflection;
using FluentValidation;
using LibrarySystem.Application.Common.Behaviors;
using LibrarySystem.Application.Pricing;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace LibrarySystem.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly()));
        services.AddAutoMapper(Assembly.GetExecutingAssembly());
        services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());

        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));
        services.AddSingleton<PricingStrategyFactory>();

        return services;
    }
}

