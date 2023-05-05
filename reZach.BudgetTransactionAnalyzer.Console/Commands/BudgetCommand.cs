using Microsoft.Extensions.Logging;
using reZach.BudgetTransactionAnalyzer.Business;
using reZach.BudgetTransactionAnalyzer.Console.Commands.Setting;
using reZach.BudgetTransactionAnalyzer.Models;
using reZach.BudgetTransactionAnalyzer.Models.Reports;
using Spectre.Console;
using Spectre.Console.Cli;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace reZach.BudgetTransactionAnalyzer.Console.Commands
{
    public class BudgetCommand : AsyncCommand<BudgetSettings>
    {
        private readonly IDriver _driver;
        private readonly ILogger _logger;

        private int BarChartWidth = 100;

        public BudgetCommand(IDriver driver, ILogger<BudgetCommand> logger)
        {
            _driver = driver;
            _logger = logger;
        }

        public override async Task<int> ExecuteAsync(CommandContext context, BudgetSettings settings)
        {
            string answer = string.Empty;
            bool breakLoop = false;

            while (!breakLoop)
            {
                answer = AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                    .Title("[yellow]Budget Transaction Analyzer[/]")
                    .PageSize(10)
                    .MoreChoicesText("[grey](Move up and down to reveal more choices)[/]")
                    .AddChoices(new[]
                    {
                        ConsoleSelectableOptions.GetAverageSpendByCategoryInAMonth,
                        ConsoleSelectableOptions.ViewAllTransactions,
                        ConsoleSelectableOptions.AboutThisApp,
                        ConsoleSelectableOptions.Exit
                    })
                );

                switch (answer)
                {
                    case ConsoleSelectableOptions.GetAverageSpendByCategoryInAMonth:

                        string numberOfMonths = AnsiConsole.Prompt(
                            new SelectionPrompt<string>()
                                .Title("How many past months do you want to calculate the average for?")
                                .PageSize(10)
                                .MoreChoicesText("[grey](Move up and down to reveal more choices)[/]")
                                .AddChoices(new[]
                                {
                                    "all",
                                    "1",
                                    "2",
                                    "3",
                                    "4",
                                    "5",
                                    "6",
                                    "7",
                                    "8",
                                    "9",
                                    "10",
                                    "11",
                                    "12"
                                })
                            );

                        List<TransactionRecord> transactions = _driver.ProcessTransactions(settings.PathToTransactionsFolder, settings.PathToSettingsFile);

                        List<CategorySpendByMonth> spendByMonth;

                        if (int.TryParse(numberOfMonths, out int numberOfPastMonths))
                            spendByMonth = _driver.GetAverageSpend(transactions, numberOfPastMonths);
                        else
                            spendByMonth = _driver.GetAverageSpend(transactions);

                        AnsiConsole.Write(new BarChart()
                            .Width(BarChartWidth)
                            .Label($"[green bold underline]Average spend {(numberOfPastMonths != 0 ? $"(over the past {numberOfPastMonths} months) " : "")}by category by month [/]")
                            .CenterLabel()
                            .AddItems(spendByMonth, (item) => new BarChartItem(
                                item.Category.ToString(), item.Total, GetAverageSpendColor(item.Total))));

                        double total = spendByMonth.Sum(s => s.Total);

                        AnsiConsole.MarkupLine($"{Environment.NewLine}Total: {total.ToString("c")}");

                        break;
                    case ConsoleSelectableOptions.ViewAllTransactions:

                        List<TransactionRecord> allTransactions = _driver.LoadAllTransactions(settings.PathToTransactionsFolder);

                        var table = new Table();

                        table.AddColumn("Date");
                        table.AddColumn("Description");
                        table.AddColumn("Amount");
                        table.AddColumn("Category");

                        for (int i = 0; i < allTransactions.Count; i++)
                        {
                            table.AddRow(
                                allTransactions[i].Date.ToString("MM-dd-yyyy"), 
                                allTransactions[i].Description,
                                allTransactions[i].Amount.ToString("c"),
                                allTransactions[i].Category.ToString()
                            );
                        }

                        // Render the table to the console
                        AnsiConsole.Write(table);

                        break;
                    case ConsoleSelectableOptions.AboutThisApp:

                        AnsiConsole.MarkupLine($"This app was created to help budget by reviewing what you currently spend and working backwards on adjusting your budget as necessary.{Environment.NewLine}{Environment.NewLine}");

                        var rule = new Rule("[yellow]Settings[/]");
                        rule.Alignment = Justify.Center;
                        AnsiConsole.Write(rule);

                        var panel = new Panel(settings.PathToTransactionsFolder ?? "[grey][[not set]][/]");
                        panel.Header("PathToTransactionsFolder");
                        panel.Padding = new Padding(2, 1, 2, 1);
                        panel.Expand = true;
                        AnsiConsole.Write(panel);

                        var panel2 = new Panel(settings.PathToSettingsFile ?? "[grey][[not set]][/]");
                        panel2.Header("PathToSettingsFile");
                        panel2.Expand = true;
                        AnsiConsole.Write(panel2);

                        break;
                    case ConsoleSelectableOptions.Exit:
                        breakLoop = true;
                        break;
                    default:
                        break;
                }

                if (!breakLoop)
                    AnsiConsole.Ask<string>($"{Environment.NewLine}[grey]Press enter to continue[/]", "");

                AnsiConsole.Clear();
            }

            return await Task.FromResult(0);
        }

        private class ConsoleSelectableOptions
        {
            public const string GetAverageSpendByCategoryInAMonth = "Get average spend by category by month";
            public const string ViewAllTransactions = "View all transactions";
            public const string AboutThisApp = "About this app";
            public const string Exit = "Exit";
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