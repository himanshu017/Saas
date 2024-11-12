using AdminPanel.Shared.BO.WebApp;

namespace AdminPanel.Web.Client.Services
{
    public interface ISearchService
    {
        public event Action<string> SearchTextChanged;
        void SetSearchText(string searchText);

        public event Action<bool> SetSubscription;
        void SetSubscriptionFlag(bool isSubscribed);

        public event Action<GetProductListBO> SetSearchModel;
        public event Action ClearSearchText;
        void RefreshSearch(GetProductListBO model);
        void ClearText();
    }

    public class SearchService : ISearchService
    {
        public event Action<string> SearchTextChanged;
        public event Action<bool> SetSubscription;
        public event Action<GetProductListBO> SetSearchModel;
        public event Action ClearSearchText;
        public string? SearchText { get; private set; }
        public bool IsSubscribed { get; set; } = false;

        public GetProductListBO SearchModel {get;set;}

        private static ISearchService instance;
        public static ISearchService GetInstance<T>() where T : ISearchService, new()
        {
            if (instance == null)
            {
                return instance = new T();
            }
            return instance;
        }

        // set the search filter and pass on to the subscribed component
        public void SetSearchText(string searchText)
        {
            SearchText = searchText;
            NotifySearchTextChanged();
        }

        // set the flag from the component which we want to subscribe to the search event change
        public void SetSubscriptionFlag(bool value)
        {
            IsSubscribed = value;
            NotifySubscriptionChanged();
        }

        public void RefreshSearch(GetProductListBO model)
        {
            SearchModel = model;
            NotifySearchModelChanged();
        }

        public void ClearText()
        {
            NotifyClearSearchText();
        }

        private void NotifySearchTextChanged() => SearchTextChanged?.Invoke(SearchText);
        private void NotifySubscriptionChanged() => SetSubscription?.Invoke(IsSubscribed);
        private void NotifySearchModelChanged() => SetSearchModel?.Invoke(SearchModel);
        private void NotifyClearSearchText() => ClearSearchText?.Invoke();
    }
}
