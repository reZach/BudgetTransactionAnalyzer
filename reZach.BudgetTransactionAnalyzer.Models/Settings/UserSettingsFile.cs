namespace reZach.BudgetTransactionAnalyzer.Models.Settings
{
    public class UserSettingsFile
    {
        public List<ExcludedTransaction> ExcludedTransactions { get; set; }
    }

    public class ExcludedTransaction
    {
        public DateTime? Date { get; set; }
        public double? Amount { get; set; }
        public string Description { get; set; }
    }
}
