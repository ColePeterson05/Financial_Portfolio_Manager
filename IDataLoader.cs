namespace Financial_Portfolio_Management;

public interface IDataLoader
{
    public Dictionary<string, Dictionary<int, string>> LoadPortfolio(string filePath);

    public List<Stock> LoadStock(string filePath);

    public List<ETF> LoadETF(string filePath);
}