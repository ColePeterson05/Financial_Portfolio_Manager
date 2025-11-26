namespace Financial_Portfolio_Manager;
//Last modified 11/25/25 needs work
public class GroupManager : IGroupManager, IUser
{
    private string Name;
    private int AccountId;
    private bool IsLoggedin;
    private List<Portfolio> portfolios = new();
    private List<Portfolio> _viewPortfolio;
    public GroupManager(string name, int accountId)
    {
        Name = name;
        AccountId = accountId;
    }

    public bool ManageGroupPortfolio { get; set; } //Im not sure if this is correct

    List<Portfolio> IUser.ViewPortfolio => _viewPortfolio;

    
    
}