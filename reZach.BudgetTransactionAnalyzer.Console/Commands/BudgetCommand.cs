using Microsoft.Extensions.Logging;
using reZach.BudgetTransactionAnalyzer.Business;
using reZach.BudgetTransactionAnalyzer.Console.Commands.Setting;
using Spectre.Console.Cli;
using System.Threading.Tasks;

namespace reZach.BudgetTransactionAnalyzer.Console.Commands
{
    public class BudgetCommand : AsyncCommand<BudgetSettings>
    {
        private readonly IDriver _driver;
        private readonly ILogger _logger;

        public BudgetCommand(IDriver driver, ILogger<BudgetCommand> logger)
        {
            _driver = driver;
            _logger = logger;
        }

        public override async Task<int> ExecuteAsync(CommandContext context, BudgetSettings settings)
        {
            _driver.ProcessTransactions(settings.PathToTransactionsFolder, settings.PathToSettingsFile);

            //_logger.LogInformation("Mandatory: {Mandatory}", settings.PathToTransactionsFolder);
            //Logger.LogInformation("Optional: {Optional}", settings.Optional);
            //Logger.LogInformation("CommandOptionFlag: {CommandOptionFlag}", settings.CommandOptionFlag);
            //Logger.LogInformation("CommandOptionValue: {CommandOptionValue}", settings.CommandOptionValue);



            return await Task.FromResult(0);
        }
    }
}