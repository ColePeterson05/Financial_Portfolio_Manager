namespace Financial_Portfolio_Manager;

public class GroupPortfolio : Portfolio
{
    public List<IUser> Members;

    public GroupPortfolio(String name, IUser owner)
        : base(name, PortfolioType.Group, owner)
    {
        Members = new List<IUser>();
        Members.Add(owner);
    }

    public bool IsAdmin(IUser user)
    {
        return user == Owner;
    }
}
