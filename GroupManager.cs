namespace Financial_Portfolio_Manager;

public class GroupManager : IUser, IGroupManager
{
    public string Name { get; set; }
    public int AccountId { get; set; } = 0; // default to 0, which means "unassigned" for repo
    private bool IsLoggedIn { get; set; }
    private List<Portfolio> portfolios = new();
    private List<Portfolio> _viewPortfolio;

    public GroupManager(string name)
    {
        Name = name;
    }
    
    public List<Portfolio> ViewPortfolio()
    {
        return portfolios;
    }
    
    List<Portfolio> IUser.ViewPortfolio => _viewPortfolio;

    public void AddMember(int userId, int portfolioId)
    {
        throw new NotImplementedException();
    }

    public void RemoveMember(int userId, int portfolioId)
    {
        throw new NotImplementedException();
    }
}