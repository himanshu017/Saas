using AdminPanel.Shared.BO.CompanyAdmin;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdminPanel.Shared.BO
{
    public class GetCustomerModel
    {
        public long CompanyId { get; set; }
        public int PageSize { get; set; }
        public int PageNumber { get; set; }
        public string? SearchString { get; set; }
        public bool OrderByDesc { get; set; }
        public string? SortColumn { get; set; }
        public bool Active { get; set; } = true;
        public bool OnHold { get; set; } = false;
    }

    public class CustomerBO : CommonAuditFields
    {
        public long CustomerId { get; set; }
        public long CompanyId { get; set; }
        public string? AlphaCode { get; set; }
        public string? RegistrationNo { get; set; }
        public string? CustomerName { get; set; }
        public string? ContactName { get; set; }
        public string? Email { get; set; }
        public string? Phone1 { get; set; }
        public string? Phone2 { get; set; }
        public string? Fax { get; set; }
        public string? SalesmanCode { get; set; }
        public bool IsDebtorOnHold { get; set; } = false;
        public double? CreditLimit { get; set; }
        public bool IsSuspended { get; set; } = false;
        public bool IsActive { get; set; } = true;
        public bool IsSalesRep { get; set; } = false;
        public bool IsSpecialCategory { get; set; }
        public bool IsParent { get; set; }
        public string? PriceCode { get; set; }
        public double? BalanceTotal { get; set; }
        public double? Day30Balance { get; set; }
        public double? Day60Balance { get; set; }
        public double? Day90PlusBalance { get; set; }
        public double? Day120PlusBalance { get; set; }
        public double? TotalAmountDue { get; set; }
        public double? TotalAmountOwed { get; set; }
        public double? Ytdsales { get; set; }
        public double? Mtdsales { get; set; }
        public double? PreviousMonthSales { get; set; }
        public string? StandardDeliveryDays { get; set; }
        public string? PriceLevel { get; set; }
        public bool HasSpecialPermittedDays { get; set; }
        public string? PermittedDays { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }

        public IEnumerable<CustomerAddressBO>? CustomerAddresses { get; set; }
        public IEnumerable<CategoriesBO>? CustomerCategories { get; set; }
        public IEnumerable<CustomerBO>? ChildCustomerRecords { get; set; }
        public IEnumerable<long>? SpecialCategories { get; set; }
        public IEnumerable<long>? ChildCustomerIds { get; set; }
        public IEnumerable<long>? CustomerRuns { get; set; }
    }
}
