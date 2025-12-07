namespace Financial_Portfolio_Manager;

public interface IGroupManager
{
    void AddMember(IUser user, GroupPortfolio portfolio);
    void RemoveMember(IUser user, GroupPortfolio portfolio);
}