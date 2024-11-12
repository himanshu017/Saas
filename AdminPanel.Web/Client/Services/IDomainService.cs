namespace AdminPanel.Web.Client.Services
{
    public interface IDomainService
    {
        public event Action OnDataSet;
        public void SetStateData();
    }

    public class DomainService : IDomainService
    {
        public event Action OnDataSet;

        public void SetStateData()
        {
            NotifyDataChanged();
        }

        private void NotifyDataChanged() => OnDataSet?.Invoke();
    }
}
