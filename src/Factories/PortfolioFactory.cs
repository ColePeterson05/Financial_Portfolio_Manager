namespace Financial_Portfolio_Manager;

public abstract class PortfolioFactory
{
    public abstract Portfolio CreatePortfolio(string name, IUser owner);
}