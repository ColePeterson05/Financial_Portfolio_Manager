namespace Financial_Portfolio_Manager;

public class Stock : PortfolioItem
{
    public string CompanyName { get; private set; }
    public string Sector { get; private set; }

    public Stock(string ticker, string companyName, double price, string sector)
        : base(ticker, price)
    {
        CompanyName = companyName;
        Sector = sector;
    }
}
