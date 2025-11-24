namespace Financial_Portfolio_Manager;

public interface IGroupManager
{
    void AddMember(int userId, int portfolioId);
    void RemoveMember(int userId, int portfolioId);
}