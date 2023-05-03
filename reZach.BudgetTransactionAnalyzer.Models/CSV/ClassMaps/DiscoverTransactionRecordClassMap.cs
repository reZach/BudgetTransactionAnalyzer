using CsvHelper.Configuration;

namespace reZach.BudgetTransactionAnalyzer.Models.CSV.ClassMaps
{
    public sealed class DiscoverTransactionRecordClassMap : ClassMap<CSVTransactionRecord>
    {
        public DiscoverTransactionRecordClassMap()
        {
            Map(m => m.Date).Name("Trans. Date");
            Map(m => m.Description).Name("Description");
            Map(m => m.Amount).Name("Amount");
            Map(m => m.Category).Name("Category");
        }
    }
}
