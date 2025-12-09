namespace Financial_Portfolio_Manager;

public interface IAuthService
{
    //interface doesnt need variable types 
    //Double check this line of code to make sure it meets requirements in those classes
    bool Login(int accountId); //changed from Bool to user 
    void  Logout();
    IUser GetCurrentUser();
}