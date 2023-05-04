﻿using reZach.BudgetTransactionAnalyzer.Models;
using reZach.BudgetTransactionAnalyzer.Models.Settings;
using System.Text.Json;

namespace reZach.BudgetTransactionAnalyzer.Business.Postprocessors
{
    public sealed class Postprocessor : IPostprocessor
    {
        private readonly UserSettingsFile _userSettings;

        public Postprocessor()
        {
            _userSettings = LoadUserSettings();
        }

        public List<TransactionRecord> ProcessTransactions(List<TransactionRecord> transactions)
        {
            if (!_userSettings.ExcludedTransactions.Any())
                return transactions;

            // Exclude transactions that match any criteria that might be in our settings file            

            return transactions
                .Where(t => 
                    _userSettings.ExcludedTransactions
                        .FirstOrDefault(e => 
                        
                            // If the date [is present, and] matches, and
                            (e.Date.HasValue ? e.Date == t.Date : true) &&

                            // If the amount [is present, and] matches, and
                            (e.Amount.HasValue ? e.Amount == t.Amount : true) &&

                            // If the description [is present, and] matches, and
                            (!string.IsNullOrEmpty(e.Description) && !string.IsNullOrEmpty(t.Description) ? t.Description.Contains(e.Description, StringComparison.OrdinalIgnoreCase) : true)) == null)
                .ToList();
        }

        private UserSettingsFile LoadUserSettings()
        {
            string settings = File.ReadAllText("C:\\Users\\zachary\\source\\repos\\BudgetTransactionAnalyzer\\reZach.BudgetTransactionAnalyzer.Console\\bin\\Debug\\net7.0\\settings.json");

            UserSettingsFile userSettings = JsonSerializer.Deserialize<UserSettingsFile>(settings);

            // Remove empty entries;
            // we do this as empty entries would break filtering in the ProcessTransactions method
            userSettings.ExcludedTransactions.RemoveAll(e => !e.Date.HasValue && !e.Amount.HasValue && string.IsNullOrEmpty(e.Description));

            return userSettings;
        }
    }
}
