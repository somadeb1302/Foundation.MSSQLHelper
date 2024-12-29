using Foundation.MSSQLHelper.MSSQLHelper;
using Microsoft.Extensions.DependencyInjection;

namespace Foundation.MSSQLHelper;
public static class MSSQlHeperExtensions
{
    public static IServiceCollection AddMSSQLHelperService(this IServiceCollection services)
    {
        if (services == null)
            throw new ArgumentNullException(nameof(services));

        services.AddSingleton(c =>
        {
            return (ISqlHelper) new SqlHelper();
        });

        ;
        return services;
    }
}
