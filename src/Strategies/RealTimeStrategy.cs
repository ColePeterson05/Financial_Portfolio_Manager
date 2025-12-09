namespace Financial_Portfolio_Manager;

public class RealTimeStrategy : IValuationStrategy
{
    public string StrategyName => "Real-Time Market Value";

    public double GetValuation(Quote quote)
    {
        return quote.Price;
    }
}