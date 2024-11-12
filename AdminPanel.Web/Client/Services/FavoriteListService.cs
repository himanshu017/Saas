
namespace AdminPanel.Web.Client.Services
{
    public class FavoriteListService
    {
        public event Action<long> OnAddToList;
        public event Action OnClose;
        public event Action OnReset;
        public event Func<long, long, Task<bool>> OnDelete;

        public void AddToList(long productId)
        {
            OnAddToList?.Invoke(productId);
        }

        public void Close()
        {
            OnClose?.Invoke();
        }

        public void Reset()
        {
            OnReset?.Invoke();
        }

        public async Task<bool> DeleteItem(long productId, long listId)
        {
            return await OnDelete?.Invoke(productId, listId);
        }
    }
}
