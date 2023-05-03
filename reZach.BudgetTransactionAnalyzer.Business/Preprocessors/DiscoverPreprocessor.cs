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

                // If the transaction is from Verizon, the category of the transaction should be "Phone"
                if (trans.Description.Contains("VZWRLSS*PRPAY AUTOPAY"))
                    trans.Category = "Phone";

                processed.Add(new TransactionRecord
                {
                    Date = trans.Date,
                    Description = trans.Description,
                    Amount = trans.Amount,
                    Category = trans.Category switch
                    {
                        "Department Stores" => Category.DepartmentStores,
                        "Gasoline" => Category.Gasoline,
                        "Home Improvement" => Category.HomeImprovement,
                        "Medical Services" => Category.MedicalServices,
                        "Merchandise" => Category.Merchandise,
                        "Phone" => Category.Phone,
                        "Restaurants" => Category.Restaurants,
                        "Services" => Category.Services,
                        "Supermarkets" => Category.Supermarkets,
                        "Travel/ Entertainment" => Category.TravelEntertainment,
                        _ => Category.Unknown
                    }
                });
            }

            return processed;
        }
    }
}