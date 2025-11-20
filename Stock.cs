namespace Financial_Portfolio_Manager;

public class Stock : PortfolioItem
{
    private string _ticker { get; set; }
    private string _companyName { get; set; }
    private int _price { get; set; }
    private string _sector { get; set; }

    // need to add StockQuote struct later

    public Stock(string ticker, string companyName, int price, string sector)
    {
        _ticker = ticker;
        _companyName = companyName;
        _price = price;
        _sector = sector;
    }
}
