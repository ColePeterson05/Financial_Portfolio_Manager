namespace Financial_Portfolio_Management;

public class UserRepository
{
    private List<IUser> users = new();

    public List<IUser> GetAll()
    {
        return users;
    }
    public User GetByName(string name)
    {
        return users.OfType<User>().FirstOrDefault(u => u.Name == name); //rider autofill function double check
    }
    public void Add(User user)
    {
        users.Add(user);
    }
}