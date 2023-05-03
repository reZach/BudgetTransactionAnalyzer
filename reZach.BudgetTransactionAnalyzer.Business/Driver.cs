using reZach.BudgetTransactionAnalyzer.Business.CSV;
using reZach.BudgetTransactionAnalyzer.Business.Preprocessors;
using reZach.BudgetTransactionAnalyzer.Models;
using reZach.BudgetTransactionAnalyzer.Models.CSV;
using reZach.BudgetTransactionAnalyzer.Models.CSV.ClassMaps;

namespace reZach.BudgetTransactionAnalyzer.Business
{
    public class Driver : IDriver
    {
        private readonly ICSVProcessor _csvProcessor;
        private readonly IDiscoverPreprocessor _discoverPreprocessor;

        public Driver(ICSVProcessor csvProcessor, IDiscoverPreprocessor discoverPreprocessor)
        {
            _csvProcessor = csvProcessor;
            _discoverPreprocessor = discoverPreprocessor;
        }

        public void ProcessTransactions(string transactionsFolderPath, string settingsFilePath)
        {
            string folderPath = new FileInfo(transactionsFolderPath).Directory.FullName;

            if (!Directory.Exists(folderPath))
                throw new Exception($"The path '{folderPath}' does not exist; cannot find any transactions.");

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
        }
    }
}
