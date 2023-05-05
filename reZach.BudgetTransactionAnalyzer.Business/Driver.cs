using reZach.BudgetTransactionAnalyzer.Business.CSV;
using reZach.BudgetTransactionAnalyzer.Business.Postprocessors;
using reZach.BudgetTransactionAnalyzer.Business.Preprocessors;
using reZach.BudgetTransactionAnalyzer.Models;
using reZach.BudgetTransactionAnalyzer.Models.CSV;
using reZach.BudgetTransactionAnalyzer.Models.CSV.ClassMaps;
using reZach.BudgetTransactionAnalyzer.Models.Reports;

namespace reZach.BudgetTransactionAnalyzer.Business
{
    public class Driver : IDriver
    {
        private readonly ICSVProcessor _csvProcessor;
        private readonly IDiscoverPreprocessor _discoverPreprocessor;
        private readonly IPostprocessor _postprocessor;

        public Driver(ICSVProcessor csvProcessor, IDiscoverPreprocessor discoverPreprocessor, IPostprocessor postprocessor)
        {
            _csvProcessor = csvProcessor;
            _discoverPreprocessor = discoverPreprocessor;
            _postprocessor = postprocessor;
        }

        public List<TransactionRecord> ProcessTransactions(string transactionsFolderPath, string settingsFilePath)
        {
            List<TransactionRecord> masterTransactions = LoadTransactionsPrivate(transactionsFolderPath);

            // Post-process (ie. exclude transactions)
            masterTransactions = _postprocessor.ProcessTransactions(masterTransactions, settingsFilePath);

            return masterTransactions;
        }

        public List<CategorySpendByMonth> GetAverageSpend(List<TransactionRecord> transactions, int? numberOfPastMonths = null)
        {
            List<CategorySpendByMonth> spendByMonth = new List<CategorySpendByMonth>();

            // We sort here so we can retrieve the latest year in the line below
            transactions = transactions.OrderByDescending(t => t.Date.Year).ToList();

            int year = transactions[0].Date.Year;
            IEnumerable<IGrouping<int, TransactionRecord>> byMonth = transactions.GroupBy(t => t.Date.Month);
            int monthsToAverage = numberOfPastMonths.HasValue ? numberOfPastMonths.Value : byMonth.Count();
            int monthsToAverageCounter = monthsToAverage;

            for (int i = 0; i < byMonth.Count() && monthsToAverageCounter > 0; i++)
            {
                int month = byMonth.ElementAt(i).Key;
                IEnumerable<IGrouping<BudgetCategory, TransactionRecord>> categories = byMonth
                    .ElementAt(i)
                    .Where(m => m.Date.Year == year)
                    .GroupBy(g => g.Category);

                for (int j = 0; j < categories.Count(); j++)
                {
                    double sum = categories
                        .ElementAt(j)
                        .Sum(c => c.Amount);

                    spendByMonth.Add(new CategorySpendByMonth
                    {
                        Category = categories.ElementAt(j).Key,
                        Total = Math.Round(sum, 2),
                        Date = new DateTime(year, month, 1)
                    });
                }

                monthsToAverageCounter--;

                // Adjust year; if we roll over to a previous year, decrement the year
                if (i + 1 < byMonth.Count() && byMonth.ElementAt(i + 1).Key > byMonth.ElementAt(i).Key)
                    year--;
            }


            List<CategorySpendByMonth> final = new List<CategorySpendByMonth>();

            IEnumerable<IGrouping<BudgetCategory, CategorySpendByMonth>> averageByCategory = spendByMonth
                .GroupBy(s => s.Category);

            for (int i = 0; i < averageByCategory.Count(); i++)
            {
                double average = Math.Round(averageByCategory.ElementAt(i).Sum(s => s.Total) / monthsToAverage, 2);

                final.Add(new CategorySpendByMonth
                {
                    Category = averageByCategory.ElementAt(i).Key,
                    Total = average,
                    Date = averageByCategory.ElementAt(i).First().Date
                });
            }

            final = final.OrderBy(f => Enum.GetName(typeof(BudgetCategory), f.Category)).ToList();

            return final;
        }

        public List<TransactionRecord> LoadAllTransactions(string transactionsFolderPath)
        {
            return LoadTransactionsPrivate(transactionsFolderPath);
        }

        private List<TransactionRecord> LoadTransactionsPrivate(string transactionsFolderPath)
        {
            string folderPath;

            // Assign the folder based if transactionsFolderPath is a folder or file
            if (Directory.Exists(transactionsFolderPath))
                folderPath = transactionsFolderPath;
            else
            {
                folderPath = new FileInfo(transactionsFolderPath).Directory.FullName;

                if (!Directory.Exists(folderPath))
                    throw new Exception($"The path '{folderPath}' does not exist; cannot find any transactions.");
            }

            string[] transactionFiles = Directory.GetFiles(folderPath, "*.csv");

            if (transactionFiles.Length == 0)
                throw new Exception($"Could not find any [*.csv] transaction files at the following path '{folderPath}'.");

            List<TransactionRecord> masterTransactions = new List<TransactionRecord>();
            for (int i = 0; i < transactionFiles.Length; i++)
            {
                List<CSVTransactionRecord> transactions = new List<CSVTransactionRecord>();

                if (Path.GetFileNameWithoutExtension(transactionFiles[i]).Contains("Discover-Transactions", StringComparison.OrdinalIgnoreCase))
                {
                    transactions = _csvProcessor.ReadCSVFile<DiscoverTransactionRecordClassMap>(transactionFiles[i]);
                    masterTransactions.AddRange(_discoverPreprocessor.ProcessTransactions(transactions));
                }
                else
                {
                    _csvProcessor.ReadCSVFile(transactionFiles[i]);
                    masterTransactions.AddRange(_discoverPreprocessor.ProcessTransactions(transactions));
                }
            }

            return masterTransactions;
        }
    }
}
