namespace Financial_Portfolio_Management;

public class Stock
{
    public string Ticker { get; set; }
    public string CompanyName { get; set; }
    public int Price { get; set; }
    public string Sector { get; set; }

    // need to add StockQuote struct later

    public Stock(string ticker, string companyName, int price, string sector)
    {
        Ticker = ticker;
        CompanyName = companyName;
        Price = price;
        Sector = sector;
    }
}
