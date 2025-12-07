namespace Financial_Portfolio_Manager;

public class AuthService : IAuthService
{
    private readonly IUserRepository _userRepo;
    private IUser _currentUser;

    public AuthService(IUserRepository userRepo)
    {
        //Creating instance of repo 
        _userRepo = userRepo;
    }
    
    public bool Login(int accountId)
    {
        IUser user = _userRepo.GetUser(accountId);
        if (user == null)
        {
            Console.WriteLine("No user is was found with that Id.");
            _currentUser = null;
            return false;
        }

        _currentUser = user;
        Console.WriteLine($"{user.Name} has logged in.");
        return true;
    }

    public void Logout()
    {
        if (_currentUser == null)
        {
            Console.WriteLine("No user is currently logged in.\n");
            return;
        }

        Console.WriteLine($"{_currentUser.Name} has logged out.\n");
        //Returning back to orginal state
        _currentUser = null;
    }

    public User GetCurrentUser()
    {
        if (_currentUser == null)
            throw new InvalidOperationException("No user is logged in.");

        return null;
    }
}