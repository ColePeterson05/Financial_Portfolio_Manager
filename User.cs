namespace Financial_Portfolio_Manager;

public class User : IUser
{
   public string Name { get; set; }
   public int AccountId { get; set; } = 0; // default to 0, which means "unassigned" for repo
   //private bool IsLoggedIn { get; set; } no need to check if they are logged in if they are logged in
   public List<Portfolio> Portfolios { get; } = new();
   private List<Portfolio> _viewPortfolio;

   public User(string name)
   {
      Name = name;
   }

   // "Viewing" could do additional logic later
   public List<Portfolio> ViewPortfolio()
   {
      return Portfolios;
   }
}