using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

public static class AuthenticationConfig
{
    public static void ConfigureAuthentication(this IServiceCollection services)
    {
        services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
            .AddCookie(options =>
            {
                options.LoginPath = "/Account/Login"; // Specify the login page URL
                // forward?
            });
    }

    public static void ConfigureAuthorization(this IServiceCollection services)
    {
        services.AddAuthorization();
    }

    public static void UseAuthenticationConfig(this IApplicationBuilder app)
    {
        app.UseAuthentication(); // Enable authentication middleware
        app.UseAuthorization(); // Enable authorization middleware
    }
}
