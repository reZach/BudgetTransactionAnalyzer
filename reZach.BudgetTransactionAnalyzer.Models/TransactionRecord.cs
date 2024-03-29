﻿namespace reZach.BudgetTransactionAnalyzer.Models
{
    public class TransactionRecord
    {
        public DateTime Date { get; set; }
        public string Description { get; set; }
        public double Amount { get; set; }
        public BudgetCategory Category { get; set; }

        public bool ExcludeFromAverage { get; set; }
    }
}
