namespace Financial_Portfolio_Manager;

public class AuthService : IAuthService
{
    private User? _currentUser;

    //Logging in the user 
    public bool Login(string username, string password)
    {
        //validating the credientials 
        if (username == "admin" && password == "password" )
        {
            _currentUser = new User
            {
                Name = "Admin",
                AccountId = 1
            };
            return true;
        }
        return false;
    }

    public void Logout()
    {
        _currentUser = null; //returning null for log out
    }

    public User GetCurrentUser()
    {
        if (_currentUser == null) //Providing an error if a user is not currently logged in
            throw new InvalidOperationException("No User is currently logged in.");
        return _currentUser; //Otherwise it will return current user 
    }
}