namespace Financial_Portfolio_Manager;

public class GroupPortfolio : Portfolio
{
    private List<IUser> _members;
    public IUser Admin { get; private set; }

    public GroupPortfolio(String name, IUser creator)
        : base(name, PortfolioType.Group)
    {
        _members = new List<IUser>();
        Admin = creator;
        _members.Add(creator);
    }

    public bool IsAdmin(IUser user)
    {
        return user == Admin;
    }

    public IReadOnlyList<IUser> Members => _members;
}
