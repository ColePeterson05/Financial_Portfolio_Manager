using System.Collections.ObjectModel;

namespace Financial_Portfolio_Manager.ViewModels
{
    public class PortfolioDetailViewModel
    {
        public Portfolio SelectedPortfolio { get; }

        // ObservableCollection for UI binding
        public ObservableCollection<PortfolioItem> Items { get; }

        public PortfolioDetailViewModel(Portfolio portfolio)
        {
            SelectedPortfolio = portfolio;

            // Initialize ObservableCollection with existing items
            Items = new ObservableCollection<PortfolioItem>(portfolio.Items);
        }

        // Remove a PortfolioItem by ticker
        public void RemoveItem(string ticker)
        {
            // Find the item in the collection
            var itemToRemove = Items.FirstOrDefault(i => i.Ticker == ticker);
            if (itemToRemove != null)
            {
                // Remove from the model
                SelectedPortfolio.removeItem(itemToRemove);

                // Remove from the ObservableCollection
                Items.Remove(itemToRemove);
            }
        }
    }
}