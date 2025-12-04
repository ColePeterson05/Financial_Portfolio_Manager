using ReactiveUI;
using System.Reactive;
using Financial_Portfolio_Manager;

public class LoginViewModel : ReactiveObject
{
    private readonly IAuthService _authService;

    private string _enteredAccountId;
    public string EnteredAccountId
    {
        get => _enteredAccountId;
        set => this.RaiseAndSetIfChanged(ref _enteredAccountId, value);
    }

    private string _enteredName;
    public string EnteredName
    {
        get => _enteredName; 
        set => this.RaiseAndSetIfChanged(ref _enteredName, value);
    }

    public ReactiveCommand<Unit, User> LoginCommand { get; }

    public LoginViewModel(IAuthService authService)
    {
        _authService = authService;

        LoginCommand = ReactiveCommand.Create(() =>
        {
            // Pass both account ID and name as string
            return _authService.Login(EnteredAccountId, EnteredName);
        });
    }
}