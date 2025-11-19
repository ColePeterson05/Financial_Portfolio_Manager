namespace Financial_Portfolio_Management;

public interface IUserRepository
{
    List<User> GetUsers();
    User GetUser(int id);
    User AddUser(User user);
}