namespace Financial_Portfolio_Manager;

public class UserRepository : IUserRepository
{
    private static UserRepository _instance;

    // lock for thread safety, good idea for singleton design pattern
    private static readonly object _lock = new object();

    private int _nextAccountId = 1;
    private List<IUser> _users;

    private UserRepository()
    {
        _users = new List<IUser>();
    }

    // singleton design pattern with lock for thread safety
    public static UserRepository GetInstance()
    {
        if (_instance == null)
        {
            lock (_lock)
            {
                if (_instance == null)
                {
                    _instance = new UserRepository();
                }
            }
        }
        return _instance;
    }

    public List<IUser> GetUsers()
    {
        return _users;
    }

    public IUser GetUser(int id)
    {
        return _users.FirstOrDefault(u => u.AccountId == id); //rider autofill function double check
    }

    public void Add(IUser user)
    {
        // check if user ID already exists
        if (user.AccountId > 0 && GetUser(user.AccountId) != null)
        {
            Console.WriteLine($"Error: User with ID {user.AccountId} already exists.");
            return;
        }

        // if user does not have an ID, assign the next available ID
        if (user.AccountId == 0)
        {
            user.AccountId = _nextAccountId;
            _nextAccountId++;
        }
        else
        {
            if (user.AccountId >= _nextAccountId)
            {
                _nextAccountId = user.AccountId + 1;
            }
        }

        _users.Add(user);
    }
}
