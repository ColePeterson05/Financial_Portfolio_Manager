namespace Financial_Portfolio_Manager;

public class ETF
{
    public string Ticker { get; set; }
    public double Price { get; set; }

    public ETF(string ticker, double price)
    {
        Ticker = ticker;
        Price = price;
    }
}
