namespace Financial_Portfolio_Manager;

public class PortfolioService
{
    private readonly IPortfolioRepository _portfolioRepo;
    private readonly IUserRepository _userRepo;

    public PortfolioService(IPortfolioRepository portfolioRepo, IUserRepository userRepo)
    {
        _portfolioRepo = portfolioRepo;
        _userRepo = userRepo;
    }

    public Portfolio CreatePortfolio(int userId, string name, PortfolioType type)
    {
        IUser owner = _userRepo.GetUser(userId);
        if (owner == null) throw new Exception("User does not exist");

        // need factory implementation
        Portfolio portfolio;
        
        if (type == PortfolioType.Individual)
        {
            portfolio = new IndividualPortfolio(name, owner);
        }
        else
        {
            portfolio = new GroupPortfolio(name, owner);
        }
        
        _portfolioRepo.Add(portfolio);
        return portfolio;
    }

    public void DeletePortfolio(int portfolioId, IUser user)
    {
        Portfolio portfolio = _portfolioRepo.GetById(portfolioId);
        if (portfolio == null) throw new Exception("Portfolio does not exist");

        if (portfolio is GroupPortfolio gp && gp.Admin != user)
        {
            Console.WriteLine("You do not have permission to delete this portfolio");
            return;
        }

        if (portfolio is IndividualPortfolio ip && ip.Owner != user)
        {
            Console.WriteLine("You do not have permission to delete this portfolio");
            return;
        }
        
        _portfolioRepo.Delete(portfolioId);
    }
}