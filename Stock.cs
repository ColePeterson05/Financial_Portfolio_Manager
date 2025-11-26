namespace Financial_Portfolio_Manager;

public class Stock : PortfolioItem
{
    private string _companyName { get; set; }
    private string _sector { get; set; }

    public Stock(string ticker, string companyName, int price, string sector) : base(ticker, price)
    {
        _companyName = companyName;
        _sector = sector;
    }
}
