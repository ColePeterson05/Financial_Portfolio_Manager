using System.Text.RegularExpressions;

namespace Financial_Portfolio_Manager;

public class IndividualPortfolio : Portfolio
{
    private string _name;
    private int _portfolioId;
    private List<PortfolioItem> _items;
    private User _owner;
    private PortfolioType _type;

    public IndividualPortfolio()
    {
        _items = new List<PortfolioItem>();
    }

    public IndividualPortfolio(string name)
    {
        _name = name;
        _items = new List<PortfolioItem>();
    }

    public void AddItem(PortfolioItem item)
    {
        _items.Add(item);
    }

    public void RemoveStock(string ticker)
    {
        //Autofill from Rider needs to be revised
        _items.RemoveAll(i => i.Ticker == ticker);
    }
}