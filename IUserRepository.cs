namespace Financial_Portfolio_Manager;

public interface IUserRepository
{
    List<IUser> GetUsers();
    User GetUser(int id);
    User AddUser(User user);
}