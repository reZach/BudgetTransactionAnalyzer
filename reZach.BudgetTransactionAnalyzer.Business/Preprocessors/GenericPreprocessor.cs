using reZach.BudgetTransactionAnalyzer.Models;
using reZach.BudgetTransactionAnalyzer.Models.CSV;

namespace reZach.BudgetTransactionAnalyzer.Business.Preprocessors
{
    public sealed class GenericPreprocessor : BasePreprocessor, IDiscoverPreprocessor
    {
        public override List<TransactionRecord> ProcessTransactions(List<CSVTransactionRecord> transactions)
        {
            return transactions.Select(t => new TransactionRecord
            {
                Date = t.Date,
                Description = t.Description,
                Amount = t.Amount,
                Category = t.Category switch
                {
                    _ => Category.Unknown
                }
            }).ToList();
        }
    }
}
