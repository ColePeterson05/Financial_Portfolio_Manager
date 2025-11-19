namespace Financial_Portfolio_Management;

public class User : IUser
{
    public String Name { get; set; };
    public int AccountId  { get; set; }
    public Boolean IsLoggedIn  { get; set; }
    public List<Portfolio> Portfolios { get; set; }

    public User()
    {
        Name = name;
        Portfolios = new List<Portfolio>();
    }

    public List<Porfolio> ViewPortfolio()
    {
        return Portfolios;
    }
    
}
