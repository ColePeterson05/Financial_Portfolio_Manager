using Avalonia;
using Avalonia.Controls;
using Avalonia.Layout;
using Avalonia.Styling;
using Avalonia.Media;
using Avalonia.Controls.ApplicationLifetimes;

public class Program
{
    // Entry point
    [STAThread]
    public static void Main(string[] args)
    {
        BuildAvaloniaApp().StartWithClassicDesktopLifetime(args);
    }

    // Configure Avalonia
    public static AppBuilder BuildAvaloniaApp()
        => AppBuilder.Configure<App>()
            .UsePlatformDetect()
            .LogToTrace();
}

// Minimal App class
public class App : Application
{
    public override void OnFrameworkInitializationCompleted()
    {
        if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
        {
            desktop.MainWindow = new MainWindow();
        }

        base.OnFrameworkInitializationCompleted();
    }
}

// Simple UI in one class
public class MainWindow : Window
{
    private int _clickCount = 0;
    private TextBlock _label;

    public MainWindow()
    {
        Title = "Avalonia Test Window";
        Width = 800;
        Height = 400;

        // Add global button styles programmatically (instead of XAML)
        Styles.Add(new Style(x => x.OfType<Button>())
        {
            Setters =
            {
                new Setter(Button.BackgroundProperty, Brushes.DarkRed),
                new Setter(Button.ForegroundProperty, Brushes.White),
                new Setter(Button.FontWeightProperty, FontWeight.Bold),
                new Setter(Button.CornerRadiusProperty, new CornerRadius(8)),
                new Setter(Button.PaddingProperty, new Thickness(8,6)),
            }
        });

        // Optional hover effect
        Styles.Add(new Style(x => x.OfType<Button>().Class(":pointerover"))
        {
            Setters =
            {
                new Setter(Button.BackgroundProperty, Brushes.Red)
            }
        });

        // Optional pressed effect
        Styles.Add(new Style(x => x.OfType<Button>().Class(":pressed"))
        {
            Setters =
            {
                new Setter(Button.BackgroundProperty, Brushes.IndianRed)
            }
        });

        var button = new Button { Content = "Click Me", HorizontalAlignment = HorizontalAlignment.Center };
        var portButton = new Button { Content = "Portfolio", HorizontalAlignment = HorizontalAlignment.Center };

        _label = new TextBlock { Text = "Hello Avalonia!", HorizontalAlignment = HorizontalAlignment.Center };

        button.Click += (s, e) =>
        {
            _clickCount++;
            _label.Text = $"Button clicked {_clickCount} time{(_clickCount == 1 ? "" : "s")}.";
        };

        portButton.Click += (s, e) =>
        {
            _clickCount++;
            _label.Text = $"Portfolio clicked {(_clickCount == 1 ? "" : "s")}.";
        };

        //Where the buttons go
        var stack = new StackPanel
        {
            VerticalAlignment = VerticalAlignment.Center,
            Children = { _label, button, portButton } //button location
        };

        Content = stack;
    }
}