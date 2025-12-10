namespace Financial_Portfolio_Manager;

public class GroupPortfolioFactory : PortfolioFactory
{
    public override Portfolio CreatePortfolio(string name, IUser owner)
    {
        return new GroupPortfolio(name, owner);
    }
}