namespace Financial_Portfolio_Manager;

public class GroupService
{
    private readonly UserRepository _userRepo;
    private readonly PortfolioRepository _portfolioRepo;
    private readonly AuthService _authService;

    public GroupService(UserRepository userRepo, PortfolioRepository portfolioRepo, AuthService authService)
    {
        _userRepo = userRepo;
        _portfolioRepo = portfolioRepo;
        _authService = authService;
    }

    public void AddUserToGroup(int userId, int portfolioId)
    {
        if (_authService.GetCurrentUser() is not GroupManager activeManager)
        {
            Console.WriteLine("You are not authorized to add user");
            return;
        }
        IUser userToAdd = _userRepo.GetUser(userId);
        var portfolio = _portfolioRepo.GetById(portfolioId) as GroupPortfolio;
        
        activeManager.AddMember(userToAdd, portfolio);
    }

    public void RemoveUserFromGroup(int userId, int portfolioId)
    {
        if (_authService.GetCurrentUser() is not GroupManager activeManager)
        {
            Console.WriteLine("You are not authorized to add user");
            return;
        }
        IUser userToRemove = _userRepo.GetUser(userId);
        var portfolio = _portfolioRepo.GetById(portfolioId) as GroupPortfolio;
        
        activeManager.RemoveMember(userToRemove, portfolio);
    }
}