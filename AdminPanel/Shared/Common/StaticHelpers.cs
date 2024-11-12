using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdminPanel.Shared
{
    public static class StaticHelpers
    {
        public static Dictionary<string, string> DayNames => new Dictionary<string, string>()
        {
            ["U"] = "Sunday",
            ["M"] = "Monday",
            ["T"] = "Tuesday",
            ["W"] = "Wednesday",
            ["R"] = "Thursday",
            ["F"] = "Friday",
            ["S"] = "Saturday"
        };

        public static Dictionary<string, string> SearchPageSortOptions => new Dictionary<string, string>()
        {
            ["Name (A-Z)"] = "Name asc",
            ["Name (Z-A)"] = "Name desc",
            ["Code (A-Z)"] = "Code asc",
            ["Code (Z-A)"] = "Code desc",
            ["Most Recent"] = "CreatedOn desc",
            ["Rewards Item"] = "IsRewardItem desc"
        };

        public static Dictionary<string, byte> SysAddressTypes => new Dictionary<string, byte>()
        {
            ["Billing Address"] = 1,
            ["Delivery Address"] = 2,
            ["Pickup Address"] = 3
        };

        public static Dictionary<string, string> FeatureNameSimplified => new Dictionary<string, string>()
        {
            ["IsNoticeBoard"] = "Show Notice Board",
            ["IsShowTermConditions"] = "Show Terms & Conditions",
            ["IsShowLiquorControl"] = "Liquor Control Warning",
            ["IsShowNonFoodVersion"] = "Non-Food Popup",
            ["IsShowPDF"] = "Show PDF popup",
            ["IsShowImage"] = "Show Image popup",


            ["IsShowPrice"] = "Show Price",
            ["IsHighlightRewardItems"] = "Highlight Reward Items",
            ["IsMinOQ"] = "Minimum Order Qty",
            ["IsMaxOQ"] = "Maximum Order Qty",
            ["IsAllowDecimal"] = "Allow Decimal Qty",
            ["IsAllowMultiples"] = "Allow Qty Multiples",
            ["IsDynamicUOM"] = "Dynamic UOM",
            ["IsShowQtyPerUnit"] = "Show Qty / Unit",
            ["IsGSTEnabled"] = "GST Enabled",
            ["IsAllowMultipleAddress"] = "Allow Multiple Addresses",
            ["IsAllowBackOrdering"] = "Allow Back Ordering",
            ["IsPictureView"] = "Picture View",
            ["IsShowStock"] = "Show Stock",
            ["IsHighlightStock"] = "Highlight Stock",
            ["IsShowCategory"] = "Show Category",
            ["IsShowSubCategory"] = "Show Sub Category",
            ["IsShowOnlyStockItems"] = "Show Only In Stock Items",
            ["IsAllowShare"] = "Allow Sharing",
            ["IsBrowsingEnableForOnHold"] = "Browsing Enabled For Hold Cust",


            ["IsShowParentChild"] = "Show Parent Child",
            ["IsShowSuggestiveSell"] = "Suggestive Sell",
            ["IsShowReporting"] = "Show Reporting",
            ["IsTargetMarketing"] = "Target Marketng",
            ["IsSendSMS"] = "Send SMS",
            ["IsSendNotification"] = "Send Push Notification",
            ["IsShareLinks"] = "Allow Share Links",
            ["IsRequestInvoice"] = "Request Invoice", // admin
            ["IsManagementUserCreation"] = "Management User Creation",
            ["IsSalesrepNotification"] = "Salesrep Notification",


            ["IsSearchProducts"] = "Allow Product Search",
            ["IsAllowBarcodeScanning"] = "Allow Barcode Scanning",
            ["IsShowInvoice"] = "Show Invoices",
            ["IsShowOrderHistory"] = "Show Order History",
            ["IsShowBrand"] = "Show Brand",
            ["IsShowSupplier"] = "Show Supplier",
            ["IsShowProductHistory"] = "Show Product History",
            ["IsPantryList"] = "Create Pantry Lists",
            ["IsAllowAddPantryList"] = "Add Favorite List",
            ["IsAllowCopyPantryList"] = "Copy Favorite List",
            ["IsAllowDeleteDefaultPantryList"] = "Delete Default List Items",
            ["IsAllowAddToDefaultPantryList"] = "Add to Default List",
            ["IsPantrySorting"] = "Allow Pantry Sorting",
            ["IsRememberLastUOM"] = "Remember Last Order UOM",
            ["IsAllowToAddAddress"] = "Allow to Add Address",
            ["IsShowFinancialDetails"] = "Show Financial Details",
            ["IsShowCompanyContact"] = "Show Company Contact Details",


            ["IsShowDateSelection"] = "Show Date Selection",
            ["IsShowNextDeliveryDate"] = "Show Next Delivery Dates",
            ["IsGenerateInvoicePDF"] = "Generate Invoice PDF",
            ["IsPromoCode"] = "Allow Promocodes",
            ["IsPONumber"] = "PO Number",
            ["IsRecurringOrder"] = "Recurring Order",
            ["IsSaveOrder"] = "Save Order",
            ["IsSameDayDelivery"] = "Same Day Delivery",
            ["IsStripeEnabled"] = "Enable Stripe",
            ["IsFreightCharges"] = "Freight Charges",
            ["IsSundayOrdering"] = "Sunday Ordering",
            ["IsSaturdayOrdering"] = "Saturday Ordering",
            ["IsNonDeliveryDayOrdering"] = "Non Delivery Day Ordering",
            ["IsOrderPlacementEnableForOnHold"] = "Place Order for On Hold",
            ["IsSplitOrderByCategory"] = "Split Orders By category",
            ["IsSplitOrderByFilter"] = "Split Orders By Filter",


            ["IsItemEnquiry"] = "Item Enquiry",
            ["IsItemFeedback"] = "Item Feedback",
            ["IsSpecialItemRequest"] = "Special Item Request",
            ["IsSendAdminOrderNotification"] = "Send Admin Order Notification",
            ["IsCheckAppVersion"] = "Check App version",


            ["IsSalesrepAllowed"] = "Enable Salesrep",
            ["IsSalesrepLocation"] = "Save Salesrep Location",
            ["IsAllowToAddSpecialPrice"] = "Allow to Add Special Price ",
            ["IsAddSpecialPriceFromPantry"] = "Add Special Price from Pantry",
            ["IsGenerateQuote"] = "Generate Quote",
            ["IsShowCost"] = "Show Cost",
            ["IsShowProfitMargin"] = "Show Profit Margins",


            ["IsBoxCount"] = "Show Box Count",
            ["IsDeliveryCompliance"] = "Show Delivery Compliance",
            ["IsTempratureReader"] = "Temprature Reading",
            ["IsVehicleChecklist"] = "Vehicle Checklist",
            ["IsVehicleTemprature"] = "Item Vehicle Temprature",
        };




    }
}
