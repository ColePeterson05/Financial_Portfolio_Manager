namespace Financial_Portfolio_Manager;

public class PortfolioService
{
    private readonly IPortfolioRepository portfolioRepo;
    private readonly IUserRepository userRepo;

    public PortfolioService(IPortfolioRepository portfolioRepo, IUserRepository userRepo)
    {
        this.portfolioRepo = portfolioRepo;
        this.userRepo = userRepo;
    }

    public Portfolio CreatePortfolio(IUser owner, string name, bool isGroup)
    {
        if (owner == null)
            throw new ArgumentNullException(nameof(owner));

        var portfolio = new Portfolio
        {
            Name = name,
            Owner = owner,
            IsGroup = isGroup
        };

        return portfolioRepo.Create(portfolio);
    }

    public void DeletePortfolio(int portfolioId)
    {
        portfolioRepo.Delete(portfolioId);
    }
}