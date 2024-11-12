using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdminPanel.Shared.Common
{
    public partial class StripeResponse
    {
        public string? Id { get; set; }
        public string? Object { get; set; }
        public string? ApiVersion { get; set; }
        public long Created { get; set; } // datetime ticks ?
        public Data? Data { get; set; }
        public bool Livemode { get; set; }
        public int PendingWebhooks { get; set; }
        public Request? Request { get; set; }
        public string? Type { get; set; }
    }

    public partial class Data
    {
        public DataObject? Object { get; set; }
    }

    public partial class DataObject
    {
        public string? Id { get; set; }
        public string? ObjectObject { get; set; }
        public long Amount { get; set; }
        public long AmountCaptured { get; set; }
        public long AmountRefunded { get; set; }
        public string? Application { get; set; }
        public long ApplicationFee { get; set; }
        public long ApplicationFeeAmount { get; set; }
        public string? BalanceTransaction { get; set; }
        public BillingDetails? BillingDetails { get; set; }
        public string? CalculatedStatementDescriptor { get; set; }
        public bool Captured { get; set; }
        public long Created { get; set; }
        public string? Currency { get; set; }
        public object? Customer { get; set; }
        public string? Description { get; set; }
        public string? Destination { get; set; }
        public object Dispute { get; set; }
        public bool Disputed { get; set; }
        public object? FailureBalanceTransaction { get; set; }
        public string? FailureCode { get; set; }
        public string? FailureMessage { get; set; }
        public FraudDetails? FraudDetails { get; set; }
        public object? Invoice { get; set; }
        public bool Livemode { get; set; }
        public object Metadata { get; set; }
        public string OnBehalfOf { get; set; }
        public object Order { get; set; }
        public Outcome Outcome { get; set; }
        public bool Paid { get; set; }
        public string PaymentIntent { get; set; }
        public string PaymentMethod { get; set; }
        public PaymentMethodDetails PaymentMethodDetails { get; set; }
        public string? ReceiptEmail { get; set; }
        public string? ReceiptNumber { get; set; }
        public Uri? ReceiptUrl { get; set; }
        public bool? Refunded { get; set; }
        public object Review { get; set; }
        public object Shipping { get; set; }
        public object Source { get; set; }
        public object SourceTransfer { get; set; }
        public object StatementDescriptor { get; set; }
        public object StatementDescriptorSuffix { get; set; }
        public string Status { get; set; }
        public object TransferData { get; set; }
        public object TransferGroup { get; set; }
    }

    public partial class BillingDetails
    {
        public Address Address { get; set; }
        public string? Email { get; set; }
        public string? Name { get; set; }
        public string? Phone { get; set; }
    }

    public partial class Address
    {
        public string? City { get; set; }
        public string? Country { get; set; }
        public object? Line1 { get; set; }
        public object? Line2 { get; set; }
        public string? PostalCode { get; set; }
        public string? State { get; set; }
    }

    public partial class FraudDetails
    {
    }

    public partial class Outcome
    {
        public string? NetworkStatus { get; set; }
        public string? Reason { get; set; }
        public string? RiskLevel { get; set; }
        public int RiskScore { get; set; }
        public string? SellerMessage { get; set; }
        public string? Type { get; set; }
    }

    public partial class PaymentMethodDetails
    {
        public Card Card { get; set; }
        public string? Type { get; set; }
    }

    public partial class Card
    {
        public string? Brand { get; set; }
        public Checks? Checks { get; set; }
        public string? Country { get; set; }
        public int ExpMonth { get; set; }
        public int ExpYear { get; set; }
        public string? Fingerprint { get; set; }
        public string? Funding { get; set; }
        public string? Installments { get; set; }
        public int Last4 { get; set; }
        public object Mandate { get; set; }
        public string? Network { get; set; }
        public object ThreeDSecure { get; set; }
        public object Wallet { get; set; }
    }

    public partial class Checks
    {
        public string? AddressLine1Check { get; set; }
        public string? AddressPostalCodeCheck { get; set; }
        public string? CvcCheck { get; set; }
    }

    public partial class Request
    {
        public string? Id { get; set; }
        public Guid IdempotencyKey { get; set; }
    }
}
