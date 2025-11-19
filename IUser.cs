namespace Financial_Portfolio_Management;

public interface IUser
{ 
    List<Portfolio> ViewPortfolio { get; }
    List<User> ViewUser { get; } //Autofilled by Rider possibly needs to be added once I thought about it
}