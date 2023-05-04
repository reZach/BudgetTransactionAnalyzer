using Microsoft.Extensions.Logging;
using reZach.BudgetTransactionAnalyzer.Business;
using reZach.BudgetTransactionAnalyzer.Console.Commands.Setting;
using reZach.BudgetTransactionAnalyzer.Models;
using reZach.BudgetTransactionAnalyzer.Models.Reports;
using Spectre.Console;
using Spectre.Console.Cli;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
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
            List<TransactionRecord> transactions = _driver.ProcessTransactions(settings.PathToTransactionsFolder, settings.PathToSettingsFile);

            List<CategorySpendByMonth> spendByMonth = _driver.GetAverageSpend(transactions);

            AnsiConsole.Write(new BarChart()
                .Width(80)
                .Label("[green bold underline]Average spend in categories per month[/]")
                .CenterLabel()
                .AddItems(spendByMonth, (item) => new BarChartItem(
                    item.Category.ToString(), item.Total, GetAverageSpendColor(item.Total))));

            double total = spendByMonth.Sum(s => s.Total);

            AnsiConsole.MarkupLine($"Total: {total.ToString("c")}");

            return await Task.FromResult(0);
        }

        private Color GetAverageSpendColor(double total)
        {
            return total switch
            {                
                (<= 50) => Color.Green,
                (> 50 and <= 100) => Color.Yellow,
                (> 100) => Color.Red,
                _ => Color.Grey
            };
        }
    }
}