using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using reZach.BudgetTransactionAnalyzer.Business;
using reZach.BudgetTransactionAnalyzer.Business.CSV;
using reZach.BudgetTransactionAnalyzer.Business.DependencyInjection;
using reZach.BudgetTransactionAnalyzer.Console.Commands;
using Spectre.Console.Cli;
using Spectre.Console.Cli.Extensions.DependencyInjection;

var serviceCollection = new ServiceCollection()
    .AddTransient<ICSVProcessor, CSVProcessor>()
    .AddTransient<IDriver, Driver>()
    .AddLogging(configure => configure
        .AddSimpleConsole(opts =>
        {
            opts.TimestampFormat = "yyyy-MM-dd HH:mm:ss ";
        })
    );

serviceCollection.AddBusinessService();

using var registrar = new DependencyInjectionRegistrar(serviceCollection);
var app = new CommandApp(registrar);

app.Configure(
    config =>
    {
        config.ValidateExamples();

        config.AddCommand<BudgetCommand>("budget")
                .WithDescription("Example budget command.")
                .WithExample(new[] { "budget" });
    });

return await app.RunAsync(args);