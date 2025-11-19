using System.Reflection.Metadata.Ecma335;

namespace Financial_Portfolio_Management;

// IMPORTANT: Subject to change based on later implementations of stocks & ETFs. Think about:
// Load stocks and ETFs into Lists of all available stocks and ETFs from TXT files
// Use Dictionaries for lookup ex: Dictionary<string, Stock>

public class TxtDataLoader : IDataLoader
{
    // Loads stock data from a TXT file
    public List<Stock> LoadStock(string filePath)
    {
        List<Stock> stocks = new List<Stock>();

        string[] lines = File.ReadAllLines(filePath);

        foreach (string line in lines)
        {
            if (string.IsNullOrWhiteSpace(line))
                continue;

            string[] parts = line.Split(',');

            string ticker = parts[0].Trim();
            string companyName = parts[1].Trim();
            int price = int.Parse(parts[2].Trim());
            string sector = parts[3].Trim();

            Stock stock = new Stock(ticker, companyName, price, sector);
            stocks.Add(stock);
        }

        return stocks;
    }

    // Loads ETF data from a TXT file
    public List<ETF> LoadETF(string filePath)
    {
        List<ETF> etfs = new List<ETF>();

        string[] lines = File.ReadAllLines(filePath);

        foreach (string line in lines)
        {
            if (string.IsNullOrWhiteSpace(line))
                continue;

            string[] parts = line.Split(',');

            string ticker = parts[0].Trim();
            int price = int.Parse(parts[1].Trim());

            ETF etf = new ETF(ticker, price);
            etfs.Add(etf);
        }

        return etfs;
    }

    // Loads portfolio data from a TXT file
    // Method expects format to by name on one line, followed by comma-separated stock tickers on the next line
    public Dictionary<string, Dictionary<int, string>> LoadPortfolio(string filePath)
    {
        var memberPortfolios = new Dictionary<string, Dictionary<int, string>>();
        string[] lines = File.ReadAllLines(filePath);

        string currentMemberName = null;

        foreach (string line in lines)
        {
            if (string.IsNullOrWhiteSpace(line))
                continue;

            string trimmed = line.Trim();

            if (!trimmed.Contains(","))
            {
                currentMemberName = trimmed;
                memberPortfolios[currentMemberName] = new Dictionary<int, string>();
            }
            else
            {
                string[] stockStrings = trimmed
                    .Split(",")
                    .Select(s => s.Trim())
                    .Where(s => !string.IsNullOrEmpty(s))
                    .ToArray();

                for (int i = 0; i < stockStrings.Length; i++)
                {
                    memberPortfolios[currentMemberName][i + 1] = stockStrings[i];
                }
            }
        }
        return memberPortfolios;
    }
}
