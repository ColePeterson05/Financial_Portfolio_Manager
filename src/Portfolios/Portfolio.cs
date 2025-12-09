namespace Financial_Portfolio_Manager;

public abstract class Portfolio
{
    public String Name { get; private set; }
    public int PortfolioId { get; set; } = 0; //default is set to 0
    private List<PortfolioItem> _items;
    public PortfolioType Type { get; private set; }
    public IUser Owner { get; private set; }

    public Portfolio(String name, PortfolioType type, IUser owner)
    {
        Name = name;
        _items = new List<PortfolioItem>();
        Type = type;
        Owner = owner;
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

    public double CalculateTotalValue(IValuationStrategy strategy)
    {
        double total = 0;

        foreach (PortfolioItem item in _items)
        {
            Quote snapshot = new Quote(item.Price);
            
            total += strategy.GetValuation(snapshot);
        }

        return total;
    }
}
