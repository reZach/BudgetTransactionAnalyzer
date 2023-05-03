using CsvHelper.Configuration;
using reZach.BudgetTransactionAnalyzer.Models.CSV;
using System.Globalization;

namespace reZach.BudgetTransactionAnalyzer.Business.CSV
{
    public interface ICSVProcessor
    {
        List<CSVTransactionRecord> ReadCSVFile(string filePath);
        List<CSVTransactionRecord> ReadCSVFile<T>(string filePath) where T : ClassMap<CSVTransactionRecord>;
        List<CSVTransactionRecord> ReadCSVFile(string filePath, CultureInfo cultureInfo);
        List<CSVTransactionRecord> ReadCSVFile<T>(string filePath, CultureInfo cultureInfo) where T : ClassMap<CSVTransactionRecord>;
    }
}
