using reZach.BudgetTransactionAnalyzer.Models;
using reZach.BudgetTransactionAnalyzer.Models.CSV;

namespace reZach.BudgetTransactionAnalyzer.Business.Preprocessors
{
    public sealed class DiscoverPreprocessor : BasePreprocessor, IDiscoverPreprocessor
    {
        public override List<TransactionRecord> ProcessTransactions(List<CSVTransactionRecord> transactions)
        {
            List<TransactionRecord> processed = new List<TransactionRecord>();

            for (int i = 0; i < transactions.Count; i++)
            {
                CSVTransactionRecord trans = transactions[i];

                // Ignore cc payments
                if (string.Equals(trans.Category, "Payments and Credits", StringComparison.OrdinalIgnoreCase))
                    continue;

                // If the transaction is from Verizon, the category of the transaction should be "Phone"
                if (trans.Description.Contains("VZWRLSS*PRPAY AUTOPAY") || trans.Description.Contains("VERIZON*TELESALE"))
                    trans.Category = "Phone";

                // If the transaction is a "Services" transaction it's a "fun-money" transaction                
                else if (string.Equals(trans.Category, "Services", StringComparison.OrdinalIgnoreCase))
                    trans.Category = "Travel/ Entertainment";

                // These are app store purchases and need to be re-categorized
                else if (trans.Description.StartsWith("GOOGLE *") && string.Equals(trans.Category, "Merchandise", StringComparison.OrdinalIgnoreCase))
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