using Radzen;

namespace AdminPanel.Web.Client.Services
{
    public interface INotify
    {
        void SuccessMsg(string message, string summary = "");
        void ErrorMsg(string message, string summary = "");
        void WarningMsg(string message, string summary = "");
        Task<bool> ConfirmDialog(string title, string message);
    }

    public class Notify : INotify
    {
        private readonly NotificationService _notify;
        private readonly DialogService _dialog;
        public Notify(NotificationService notify, DialogService dialog)
        {
            _notify = notify;
            _dialog = dialog;
        }

        public async Task<bool> ConfirmDialog(string title, string message)
        {
            var confirm = await _dialog.Confirm(message, title,
                    new ConfirmOptions()
                    {
                        OkButtonText = "Yes",
                        CancelButtonText = "No"
                    });

            return (confirm == false || confirm == null) ? false : true;
        }

        public void ErrorMsg(string message, string summary = "")
        {
            var notification = new NotificationMessage { Severity = NotificationSeverity.Error, Detail = message, Duration = 6000, Summary = summary };
            _notify.Notify(notification);
        }

        public void SuccessMsg(string message, string summary = "")
        {
            var notification = new NotificationMessage { Severity = NotificationSeverity.Success, Detail = message, Duration = 2000, Summary = summary };
            _notify.Notify(notification);
        }

        public void WarningMsg(string message, string summary = "")
        {
            var notification = new NotificationMessage { Severity = NotificationSeverity.Warning, Detail = message, Duration = 6000, Summary = summary };
            _notify.Notify(notification);
        }
    }
}
