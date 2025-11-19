namespace Financial_Portfolio_Management;

public interface IPortfolioRepository
{
    List<Portfolio> GetAll();
    User GetById(int portfolioID);
    void Add(Portfolio portfolio);
}