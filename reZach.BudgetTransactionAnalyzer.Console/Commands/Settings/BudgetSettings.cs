using System.ComponentModel;
using reZach.BudgetTransactionAnalyzer.Console.Commands.Validation;
using Spectre.Console.Cli;

namespace reZach.BudgetTransactionAnalyzer.Console.Commands.Setting
{
    public class BudgetSettings : CommandSettings
    {
        //[CommandArgument(0, "<mandatory>")]
        //[Description("Mandatory argument")]
        //public string Mandatory { get; set; } = string.Empty;

        [CommandArgument(0, "[path to transactions folder]")]
        [Description("Path to transactions folder")]
        public string? PathToTransactionsFolder { get; set; }

        [CommandArgument(1, "[path to settings file]")]
        [Description("Path to settings file")]
        public string? PathToSettingsFile { get; set; }

        //[CommandOption("--command-option-flag")]
        //[Description("Command option flag.")]
        //public bool CommandOptionFlag { get; set; }

        //[CommandOption("--command-option-value <value>")]
        //[Description("Command option value.")]
        //[ValidateString]
        //public string? CommandOptionValue { get; set; }
    }
}