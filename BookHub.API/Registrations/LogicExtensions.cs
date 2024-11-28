using BookHub.Abstractions;
using BookHub.Abstractions.Logic.Converters.Books.Repository;
using BookHub.Abstractions.Logic.Services.Account;
using BookHub.Abstractions.Logic.Services.Books.Content;
using BookHub.Abstractions.Logic.Services.Books.Repository;
using BookHub.Abstractions.Logic.Services.Favorite;
using BookHub.Logic.Converters.Account;
using BookHub.Logic.Converters.Books.Repository;
using BookHub.Logic.Services.Account;
using BookHub.Logic.Services.Books.Content;
using BookHub.Logic.Services.Books.Repository;
using BookHub.Logic.Services.Favorite;

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
            .AddScoped<IUserService, UserService>()
            .AddScoped<IUserFavoriteService, UserFavoriteService>()
            .AddScoped<IRolesService, RolesService>();

    private static IServiceCollection AddBooksActionsHandling(
        this IServiceCollection services)
        => services
            .AddScoped<IBookDescriptionService, BookDescriptionService>()
            .AddScoped<IBookGenresService, BookGenresService>()
            .AddScoped<IChaptersService, ChaptersService>()
            .AddSingleton<IBookParamsConverter, BookParamsConverter>();
}