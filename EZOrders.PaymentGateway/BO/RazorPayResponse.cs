using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EZOrders.PaymentGateway.BO
{
    public class RazorPayResponse
    {
        public bool AcceptPartial { get; set; }
        public long Amount { get; set; }
        public long AmountPaid { get; set; }
        public string CallbackMethod { get; set; }
        public Uri CallbackUrl { get; set; }
        public long CancelledAt { get; set; }
        public long CreatedAt { get; set; }
        public string Currency { get; set; }
        public Customer Customer { get; set; }
        public string Description { get; set; }
        public long ExpireBy { get; set; }
        public long ExpiredAt { get; set; }
        public long FirstMinPartialAmount { get; set; }
        public string Id { get; set; }
        public object Notes { get; set; }
        public Notify Notify { get; set; }
        public object Payments { get; set; }
        public string ReferenceId { get; set; }
        public bool ReminderEnable { get; set; }
        public List<object> Reminders { get; set; }
        public Uri Short_Url { get; set; }
        public string Status { get; set; }
        public long UpdatedAt { get; set; }
        public bool UpiLink { get; set; }
        public string UserId { get; set; }
        public bool WhatsappLink { get; set; }
    }

    public class Customer
    {
        public string Contact { get; set; }
        public string Email { get; set; }
        public string Name { get; set; }
    }

    public class Notify
    {
        public bool Email { get; set; }
        public bool Sms { get; set; }
        public bool Whatsapp { get; set; }
    }
}
