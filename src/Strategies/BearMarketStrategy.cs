namespace Financial_Portfolio_Manager;

public class BearMarketStrategy : IValuationStrategy
{
    public string StrategyName => "Stress Test (Bear Market -20%)";

    public double GetValuation(Quote quote)
    {
        return quote.Price * 0.80;
    }
}