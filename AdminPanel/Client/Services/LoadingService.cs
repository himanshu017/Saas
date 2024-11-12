namespace AdminPanel.Client.Services
{
    public class LoadingService
    {
        public event Action OnShow;
        public event Action OnHide;
        public void Start()
        {
            OnShow?.Invoke();
        }

        public void Stop()
        {
            OnHide?.Invoke();
        }
    }
}
