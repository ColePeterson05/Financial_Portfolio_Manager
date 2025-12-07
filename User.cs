namespace Financial_Portfolio_Manager;

public class User : IUser
{
   public string Name { get; set; }
   public int AccountId { get; set; } = 0; // default to 0, which means "unassigned" for repo
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