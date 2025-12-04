using ReactiveUI;
using System.Collections.ObjectModel;
namespace Financial_Portfolio_Manager.ViewModels;

public class DashboardViewModel : ReactiveObject
{
    private readonly PortfolioService _portfolioService;
    private readonly IAuthService _authService;

    private User _currentUser;

    public ObservableCollection<Portfolio> Portfolios { get; }

    public DashboardViewModel(PortfolioService portfolioService, IAuthService authService)
    {
        _portfolioService = portfolioService;
        _authService = authService;

        //Method call for getting the current user
        _currentUser = _authService.GetCurrentUser();

        // Ensure User class has a ViewPortfolio method returning IEnumerable<Portfolio>
        Portfolios = new ObservableCollection<Portfolio>(_currentUser.ViewPortfolio());
    }

    public void CreateNewPortfolio(string name, bool isGroup)
    {
        PortfolioType type = isGroup ? PortfolioType.Group : PortfolioType.Individual;

        var created = _portfolioService.CreatePortfolio(_currentUser.AccountId, name, type);
        Portfolios.Add(created);
    }
}