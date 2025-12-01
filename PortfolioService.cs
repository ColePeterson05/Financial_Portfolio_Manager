namespace Financial_Portfolio_Manager;

public class PortfolioService
{
    private readonly IPortfolioRepository _portfolioRepo;
    private readonly IUserRepository _userRepo;
    private readonly Dictionary<PortfolioType, PortfolioFactory> _factories;

    public PortfolioService(IPortfolioRepository portfolioRepo, IUserRepository userRepo, Dictionary<PortfolioType, PortfolioFactory> factories)
    {
        _portfolioRepo = portfolioRepo;
        _userRepo = userRepo;
        _factories = factories;
    }

    public Portfolio CreatePortfolio(int userId, string name, PortfolioType type)
    {
        IUser owner = _userRepo.GetUser(userId);
        if (owner == null) throw new Exception("User does not exist");
        
        if (!_factories.ContainsKey(type)) throw new Exception("Portfolio type does not exist");
        
        PortfolioFactory factory = _factories[type];
        Portfolio newPortfolio = factory.CreatePortfolio(name, owner);
        
        _portfolioRepo.Add(newPortfolio);
        return newPortfolio;
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