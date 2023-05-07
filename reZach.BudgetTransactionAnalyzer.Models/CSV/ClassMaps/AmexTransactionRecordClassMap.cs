using CsvHelper.Configuration;

namespace reZach.BudgetTransactionAnalyzer.Models.CSV.ClassMaps
{
    public sealed class AmexTransactionRecordClassMap : ClassMap<CSVTransactionRecord>
    {
        public AmexTransactionRecordClassMap()
        {
            Map(m => m.Date).Name("Date");
            Map(m => m.Description).Name("Description");
            Map(m => m.Amount).Name("Amount");
            Map(m => m.Category).Name("Category");
        }
    }
}
