using reZach.BudgetTransactionAnalyzer.Models;
using reZach.BudgetTransactionAnalyzer.Models.CSV;

namespace reZach.BudgetTransactionAnalyzer.Business.Preprocessors
{
    public abstract class BasePreprocessor : IPreprocessor
    {
        public abstract List<TransactionRecord> ProcessTransactions(List<CSVTransactionRecord> transactions);
    }
}
