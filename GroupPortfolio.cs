namespace Financial_Portfolio_Manager;

public class GroupPortfolio : Portfolio
{
    private string _name;
    private int _portfolioId;
    private List<User> _users;
    private List<PortfolioItem> _items;
    private User _owner;
    private PortfolioType _type;

    public GroupPortfolio()
    {
        _users = new List<User>();
        _items = new List<PortfolioItem>();
    }

    public GroupPortfolio(string name)
    {
        _name = name;
        _users = new List<User>();
        _items = new List<PortfolioItem>();
    }

    public void AddItem(PortfolioItem item)
    {
        _items.Add(item);
    }
    
    public void RemoveStock(string ticker)
    {
        //auto filled lambda func from rider needs to be revised
        _items.RemoveAll(i => i.Ticker == ticker);
    }
}