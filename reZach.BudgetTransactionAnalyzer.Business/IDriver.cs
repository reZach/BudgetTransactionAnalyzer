using reZach.BudgetTransactionAnalyzer.Models;
using reZach.BudgetTransactionAnalyzer.Models.Reports;

namespace reZach.BudgetTransactionAnalyzer.Business
{
    public interface IDriver
    {
        List<TransactionRecord> ProcessTransactions(string transactionsFolderPath, string settingsFilePath);
        List<CategorySpendByMonth> GetAverageSpend(List<TransactionRecord> transactions, int? numberOfPastMonths = null);
    }
}
