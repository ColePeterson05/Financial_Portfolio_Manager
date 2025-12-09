namespace Financial_Portfolio_Manager;

public class BullMarketStrategy : IValuationStrategy
{
    public string StrategyName => "Optimistic Outlook (Bull Market +15%)";

    public double GetValuation(Quote quote)
    {
        return quote.Price * 1.15;
    }
}