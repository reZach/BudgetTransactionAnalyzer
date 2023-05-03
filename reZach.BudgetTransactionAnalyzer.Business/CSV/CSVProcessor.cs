using CsvHelper;
using CsvHelper.Configuration;
using reZach.BudgetTransactionAnalyzer.Models.CSV;
using System.Globalization;

namespace reZach.BudgetTransactionAnalyzer.Business.CSV
{
    public class CSVProcessor : ICSVProcessor
    {
        public CSVProcessor() { }

        public List<CSVTransactionRecord> ReadCSVFile(string filePath)
        {
            List<CSVTransactionRecord> records = new List<CSVTransactionRecord>();

            using (StreamReader streamReader = new StreamReader(filePath))
            using (CsvReader csvReader = new CsvReader(streamReader, CultureInfo.InvariantCulture))
            {
                records = csvReader.GetRecords<CSVTransactionRecord>().ToList();
            }

            return records;
        }

        public List<CSVTransactionRecord> ReadCSVFile<T>(string filePath) where T : ClassMap<CSVTransactionRecord>
        {
            List<CSVTransactionRecord> records = new List<CSVTransactionRecord>();

            using (StreamReader streamReader = new StreamReader(filePath))
            using (CsvReader csvReader = new CsvReader(streamReader, CultureInfo.InvariantCulture))
            {
                csvReader.Context.RegisterClassMap<T>();
                records = csvReader.GetRecords<CSVTransactionRecord>().ToList();
            }

            return records;
        }

        public List<CSVTransactionRecord> ReadCSVFile(string filePath, CultureInfo cultureInfo)
        {
            List<CSVTransactionRecord> records = new List<CSVTransactionRecord>();

            using (StreamReader streamReader = new StreamReader(filePath))
            using (CsvReader csvReader = new CsvReader(streamReader, cultureInfo))
            {
                records = csvReader.GetRecords<CSVTransactionRecord>().ToList();
            }

            return records;
        }

        public List<CSVTransactionRecord> ReadCSVFile<T>(string filePath, CultureInfo cultureInfo) where T : ClassMap<CSVTransactionRecord>
        {
            List<CSVTransactionRecord> records = new List<CSVTransactionRecord>();

            using (StreamReader streamReader = new StreamReader(filePath))
            using (CsvReader csvReader = new CsvReader(streamReader, cultureInfo))
            {
                csvReader.Context.RegisterClassMap<T>();
                records = csvReader.GetRecords<CSVTransactionRecord>().ToList();
            }

            return records;
        }
    }
}
