namespace Financial_Portfolio_Management;

public class User : IUser
{
   private string Name { get; set; }
   private int AccountID { get; set; }
   private bool IsLoggedIn { get; set; }
   private List<Portfolio> portfolio = new();

   public User(string name)
   {
      Name = name;
   }

   public List<Portfolio> ViewPortfolio()
   {
      return portfolios;
   }
}