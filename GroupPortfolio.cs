namespace Financial_Portfolio_Manager;

public class GroupPortfolio : Portfolio
{
    public List<IUser> Members;
    public IUser Admin { get; private set; }

    public GroupPortfolio(String name, IUser creator)
        : base(name, PortfolioType.Group)
    {
        Members = new List<IUser>();
        Admin = creator;
        Members.Add(creator);
    }

    public bool IsAdmin(IUser user)
    {
        return user == Admin;
    }
}
