namespace Financial_Portfolio_Manager;

public class PortfolioRepository : IPortfolioRepository
{
    private static PortfolioRepository _instance;

    // lock for thread safety, good idea for singleton design pattern
    private static readonly object _lock = new object();

    private int _nextPortfolioId = 1;
    private readonly List<Portfolio> _portfolios;

    private PortfolioRepository()
    {
        _portfolios = new List<Portfolio>();
    }

    // Thread-safe Singleton like UserRepo
    public static PortfolioRepository GetInstance()
    {
        if (_instance == null)
        {
            lock (_lock)
            {
                if (_instance == null)
                {
                    _instance = new PortfolioRepository();
                }
            }
        }

        return _instance;
    }

    public List<Portfolio> GetAll()
    {
        return _portfolios;
    }

    public Portfolio GetById(int portfolioID)
    {
        return _portfolios.FirstOrDefault(p => p.PortfolioId == portfolioID);
    }

    public void Add(Portfolio portfolio)
    {
        // Check for ID conflicts
        if (portfolio.PortfolioId > 0 && GetById(portfolio.PortfolioId) != null)
        {
            Console.WriteLine($"Error: Portfolio with ID {portfolio.PortfolioId} already exists.");
        }

        // Case 1: Portfolio has no valid ID -> assign one
        if (portfolio.PortfolioId <= 0)
        {
            portfolio.PortfolioId = _nextPortfolioId;
            _nextPortfolioId++;
        }
        else
        {
            // Case 2: Portfolio already has an ID -> adjust next ID if needed
            if (portfolio.PortfolioId >= _nextPortfolioId)
            {
                _nextPortfolioId = portfolio.PortfolioId + 1;
            }
        }
        _portfolios.Add(portfolio);
    }

    public void Delete(int id)
    {
        Portfolio portfolio = _portfolios.FirstOrDefault(p => p.PortfolioId == id);
        if (portfolio != null)
        {
            _portfolios.Remove(portfolio);
        }
    }
}
