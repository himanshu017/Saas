using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdminPanel.Shared.BO.MasterAdmin
{
    public class CompanyConfigBO
    {
        // Types of order generation settings
        public bool IsCSVGeneration { get; set; }
        public bool IsExcelGeneration { get; set; }
        public bool IsXMLGeneration { get; set; }
        public bool IsPDFGeneration { get; set; }

        // FTP details for uploading the order file
        public bool IsFTPUploading { get; set; }

        [RegularExpression("^(((ftp|http|https){1}:\\/\\/)[-a-zA-Z0-9@:%_\\+.~#?&//=]+)$", ErrorMessage = "Please enter a valid FTP url")]
        public string FTPUrl { get; set; } = string.Empty;
        public string FTPUserName { get; set; } = string.Empty;
        public string FTPPassword { get; set; } = string.Empty;
        public bool FTPUsePassive { get; set; }

        // Email setting for the company
        [RegularExpression("^[a-zA-Z0-9_\\.-]+@([a-zA-Z0-9-]+\\.)+[a-zA-Z]{2,6}$", ErrorMessage = "Please enter a valid email address")]
        public string FromEmailId { get; set; } = string.Empty;

        [RegularExpression("^[a-zA-Z0-9_\\.-]+@([a-zA-Z0-9-]+\\.)+[a-zA-Z]{2,6}$", ErrorMessage = "Please enter a valid email address")]
        public string AdminEmail { get; set; } = string.Empty;

        [RegularExpression("^[a-zA-Z0-9_\\.-]+@([a-zA-Z0-9-]+\\.)+[a-zA-Z]{2,6}$", ErrorMessage = "Please enter a valid email address")]
        public string InvoiceEmail { get; set; } = string.Empty;

        [RegularExpression("^[a-zA-Z0-9_\\.-]+@([a-zA-Z0-9-]+\\.)+[a-zA-Z]{2,6}$", ErrorMessage = "Please enter a valid email address")]
        public string SalesDepEmail { get; set; } = string.Empty;


        // Stripe payment settings
        public bool EnableStripePayment { get; set; } = false;
        public string StripePublicKey { get; set; } = string.Empty;
        public string StripeApiKey { get; set; } = string.Empty;
        public string StripePortalFeePercentageFor50AndBelow { get; set; } = string.Empty;
        public string StripeConnectedAccountID { get; set; } = string.Empty;
        public string StripePortalFeeFlatAbove50 { get; set; } = string.Empty;

        // General settings
        public bool SendCustomerAndRepMail { get; set; }
        public string GoogleAPIKey { get; set; } = string.Empty;
        public bool IsLive { get; set; } = true;
        public string OrderFilePrefix { get; set; } = string.Empty;
        public string InvoiceHeading { get; set; } = string.Empty;
        [RegularExpression("^[a-zA-Z0-9_\\.-]+@([a-zA-Z0-9-]+\\.)+[a-zA-Z]{2,6}$", ErrorMessage = "Please enter a valid email address")]
        public string OrderIssueEmail { get; set; } = string.Empty;
        public bool SplitOrders { get; set; } = false;
        public bool IsAttributeEnabled { get; set; } = false;

        // FCM settings
        public bool HasPushNotifications { get; set; } = false;
        [RegularExpression("^[0-9]*$", ErrorMessage = "Please enter a valid numeric only Sender Id")]
        public string FcmSenderIdMobile { get; set; } = string.Empty;
        public string FcmServerKeyMobile { get; set; } = string.Empty;

        [RegularExpression("^[0-9]*$", ErrorMessage = "Please enter a valid numeric only Sender Id")]
        public string FcmSenderIdWeb { get; set; } = string.Empty;
        public string FcmServerKeyWeb { get; set; } = string.Empty;

        // Social media links
        [RegularExpression("^(http:\\/\\/www\\.|https:\\/\\/www\\.|http:\\/\\/|https:\\/\\/)?[a-z0-9]+([\\-\\.]{1}[a-z0-9]+)*\\.[a-z]{2,5}(:[0-9]{1,5})?(\\/.*)?$", ErrorMessage = "Please enter a valid Url")]
        public string FacebookLink { get; set; } = string.Empty;

        [RegularExpression("^(http:\\/\\/www\\.|https:\\/\\/www\\.|http:\\/\\/|https:\\/\\/)?[a-z0-9]+([\\-\\.]{1}[a-z0-9]+)*\\.[a-z]{2,5}(:[0-9]{1,5})?(\\/.*)?$", ErrorMessage = "Please enter a valid Url")]
        public string TwitterLink { get; set; } = string.Empty;

        [RegularExpression("^(http:\\/\\/www\\.|https:\\/\\/www\\.|http:\\/\\/|https:\\/\\/)?[a-z0-9]+([\\-\\.]{1}[a-z0-9]+)*\\.[a-z]{2,5}(:[0-9]{1,5})?(\\/.*)?$", ErrorMessage = "Please enter a valid Url")]
        public string InstagramLink { get; set; } = string.Empty;

        // Twilio sms configuration
        public bool EnableTwilio { get; set; } = false;
        [RegularExpression("^[0-9]*$", ErrorMessage = "Please enter a valid numeric only Sid")]
        public string TwilioAccountSid { get; set; } = string.Empty;
        public string TwilioAuthToken { get; set; } = string.Empty;

        [RegularExpression("^[\\+]?[(]?[0-9]{3}[)]?[-\\s\\.]?[0-9]{3}[-\\s\\.]?[0-9]{4,6}$", ErrorMessage = "Please enter a valid Phone Number")]
        public string TwilioPhoneNumber { get; set; } = string.Empty;


        // Order Settings

        /// <summary>
        /// Minimun order value at which place order is required.
        /// Empty if no min value
        /// </summary>
        public decimal? MinCartValue { get; set; }

        /// <summary>
        /// Allow place order below minimum.
        /// If false check the MinCartValue.
        /// </summary>
        public bool AllowOrderBelowMin { get; set; } = false;

        /// <summary>
        /// Minimum cart value to get free delivery.
        /// Empty if delivery is always free.
        /// </summary>
        public decimal? MinCartValueForFreeDelivery { get; set; }

        /// <summary>
        /// If cart value is less than MinCartValueForFreeDelivery, then apply this fee.
        /// Id MinCartValueForFreeDelivery is not empty then there has to be a value in this too.
        /// </summary>
        public decimal? FreightCharges { get; set; }

        /// <summary>
        /// If IsNonDeliveryDay feature is enabled then apply this fee for selected dates which
        /// are highlighted as Non-Delivery Days.
        /// N/A if feature is disabled
        /// </summary>
        public decimal? NonDeliveryDayCharges { get; set; }

    }

    public class FeatureConfigDescBO
    {
        public string? Name { get; set; }
        public string? DisplayName { get; set; }
        public string? Description { get; set; }
        public string? Group { get; set; }
        public bool Value { get; set; } = false;
    }

    public class OrderSettingsBO
    {
        public decimal? MinCartValue { get; set; } = 0;
        public bool AllowOrderBelowMin { get; set; } = false;
        public decimal? MinCartValueForFreeDelivery { get; set; } = 0;
        public decimal? FreightCharges { get; set; } = 0;
        public decimal? NonDeliveryDayCharges { get; set; } = 0;
    }
}
