namespace Financial_Portfolio_Manager;

public interface IUser
{ 
    string Name { get; set; }
    int AccountId { get; set; }

    List<Portfolio> ViewPortfolio(); //no need to add get since it will be personalized based on whos portfolio it is 
    //List<User> ViewUser { get; } //Autofilled by Rider possibly needs to be added once I thought about it
}