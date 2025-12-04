namespace Financial_Portfolio_Manager;

public class AuthService : IAuthService
{
    private User? _loggedIn; // keep user isolated – SRP

    public User Login(string accountId, string name)
    {
        // Convert accountId into an int (or you could keep it as string)
        int id = int.Parse(accountId);

        _loggedIn = new User(name)
        {
            AccountId = id
        };

        return _loggedIn;
    }

    public void Logout() => _loggedIn = null; //lambda to make sure login isnt logging in 

    public User GetCurrentUser()
    {
        if (_loggedIn == null)
            throw new InvalidOperationException("No user is logged in.");

        return _loggedIn;
    }
}