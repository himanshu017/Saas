using AdminPanel.Shared.BO.WebApp;

namespace AdminPanel.Web.Client.Services
{
    public class QuickViewService
    {
        public event Action<ProductListBO> OnShow;
        public event Action OnHide;
        public void Show(ProductListBO prod)
        {
            OnShow?.Invoke(prod);
        }

        public void Hide()
        {
            OnHide?.Invoke();
        }
    }
}
