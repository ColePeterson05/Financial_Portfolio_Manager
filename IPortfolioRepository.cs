namespace Financial_Portfolio_Manager;

public interface IPortfolioRepository
{
    List<Portfolio> GetAll();
    Portfolio GetById(int portfolioID);
    void Add(Portfolio portfolio);
    void Delete(int portfolioId);
}