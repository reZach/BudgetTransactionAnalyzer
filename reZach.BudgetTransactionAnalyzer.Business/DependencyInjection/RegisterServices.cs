using Microsoft.Extensions.DependencyInjection;
using reZach.BudgetTransactionAnalyzer.Business.CSV;
using reZach.BudgetTransactionAnalyzer.Business.Postprocessors;
using reZach.BudgetTransactionAnalyzer.Business.Preprocessors;

namespace reZach.BudgetTransactionAnalyzer.Business.DependencyInjection
{
    public static class RegisterServices
    {
        /// <summary>
        /// Configures dependency injection for business services.
        /// </summary>
        /// <param name="serviceCollection"></param>
        /// <returns></returns>
        public static IServiceCollection AddBusinessService(this IServiceCollection serviceCollection)
        {
            serviceCollection
                .AddTransient<ICSVProcessor, CSVProcessor>()
                .AddTransient<IDiscoverPreprocessor, DiscoverPreprocessor>()
                .AddTransient<IPostprocessor, Postprocessor>()
                .AddTransient<IDriver, Driver>();

            return serviceCollection;
        }
    }
}
