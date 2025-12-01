using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Layout;
using Avalonia.Media;
using Avalonia.Styling;
using Financial_Portfolio_Manager;

public class Program
{
    //// Entry point
    //[STAThread]
    //public static void Main(string[] args)
    //{
    //    BuildAvaloniaApp().StartWithClassicDesktopLifetime(args);
    //}

    //// Configure Avalonia
    //public static AppBuilder BuildAvaloniaApp()
    //    => AppBuilder.Configure<App>()
    //        .UsePlatformDetect()
    //        .LogToTrace();

    public static void Main(string[] args)
    {
        IDataLoader dataLoader = new TxtDataLoader();
        IUserRepository userRepo = UserRepository.GetInstance();
        IPortfolioRepository portfolioRepo = PortfolioRepository.GetInstance();

        Console.Write("Enter path to portfolio data: ");
        string portfolioPath = Console.ReadLine();

        Console.Write("Enter path to stock data: ");
        string stockPath = Console.ReadLine();

        Console.Write("Enter path to ETF data: ");
        string etfPath = Console.ReadLine();

        Dictionary<string, Dictionary<int, string>> portfolioData = dataLoader.LoadPortfolio(
            portfolioPath
        );
        List<Stock> stockData = dataLoader.LoadStock(stockPath);
        List<Etf> etfData = dataLoader.LoadETF(etfPath);

        // master catalogs to map user stocks to stocks data
        Dictionary<string, Stock> stockCatalog = stockData.ToDictionary(s => s.Ticker, s => s);
        Dictionary<string, Etf> etfCatalog = etfData.ToDictionary(e => e.Ticker, e => e);

        // factories
        var factoryRegistry = new Dictionary<PortfolioType, PortfolioFactory>
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
            var existing = userRepo.GetUsers().FirstOrDefault(m => m.Name == userName);
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
            Console.WriteLine("- 1. Add a new user                               -");
            Console.WriteLine("- 2. Add a new portfolio                          -");
            Console.WriteLine("- 3. Add asset                                    -");
            Console.WriteLine("- 4. View users                                   -");
            Console.WriteLine("- 5. View portfolios                              -");
            Console.WriteLine("- 6. Quit                                         -");
            Console.WriteLine("---------------------------------------------------");
            Console.WriteLine();
            Console.Write("Select an option: ");
            string choice = Console.ReadLine()?.Trim();

            switch (choice)
            {
                case "1":
                    Console.Write("Input users name: ");
                    string userName = Console.ReadLine();
                    IUser newUser = new User(userName);
                    userRepo.Add(newUser);
                    break;
                case "2":
                    Console.Write("Input users id: ");
                    int userId = int.Parse(Console.ReadLine());
                    Console.Write("Input portfolio name: ");
                    string portfolioName = Console.ReadLine();
                    service.CreatePortfolio(userId, portfolioName, PortfolioType.Individual);
                    break;
                case "3":
                    Console.Write("Enter Portfolio ID to add to: ");
                    int targetPortId = int.Parse(Console.ReadLine());

                    var targetPortfolio = portfolioRepo.GetById(targetPortId);

                    if (targetPortfolio == null)
                    {
                        Console.WriteLine("Error: Portfolio not found.");
                        break;
                    }

                    Console.Write("Enter Ticker (e.g. AAPL or SPY): ");
                    string tickerInput = Console.ReadLine()?.Trim();

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
                    break;
                case "4":
                    List<IUser> allUsers = userRepo.GetUsers();
                    foreach (var user in allUsers)
                    {
                        Console.WriteLine($"{user.AccountId}: {user.Name}");
                    }
                    break;
                case "5":
                    List<Portfolio> allPortfolios = portfolioRepo.GetAll();
                    foreach (var portfolio in allPortfolios)
                    {
                        Console.WriteLine($"PORTFOLIO {portfolio.PortfolioId}: {portfolio.Name}");

                        if (portfolio is IndividualPortfolio ind)
                        {
                            Console.WriteLine($"OWNER:     {ind.Owner.Name}");
                        }

                        Console.WriteLine("---------------------------------------------------");
                        Console.WriteLine("{0,-10} {1,-30} {2,10}", "TICKER", "COMPANY", "PRICE");
                        Console.WriteLine("{0,-10} {1,-30} {2,10}", "------", "-------", "-----");

                        foreach (var item in portfolio.Items)
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
                                companyName.Length > 27
                                    ? companyName.Substring(0, 27) + "..."
                                    : companyName, // Truncate if too long
                                item.Price
                            );
                        }

                        Console.WriteLine("---------------------------------------------------\n");
                    }
                    break;
                case "6":
                    return;
                default:
                    return;
            }
        }
    }
}

//// Minimal App class
//public class App : Application
//{
//    public override void OnFrameworkInitializationCompleted()
//    {
//        if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
//        {
//            desktop.MainWindow = new MainWindow();
//        }

//        base.OnFrameworkInitializationCompleted();
//    }
//}

//// Simple UI in one class
//public class MainWindow : Window
//{
//    private int _clickCount = 0;
//    private TextBlock _label;

//    public MainWindow()
//    {
//        Title = "Avalonia Test Window";
//        Width = 800;
//        Height = 400;

//        // Add global button styles programmatically (instead of XAML)
//        Styles.Add(new Style(x => x.OfType<Button>())
//        {
//            Setters =
//            {
//                new Setter(Button.BackgroundProperty, Brushes.DarkRed),
//                new Setter(Button.ForegroundProperty, Brushes.White),
//                new Setter(Button.FontWeightProperty, FontWeight.Bold),
//                new Setter(Button.CornerRadiusProperty, new CornerRadius(8)),
//                new Setter(Button.PaddingProperty, new Thickness(8,6)),
//            }
//        });

//        // Optional hover effect
//        Styles.Add(new Style(x => x.OfType<Button>().Class(":pointerover"))
//        {
//            Setters =
//            {
//                new Setter(Button.BackgroundProperty, Brushes.Red)
//            }
//        });

//        // Optional pressed effect
//        Styles.Add(new Style(x => x.OfType<Button>().Class(":pressed"))
//        {
//            Setters =
//            {
//                new Setter(Button.BackgroundProperty, Brushes.IndianRed)
//            }
//        });

//        var button = new Button { Content = "Click Me", HorizontalAlignment = HorizontalAlignment.Center };
//        var portButton = new Button { Content = "Portfolio", HorizontalAlignment = HorizontalAlignment.Center };

//        _label = new TextBlock { Text = "Hello Avalonia!", HorizontalAlignment = HorizontalAlignment.Center };

//        button.Click += (s, e) =>
//        {
//            _clickCount++;
//            _label.Text = $"Button clicked {_clickCount} time{(_clickCount == 1 ? "" : "s")}.";
//        };

//        portButton.Click += (s, e) =>
//        {
//            _clickCount++;
//            _label.Text = $"Portfolio clicked {(_clickCount == 1 ? "" : "s")}.";
//        };

//        //Where the buttons go
//        var stack = new StackPanel
//        {
//            VerticalAlignment = VerticalAlignment.Center,
//            Children = { _label, button, portButton } //button location
//        };

//        Content = stack;
//    }
//}
