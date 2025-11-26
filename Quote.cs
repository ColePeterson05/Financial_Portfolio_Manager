namespace Financial_Portfolio_Manager;

public struct Quote
{
    public double Price { get;  }
    public DateTime LastUpdated { get;  }

    public Quote(double price)
    {
        Price = price;
        LastUpdated = DateTime.Now;
    }
}