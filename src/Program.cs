using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Layout;
using Avalonia.Media;
using Avalonia.Metadata;
using Avalonia.Styling;
using Financial_Portfolio_Manager;

public class Program
{
    public static void Main(string[] args)
    {
        IDataLoader dataLoader = new TxtDataLoader();
        IUserRepository userRepo = UserRepository.GetInstance();
        IPortfolioRepository portfolioRepo = PortfolioRepository.GetInstance();
        IAuthService authService = new AuthService(userRepo);

        Console.Write("Enter path to portfolio data: ");
        string portfolioPath = Console.ReadLine()?.Trim() ?? string.Empty;

        Console.Write("Enter path to stock data: ");
        string stockPath = Console.ReadLine()?.Trim() ?? string.Empty;

        Console.Write("Enter path to ETF data: ");
        string etfPath = Console.ReadLine()?.Trim() ?? string.Empty;

        Dictionary<string, Dictionary<int, string>> portfolioData = dataLoader.LoadPortfolio(
            portfolioPath
        );
        List<Stock> stockData = dataLoader.LoadStock(stockPath);
        List<Etf> etfData = dataLoader.LoadETF(etfPath);

        // master catalogs to map user stocks to stocks data
        Dictionary<string, Stock> stockCatalog = stockData.ToDictionary(s => s.Ticker, s => s);
        Dictionary<string, Etf> etfCatalog = etfData.ToDictionary(e => e.Ticker, e => e);

        // factories
        Dictionary<PortfolioType, PortfolioFactory> factoryRegistry = new Dictionary<
            PortfolioType,
            PortfolioFactory
        >
        {
            { PortfolioType.Individual, new IndividualPortfolioFactory() },
            { PortfolioType.Group, new GroupPortfolioFactory() },
        };

        PortfolioService service = new PortfolioService(portfolioRepo, userRepo, factoryRegistry);

        foreach (var userEntry in portfolioData)
        {
            string userName = userEntry.Key;
            Dictionary<int, string> userTickers = userEntry.Value;

            // user loading
            IUser? existing = userRepo.GetUsers().FirstOrDefault(m => m.Name == userName);
            IUser user = existing ?? new User(userName);
            if (existing == null)
            {
                userRepo.Add(user);
            }

            // portfolio creation
            IndividualPortfolio myPortfolio = new IndividualPortfolio(
                $"{userName}'s Portfolio",
                user
            );

            foreach (var item in userTickers)
            {
                string ticker = item.Value;

                // Look up the details in our Master Catalog
                if (stockCatalog.ContainsKey(ticker))
                {
                    Stock masterStock = stockCatalog[ticker];

                    // Create a COPY for the user

                    Stock userStock = new Stock(
                        masterStock.Ticker,
                        masterStock.CompanyName,
                        masterStock.Price,
                        masterStock.Sector
                    );

                    myPortfolio.addItem(userStock);
                }
                else if (etfCatalog.ContainsKey(ticker))
                {
                    Etf masterEtf = etfCatalog[ticker];

                    Etf userEtf = new Etf(masterEtf.Ticker, masterEtf.Price);

                    myPortfolio.addItem(userEtf);
                }
                else
                {
                    Console.WriteLine(
                        $"Warning: User {userName} owns {ticker}, but we have no data for that stock."
                    );
                }
            }

            portfolioRepo.Add(myPortfolio);
        }

        while (true)
        {
            Console.WriteLine("---------------------------------------------------");
            Console.WriteLine("- 1. Create a new user                            -");
            Console.WriteLine("- 2. View users                                   -");
            Console.WriteLine("- 3. Login                                        -");
            Console.WriteLine("- 4. Quit                                         -");
            Console.WriteLine("---------------------------------------------------");
            Console.WriteLine();
            Console.Write("Select an option: ");
            string choice = Console.ReadLine()?.Trim() ?? string.Empty;

            switch (choice)
            {
                case "1":
                    CreateUser(userRepo);
                    break;
                case "2":
                    GetUsers(userRepo);
                    break;
                case "3":
                    Console.Write("Enter user ID: ");
                    int loginId = int.Parse(Console.ReadLine());

                    bool loginSuccess = authService.Login(loginId);
                    ShowLoginMenu(
                        userRepo,
                        portfolioRepo,
                        authService,
                        service,
                        stockCatalog,
                        etfCatalog
                    );
                    break;
                case "4":
                    return;
                default:
                    Console.WriteLine("Invalid option. Please try again.");
                    break;
            }
        }
    }

    public static void CreateUser(IUserRepository userRepo)
    {
        Console.Write("Input users name: ");
        string userName = Console.ReadLine()?.Trim() ?? string.Empty;
        ;

        Console.WriteLine("Select Role");
        Console.WriteLine("1. Standard User");
        Console.WriteLine("2. Group Manager");
        Console.Write("Enter choice: ");
        string choiceRole = Console.ReadLine()?.Trim() ?? string.Empty;

        IUser newUser;

        if (choiceRole == "2")
        {
            newUser = new GroupManager(userName);
        }
        else
        {
            newUser = new User(userName);
        }

        userRepo.Add(newUser);
    }

    public static void CreatePortfolio(
        PortfolioService service,
        IAuthService authService,
        PortfolioType type
    )
    {
        IUser currentUser = authService.GetCurrentUser();

        int userId = currentUser.AccountId;
        Console.Write("Input portfolio name: ");
        string portfolioName = Console.ReadLine()?.Trim() ?? string.Empty;
        service.CreatePortfolio(userId, portfolioName, type);
    }

    public static void AddAsset(
        IPortfolioRepository portfolioRepo,
        IAuthService authService,
        Dictionary<string, Stock> stockCatalog,
        Dictionary<string, Etf> etfCatalog
    )
    {
        IUser currentUser = authService.GetCurrentUser();

        List<Portfolio> myPortfolios = portfolioRepo.GetByUser(currentUser);

        Console.Write("Enter Portfolio ID to add to: ");
        int targetPortId = int.Parse(Console.ReadLine());

        Portfolio targetPortfolio = myPortfolios.FirstOrDefault(p => p.PortfolioId == targetPortId);

        if (targetPortfolio == null)
        {
            Console.WriteLine("Error: You do not own a portfolio with that ID.");
            return;
        }

        Console.Write("Enter Ticker (e.g. AAPL or SPY): ");
        string tickerInput = Console.ReadLine()?.Trim() ?? string.Empty;

        // check stock catalog
        if (stockCatalog.ContainsKey(tickerInput))
        {
            Stock master = stockCatalog[tickerInput];
            Stock userStock = new Stock(
                master.Ticker,
                master.CompanyName,
                master.Price,
                master.Sector
            );

            targetPortfolio.addItem(userStock);
        }
        // check etf catalog
        else if (etfCatalog.ContainsKey(tickerInput))
        {
            Etf master = etfCatalog[tickerInput];
            Etf userEtf = new Etf(master.Ticker, master.Price);

            targetPortfolio.addItem(userEtf);
        }
        else
        {
            Console.WriteLine("Error: Ticker not found in Stock or ETF catalogs.");
        }
    }

    public static void RemoveAsset(IPortfolioRepository portfolioRepo, IAuthService authService)
    {
        IUser currentUser = authService.GetCurrentUser();
        List<Portfolio> myPortfolios = portfolioRepo.GetByUser(currentUser);

        Console.Write("Enter Portfolio ID to remove from: ");
        int targetPortId = int.Parse(Console.ReadLine());
        Portfolio targetPortfolio = myPortfolios.FirstOrDefault(p => p.PortfolioId == targetPortId);
        if (targetPortfolio == null)
        {
            Console.WriteLine("Error: You do not own a portfolio with that ID.");
            return;
        }

        Console.Write("Enter Ticker to remove: ");
        string tickerInput = Console.ReadLine()?.Trim() ?? string.Empty;
        PortfolioItem itemToRemove = targetPortfolio.Items.FirstOrDefault(i =>
            i.Ticker.Equals(tickerInput, StringComparison.OrdinalIgnoreCase)
        );
        if (itemToRemove != null)
        {
            targetPortfolio.removeItem(itemToRemove);
            Console.WriteLine($"Removed {tickerInput} from portfolio.");
        }
        else
        {
            Console.WriteLine("Error: Ticker not found in your portfolio.");
        }
    }

    public static void GetUsers(IUserRepository userRepo)
    {
        List<IUser> allUsers = userRepo.GetUsers();
        foreach (IUser user in allUsers)
        {
            Console.WriteLine($"{user.AccountId}: {user.Name}");
        }
    }

    public static void GetPortfolios(IPortfolioRepository portfolioRepo, IAuthService authService)
    {
        IUser currentUser = authService.GetCurrentUser();

        List<Portfolio> allPortfolios = portfolioRepo.GetByUser(currentUser);
        foreach (Portfolio portfolio in allPortfolios)
        {
            Console.WriteLine($"PORTFOLIO {portfolio.PortfolioId}: {portfolio.Name}");
            Console.WriteLine($"TYPE:        {portfolio.Type}");
            Console.WriteLine("---------------------------------------------------");
            Console.WriteLine("{0,-10} {1,-30} {2,10}", "TICKER", "COMPANY", "PRICE");
            Console.WriteLine("{0,-10} {1,-30} {2,10}", "------", "-------", "-----");

            foreach (PortfolioItem item in portfolio.Items)
            {
                string companyName = "---";

                if (item is Stock s)
                {
                    companyName = s.CompanyName;
                }
                else if (item is Etf)
                {
                    companyName = "Exchange Traded Fund";
                }

                Console.WriteLine(
                    "{0,-10} {1,-30} {2,10:C}",
                    item.Ticker,
                    companyName.Length > 27 ? companyName.Substring(0, 27) + "..." : companyName, // Truncate if too long
                    item.Price
                );
            }

            Console.WriteLine("---------------------------------------------------\n");
        }
    }

    public static void ViewGroupMembers(IPortfolioRepository portfolioRepo, GroupManager manager)
    {
        List<GroupPortfolio> myGroups = portfolioRepo
            .GetByUser(manager)
            .OfType<GroupPortfolio>()
            .ToList();

        Console.Write("Enter Portfolio ID to view members: ");
        int portId = int.Parse(Console.ReadLine());

        GroupPortfolio selectedGroup = myGroups.FirstOrDefault(g => g.PortfolioId == portId);

        if (selectedGroup == null)
        {
            Console.WriteLine("Error: You do not manage a group with that ID.");
            return;
        }

        Console.WriteLine($"\nMembers of '{selectedGroup.Name}':");
        Console.WriteLine("--------------------------------");

        if (selectedGroup.Members.Count == 0)
        {
            Console.WriteLine("(No members found)");
        }
        else
        {
            foreach (IUser member in selectedGroup.Members)
            {
                string role = (member.AccountId == manager.AccountId) ? "(Owner)" : "";
                Console.WriteLine($"- {member.Name} (ID: {member.AccountId}) {role}");
            }
        }
        Console.WriteLine("--------------------------------\n");
    }

    public static void AddUserToPortfolio(
        IUserRepository userRepo,
        IPortfolioRepository portfolioRepo,
        GroupManager manager
    )
    {
        List<GroupPortfolio> myGroups = portfolioRepo
            .GetByUser(manager)
            .OfType<GroupPortfolio>()
            .ToList();

        Console.Write("Enter Portfolio ID to add member: ");
        int portId = int.Parse(Console.ReadLine());

        GroupPortfolio selectedGroup = myGroups.FirstOrDefault(g => g.PortfolioId == portId);

        if (selectedGroup == null)
        {
            Console.WriteLine("Error: You do not manage a group with that ID.");
            return;
        }

        Console.Write($"Enter User ID to add to {selectedGroup.Name}: ");
        int userId = int.Parse(Console.ReadLine());

        IUser userToAdd = userRepo.GetUser(userId);

        selectedGroup.Members.Add(userToAdd);
    }

    public static void RemoveUserFromPortfolio(
        IUserRepository userRepo,
        IPortfolioRepository portfolioRepo,
        GroupManager manager
    )
    {
        List<GroupPortfolio> myGroups = portfolioRepo
            .GetByUser(manager)
            .OfType<GroupPortfolio>()
            .ToList();
        Console.Write("Enter Portfolio ID to remove member: ");
        int portId = int.Parse(Console.ReadLine());
        GroupPortfolio selectedGroup = myGroups.FirstOrDefault(g => g.PortfolioId == portId);
        if (selectedGroup == null)
        {
            Console.WriteLine("Error: You do not manage a group with that ID.");
            return;
        }
        Console.Write($"Enter User ID to remove from {selectedGroup.Name}: ");
        int userId = int.Parse(Console.ReadLine());
        IUser userToRemove = userRepo.GetUser(userId);
        selectedGroup.Members.Remove(userToRemove);
    }

    public static void PortfolioScenarios(
        IPortfolioRepository portfolioRepo,
        IAuthService authService
    )
    {
        double value;
        IValuationStrategy strategy;

        IUser currentUser = authService.GetCurrentUser();

        List<Portfolio> myPortfolios = portfolioRepo.GetByUser(currentUser);

        Console.Write("Enter portfolio ID to view scenarios: ");
        int portId = int.Parse(Console.ReadLine());

        Portfolio selectedPortfolio = myPortfolios.FirstOrDefault(p => p.PortfolioId == portId);

        if (selectedPortfolio == null)
        {
            Console.WriteLine("Error: You do not own a portfolio with that ID.");
            return;
        }

        List<IValuationStrategy> strategies = new List<IValuationStrategy>
        {
            new RealTimeStrategy(),
            new BearMarketStrategy(),
            new BullMarketStrategy(),
        };

        Console.WriteLine("Choose a scenario:");
        Console.WriteLine("1. Current valuation");
        Console.WriteLine("2. Bear market valuation");
        Console.WriteLine("3. Bull market valuation");
        Console.Write("Enter choice: ");
        string choice = Console.ReadLine()?.Trim() ?? string.Empty;

        switch (choice)
        {
            case "1":
                strategy = new RealTimeStrategy();
                break;

            case "2":
                strategy = new BearMarketStrategy();
                break;

            case "3":
                strategy = new BullMarketStrategy();
                break;

            default:
                Console.WriteLine("Invalid option. Please try again.");
                return;
        }

        value = selectedPortfolio.CalculateTotalValue(strategy);
        Console.WriteLine($"{strategy.StrategyName}: {value:C}");
    }

    public static void ShowLoginMenu(
        IUserRepository userRepo,
        IPortfolioRepository portfolioRepo,
        IAuthService authService,
        PortfolioService service,
        Dictionary<string, Stock> stockCatalog,
        Dictionary<string, Etf> etfCatalog
    )
    {
        bool running = true;

        IUser currentUser = authService.GetCurrentUser();

        while (running)
        {
            Console.WriteLine("---------------------------------------------------");
            Console.WriteLine("- 1. Create a new user                            -");
            Console.WriteLine("- 2. Create portfolio                             -");
            Console.WriteLine("- 3. Manage asset                                 -");
            Console.WriteLine("- 4. Manage users in portfolio                    -");
            Console.WriteLine("- 5. View users                                   -");
            Console.WriteLine("- 6. View portfolios                              -");
            Console.WriteLine("- 7. View market scenarios                        -");
            Console.WriteLine("- 8. Logout                                       -");
            Console.WriteLine("---------------------------------------------------");
            Console.WriteLine();

            Console.Write("Select an option: ");
            string choice = Console.ReadLine()?.Trim() ?? string.Empty;

            switch (choice)
            {
                case "1":
                    CreateUser(userRepo);
                    break;
                case "2":
                    Console.WriteLine("Select Portfolio Type: ");
                    Console.WriteLine("1. Individual");
                    if (currentUser is GroupManager)
                        Console.WriteLine("2. Group");
                    Console.Write("Enter choice: ");
                    string portType = Console.ReadLine()?.Trim() ?? string.Empty;
                    if (portType == "1")
                        CreatePortfolio(service, authService, PortfolioType.Individual);
                    else if (portType == "2" && (currentUser is GroupManager))
                        CreatePortfolio(service, authService, PortfolioType.Group);
                    else
                        Console.WriteLine("Invalid option. Please try again.");
                    break;
                case "3":
                    Console.WriteLine("Select Asset Management: ");
                    Console.WriteLine("1. Add Asset");
                    Console.WriteLine("2. Remove Asset");
                    Console.Write("Enter choice: ");
                    string assetChoice = Console.ReadLine()?.Trim() ?? string.Empty;
                    if (assetChoice == "1")
                        AddAsset(portfolioRepo, authService, stockCatalog, etfCatalog);
                    else if (assetChoice == "2")
                        RemoveAsset(portfolioRepo, authService);
                    else
                        Console.WriteLine("Invalid option. Please try again.");
                    break;
                case "4":
                    Console.WriteLine("Select User Management: ");
                    Console.WriteLine("1. Add User to Portfolio");
                    Console.WriteLine("2. Remove User from Portfolio");
                    Console.Write("Enter choice: ");
                    string userChoice = Console.ReadLine()?.Trim() ?? string.Empty;

                    if (!(currentUser is GroupManager))
                        Console.WriteLine("Error: You are not a group manager.");
                    else if ((currentUser is GroupManager) && userChoice == "1")
                        AddUserToPortfolio(userRepo, portfolioRepo, currentUser as GroupManager);
                    else if ((currentUser is GroupManager) && userChoice == "2")
                        RemoveUserFromPortfolio(
                            userRepo,
                            portfolioRepo,
                            currentUser as GroupManager
                        );
                    else
                        Console.WriteLine("Invalid option. Please try again.");
                    break;
                case "5":
                    Console.WriteLine("Select User View: ");
                    Console.WriteLine("1. All Users");
                    if (currentUser is GroupManager)
                        Console.WriteLine("2. Users in Portfolio");
                    Console.Write("Enter choice: ");
                    string userView = Console.ReadLine()?.Trim() ?? string.Empty;

                    if (userView == "1")
                        GetUsers(userRepo);
                    else if (userView == "2" && (currentUser is GroupManager))
                        ViewGroupMembers(portfolioRepo, currentUser as GroupManager);
                    break;
                case "6":
                    GetPortfolios(portfolioRepo, authService);
                    break;
                case "7":
                    PortfolioScenarios(portfolioRepo, authService);
                    break;
                case "8":
                    authService.Logout();
                    running = false;
                    break;
                default:
                    Console.WriteLine("Invalid option. Please try again.");
                    break;
            }
        }
    }
}
