namespace Financial_Portfolio_Manager;

public class GroupManager : IUser, IGroupManager
{
    public string Name { get; set; }
    public int AccountId { get; set; } = 0; // default to 0, which means "unassigned" for repo
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

    public void AddMember(IUser user, GroupPortfolio portfolio)
    {
        if (portfolio == null) Console.WriteLine("Portfolio not found");
        if (user == null) Console.WriteLine("User not found");

        if (!portfolio.Members.Contains(user))
        {
            portfolio.Members.Add(user);
        }
    }

    public void RemoveMember(IUser user, GroupPortfolio portfolio)
    {
        if (portfolio == null) Console.WriteLine("Portfolio not found");
        if (user == null) Console.WriteLine("User not found");

        if (portfolio.Members.Contains(user))
        {
            portfolio.Members.Remove(user);
        }
        else
        {
            Console.WriteLine("User not found in portfolio");
        }
    }
}