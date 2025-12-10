namespace Financial_Portfolio_Manager;

public class IndividualPortfolioFactory : PortfolioFactory
{
    public override Portfolio CreatePortfolio(string name, IUser owner)
    {
        return new IndividualPortfolio(name, owner);
    }
}