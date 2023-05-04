using reZach.BudgetTransactionAnalyzer.Models;
using reZach.BudgetTransactionAnalyzer.Models.CSV;

namespace reZach.BudgetTransactionAnalyzer.Business.Preprocessors
{
    public interface IPreprocessor
    {
        List<TransactionRecord> ProcessTransactions(List<CSVTransactionRecord> transactions);
    }
}
