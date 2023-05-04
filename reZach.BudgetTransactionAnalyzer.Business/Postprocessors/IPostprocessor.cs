using reZach.BudgetTransactionAnalyzer.Models;

namespace reZach.BudgetTransactionAnalyzer.Business.Postprocessors
{
    public interface IPostprocessor
    {
        List<TransactionRecord> ProcessTransactions(List<TransactionRecord> transactions);
    }
}
