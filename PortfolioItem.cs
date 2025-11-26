namespace Financial_Portfolio_Manager;

public abstract class PortfolioItem
{
    public String Ticker { get; protected set; }
    private Quote _quote;
    public virtual double Price
    {
        get { return _quote.Price; }
        private set { _quote = new Quote(value); }
    }
    public DateTime LastUpdated => _quote.LastUpdated;
    

    protected PortfolioItem(string ticker, double price)
    {
        Ticker = ticker;
        Price = price;
    }

    public void UpdatePrice(double newPrice)
    {
        Price = newPrice;
    }
}