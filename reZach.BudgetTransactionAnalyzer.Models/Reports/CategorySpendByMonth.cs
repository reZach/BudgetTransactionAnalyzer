namespace reZach.BudgetTransactionAnalyzer.Models.Reports
{
    public class CategorySpendByMonth
    {
        public BudgetCategory Category { get; set; }
        public double Total { get; set; }
        public DateTime Date { get; set; }
    }
}
