using reZach.BudgetTransactionAnalyzer.Models;
using reZach.BudgetTransactionAnalyzer.Models.Settings;
using System.Text.Json;

namespace reZach.BudgetTransactionAnalyzer.Business.Postprocessors
{
    public sealed class Postprocessor : IPostprocessor
    {
        public Postprocessor()
        {

        }

        public List<TransactionRecord> ProcessTransactions(List<TransactionRecord> transactions, string settingsFilePath = null)
        {
            UserSettingsFile userSettings = LoadUserSettings(settingsFilePath);

            if (!userSettings.ExcludedTransactions.Any())
                return transactions;

            // Exclude transactions that match any criteria that might be in our settings file            

            return transactions
                .Where(t =>
                    userSettings.ExcludedTransactions
                        .FirstOrDefault(e =>

                            // If the date [is present, and] matches, and
                            (e.Date.HasValue ? e.Date == t.Date : true) &&

                            // If the amount [is present, and] matches, and
                            (e.Amount.HasValue ? e.Amount == t.Amount : true) &&

                            // If the description [is present, and] matches, and
                            (!string.IsNullOrEmpty(e.Description) && !string.IsNullOrEmpty(t.Description) ? t.Description.Contains(e.Description, StringComparison.OrdinalIgnoreCase) : true)) == null)
                .ToList();
        }

        private UserSettingsFile LoadUserSettings(string settingsFilePath)
        {
            if (string.IsNullOrEmpty(settingsFilePath))
            {
                // Attempt to find settings file in current directory
                settingsFilePath = $"{Directory.GetCurrentDirectory()}\\settings.json";
            }

            if (Path.Exists(settingsFilePath) && Path.GetFileName(settingsFilePath).Contains(".json"))
            {
                string rawSettingsText = File.ReadAllText(settingsFilePath);
                UserSettingsFile userSettings = JsonSerializer.Deserialize<UserSettingsFile>(rawSettingsText);

                // Remove empty entries;
                // we do this as empty entries would break filtering in the ProcessTransactions method
                userSettings.ExcludedTransactions.RemoveAll(e => !e.Date.HasValue && !e.Amount.HasValue && string.IsNullOrEmpty(e.Description));

                return userSettings;
            }
            else
            {
                // Settings file does not exist; return an "empty" file
                File.WriteAllText($"{Directory.GetCurrentDirectory()}\\settings.json", JsonSerializer.Serialize(new UserSettingsFile(), new JsonSerializerOptions
                {
                    WriteIndented = true
                }));

                return new UserSettingsFile();
            }
        }
    }
}
