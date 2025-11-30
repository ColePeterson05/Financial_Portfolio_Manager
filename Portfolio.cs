namespace Financial_Portfolio_Manager;

public abstract class Portfolio
{
    public String Name { get; private set; }
    public int _portfolioID;
    private List<PortfolioItem> _items;
    public PortfolioType Type { get; private set; }

    public Portfolio(String name, PortfolioType type)
    {
        Name = name;
        _items = new List<PortfolioItem>();
        Type = type;
    }

    public void addItem(PortfolioItem item)
    {
        _items.Add(item);
    }

    public void removeItem(PortfolioItem item)
    {
        _items.Remove(item);
    }

    public IReadOnlyList<PortfolioItem> Items
    {
        get { return _items; }
    }
}
