namespace Financial_Portfolio_Manager;

public interface IPortfolioRepository
{
    List<Portfolio> GetAll();
    List<Portfolio> GetByUser(IUser user);
    Portfolio GetById(int portfolioID);
    void Add(Portfolio portfolio);
    void Delete(int portfolioId);
}
