namespace Financial_Portfolio_Manager;

public class User : IUser
{
   public string Name { get; set; }
   private int AccountID { get; set; }
   private bool IsLoggedIn { get; set; }
   private List<Portfolio> portfolios = new();
   private List<Portfolio> _viewPortfolio;

   public User(string name)
   {
      Name = name;
   }

   public List<Portfolio> ViewPortfolio()
   {
      return portfolios;
   }

   List<Portfolio> IUser.ViewPortfolio => _viewPortfolio;
}