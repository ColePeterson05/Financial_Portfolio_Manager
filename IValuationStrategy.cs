namespace Financial_Portfolio_Manager;

public interface IValuationStrategy
{
    string StrategyName { get; }
    
    double GetValuation(Quote quote);
}