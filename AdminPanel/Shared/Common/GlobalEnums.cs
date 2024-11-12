using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace AdminPanel.Shared
{
    public enum EmailTemplates
    {
        VerifyEmail, // send email verification email when a user registers
        AccountActivation, // Sent when email is verified
        Welcome, // Send after account activation
        ForgotPassword, // use to send reset password link
        PasswordChanged, // sent once the user changes the password
        NewUserRequest, // When a new user registers from the app, needed here ??
        NewUserApproved, // Once the admin approes a new user request and assigns a customer to the user.
        InvoiceCopy,
        RefundIssued,
        CompanyNotification, // If the master admin wants to notify the users about an event or changes.
        WelcomeRegister
    }

    public enum UserTypes : byte
    {
        SuperAdmin = 1, // Orderflow admin to manage the companies and packages and other application data
        CompanyAdmin = 2, // Main user for a company Admin. Created when email verification in done.
        AppUser = 3, // Customer users for mobile app and webview login
        Sales = 4, // Salesrep user in mobile apps, created by company admin
        CompanyManager = 5, // created by company admin as another login to access the admin panel. Rights limited to view only.
        Driver = 6 // User for POD app.
    }

    public enum CommonTypes : byte
    {
        AddressType = 1,
        CommentType = 2,
        DiscountType = 3,
        DiscountLimitationType = 4,
        StatusType = 5,
        Status = 6,
    }

    public enum GroupTypes : byte
    {
        Customer = 1,
        DeliveryRuns = 2
    }

    public enum GroupScopes : byte
    {
        Message = 1,
        Marketing = 2
    }

    public enum MarketingTypes : byte
    {
        Image = 1,
        Pdf = 2
    }

    public enum MarketingTarget : byte
    {
        Group = 1,
        Customer = 2
    }

    public enum CutoffTypes : byte
    {
        Default = 1,
        DeliveryRun = 2,
        Postcode = 3,
        Pickup = 4
    }
    public enum DiscountLimitationTypes : byte
    {
        Unlimited = 1,
        n_times_per_product = 2,
        n_times_per_customer = 3,
        n_times_only = 4
    }

    public enum DiscountTypes : byte
    {
        Order_Total = 1,
        Selected_products = 2,
        Selected_Customers = 3,
        All_Products = 4,
        All_Customers = 5
    }
    public enum ImportFor
    {
        None = 0,
        Product = 1,
        Customer = 2,
        SpecialPrice = 3
    }

    public enum ImportType
    {
        None,
        Excel,
        CSV,
    }

    public enum WeekDay : int
    {
        M = 1,
        T = 2,
        W = 3,
        R = 4,
        F = 5,
        S = 6,
        U = 0
    }

    public enum FavListTypes : byte
    {
        Default = 1,
        Customer_Created = 2,
        Copied_List = 3
    }

    public enum OrderStatus : byte
    {
        /// <summary>
        /// Pending
        /// </summary>
        Pending = 1,

        /// <summary>
        /// Processing
        /// </summary>
        Processing = 2,

        /// <summary>
        /// Complete
        /// </summary>
        Complete = 3,

        /// <summary>
        /// Cancelled
        /// </summary>
        Cancelled = 4
    }

    public enum ShippingStatus : byte
    {
        /// <summary>
        /// Shipping not required
        /// </summary>
        ShippingNotRequired = 5,

        /// <summary>
        /// Not yet shipped
        /// </summary>
        NotYetShipped = 6,

        /// <summary>
        /// Partially shipped
        /// </summary>
        PartiallyShipped = 7,

        /// <summary>
        /// Shipped
        /// </summary>
        Shipped = 8,

        /// <summary>
        /// Delivered
        /// </summary>
        Delivered = 9
    }

    public enum PaymentStatus : byte
    {
        /// <summary>
        /// Pending
        /// </summary>
        Pending = 10,

        /// <summary>
        /// Authorized
        /// </summary>
        Authorized = 11,

        /// <summary>
        /// Paid
        /// </summary>
        Paid = 12,

        /// <summary>
        /// Partially Refunded
        /// </summary>
        PartiallyRefunded = 13,

        /// <summary>
        /// Refunded
        /// </summary>
        Refunded = 14,

        /// <summary>
        /// Voided
        /// </summary>
        Voided = 15
    }

    public enum CommentTypes : byte
    {
        Order = 1,
        Product = 2,
        Delivery = 3
    }

    public enum PaymentMethod : byte
    {
        OnAccount = 1,
        CashOnDelivery = 2,
        Stripe = 3,
        Paypal = 4
    }

    public enum AttrDisplayControlType
    {
        [Display(Description = "Color squares")]
        ColorSquares = 1,

        [Display(Description = "Radio button list")]
        RadioButtonList = 2,

        [Display(Description = "Drop-down list")]
        DropDownList = 3,

        [Display(Description = "Selection Tiles")]
        SelectionTiles = 4
    }

    public enum ToggleAttrType
    {
        Attribute = 0,
        AttributeMapping = 1,
        AttributeValue = 2
    }
}
