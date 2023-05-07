using reZach.BudgetTransactionAnalyzer.Models;
using reZach.BudgetTransactionAnalyzer.Models.CSV;

namespace reZach.BudgetTransactionAnalyzer.Business.Preprocessors
{
    public sealed class AmexPreprocessor : BasePreprocessor, IAmexPreprocessor
    {
        public override List<TransactionRecord> ProcessTransactions(List<CSVTransactionRecord> transactions)
        {
            List<TransactionRecord> processed = new List<TransactionRecord>();

            for (int i = 0; i < transactions.Count; i++)
            {
                CSVTransactionRecord trans = transactions[i];

                // Ignore cc payments
                if (string.Equals(trans.Description, "ONLINE PAYMENT - THANK YOU", StringComparison.OrdinalIgnoreCase))
                    continue;

                // Transactions from Ghost.org should be labeled as services
                if (trans.Description.StartsWith("GHOST.ORG") && string.Equals(trans.Category, "Merchandise & Supplies-Computer Supplies", StringComparison.OrdinalIgnoreCase))
                    trans.Category = "Services";

                // Ubisoft transactions should be labeled as games and be categorized as entertainment
                else if (trans.Description.StartsWith("UBISOFT") && string.Equals(trans.Category, "Merchandise & Supplies-Internet Purchase", StringComparison.OrdinalIgnoreCase))
                    trans.Category = "Travel/ Entertainment";

                processed.Add(new TransactionRecord
                {
                    Date = trans.Date,
                    Description = trans.Description,
                    Amount = trans.Amount,
                    Category = trans.Category switch
                    {
                        "Department Stores" => BudgetCategory.DepartmentStores,
                        "Gasoline" => BudgetCategory.Gasoline,
                        "Home Improvement" => BudgetCategory.HomeImprovement,
                        "Medical Services" => BudgetCategory.MedicalServices,
                        "Merchandise" => BudgetCategory.Merchandise,
                        "Phone" => BudgetCategory.Phone,
                        "Restaurants" => BudgetCategory.Restaurants,
                        "Services" => BudgetCategory.Services,
                        "Supermarkets" => BudgetCategory.Supermarkets,
                        "Travel/ Entertainment" => BudgetCategory.TravelEntertainment,
                        _ => BudgetCategory.Unknown
                    }
                });
            }

            return processed;
        }
    }
}
