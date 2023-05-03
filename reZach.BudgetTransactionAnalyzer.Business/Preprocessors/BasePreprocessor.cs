using reZach.BudgetTransactionAnalyzer.Models;
using reZach.BudgetTransactionAnalyzer.Models.CSV;

namespace reZach.BudgetTransactionAnalyzer.Business.Preprocessors
{
    public abstract class BasePreprocessor : IProcessor
    {
        public abstract List<TransactionRecord> ProcessTransactions(List<CSVTransactionRecord> transactions);
    }
}
