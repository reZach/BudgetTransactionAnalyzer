namespace reZach.BudgetTransactionAnalyzer.Models
{
    public class TransactionRecord
    {
        public DateTime Date { get; set; }
        public string Description { get; set; }
        public double Amount { get; set; }
        public Category Category { get; set; }
    }
}
