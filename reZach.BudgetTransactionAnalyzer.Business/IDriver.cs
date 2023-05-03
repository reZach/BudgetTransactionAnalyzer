namespace reZach.BudgetTransactionAnalyzer.Business
{
    public interface IDriver
    {
        void ProcessTransactions(string transactionsFolderPath, string settingsFilePath);
    }
}
