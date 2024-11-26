﻿using BookHub.Abstractions;
using BookHub.Abstractions.Logic.Converters;
using BookHub.Abstractions.Logic.Services;
using BookHub.Logic.Converters;
using BookHub.Logic.Converters.Account;
using BookHub.Logic.Services.Account;
using BookHub.Logic.Services.Books.Repository;

using Microsoft.Extensions.Options;

namespace BookHub.API.Registrations;

internal static class LogicExtensions
{
    public static IServiceCollection AddLogic(
        this IServiceCollection services,
        IConfiguration configuration)
        => services
            .AddUserHandling()
            .AddBooksActionsHandling()
            .Configure<TestConfig>(configuration.GetRequiredSection(nameof(TestConfig)))
            .AddSingleton<IValidateOptions<TestConfig>, TestConfigValidator>();

    private static IServiceCollection AddUserHandling(
        this IServiceCollection services)
        => services
            .AddHttpContextAccessor()
            .AddSingleton<IUserIdentityFacade, HttpUserIdentityFacade>()
            .AddSingleton<IUserRequestConverter, UserRequestConverter>()
            .AddScoped<IUserService, UserService>();

    private static IServiceCollection AddBooksActionsHandling(
        this IServiceCollection services)
        => services
            .AddScoped<IBookDescriptionService, BookDescriptionService>()
            .AddScoped<IBookGenresService, BookGenresService>()
            .AddSingleton<IBookParamsConverter, BookParamsConverter>();
}