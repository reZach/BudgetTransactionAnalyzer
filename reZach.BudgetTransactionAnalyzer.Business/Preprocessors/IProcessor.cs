using reZach.BudgetTransactionAnalyzer.Models;
using reZach.BudgetTransactionAnalyzer.Models.CSV;

namespace reZach.BudgetTransactionAnalyzer.Business.Preprocessors
{
    public interface IProcessor
    {
        List<TransactionRecord> ProcessTransactions(List<CSVTransactionRecord> transactions);
    }
}
