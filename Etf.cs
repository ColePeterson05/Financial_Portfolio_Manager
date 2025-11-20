namespace Financial_Portfolio_Manager;

public class Etf : PortfolioItem
{
    private string _ticker { get; set; }
    private double _price { get; set; }

    public Etf(string ticker, double price)
    {
        _ticker = ticker;
        _price = price;
    }
}
