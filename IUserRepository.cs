namespace Financial_Portfolio_Manager;

public interface IUserRepository
{
    List<IUser> GetUsers();
    IUser GetUser(int id);
    IUser AddUser(User user);
}