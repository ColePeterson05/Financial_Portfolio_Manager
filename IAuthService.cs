namespace Financial_Portfolio_Manager;

public interface IAuthService
{
    //interface doesnt need variable types 
    //Double check this line of code to make sure it meets requirements in those classes
    bool Login(string username, string password); 
    void  Logout();
    User GetCurrentUser();
}