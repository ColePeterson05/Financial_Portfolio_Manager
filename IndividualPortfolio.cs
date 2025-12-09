namespace Financial_Portfolio_Manager;

public class IndividualPortfolio : Portfolio
{
    public IndividualPortfolio(String name, IUser owner)
        : base(name, PortfolioType.Individual, owner) { }
}
