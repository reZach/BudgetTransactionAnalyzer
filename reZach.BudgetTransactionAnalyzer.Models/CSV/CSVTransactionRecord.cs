namespace reZach.BudgetTransactionAnalyzer.Models.CSV
{
    public class CSVTransactionRecord
    {
        public DateTime Date { get; set; }
        public string Description { get; set; }
        public double Amount { get; set; }
        public string Category { get; set; }
    }
}