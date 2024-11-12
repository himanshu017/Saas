using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdminPanel.Shared.BO
{
    public class CustomerAddressBO : CommonAuditFields
    {
        public long CustomerId { get; set; }
        public long Id { get; set; }
        public byte AddressTypeId { get; set; }
        public string? AddressTypeName { get; set; }
        public int CountryId { get; set; }
        public string? CountryName { get; set; }
        public int StateId { get; set; }
        public string? StateName { get; set; }
        public string? ContactPerson { get; set; }
        public string? Email { get; set; }
        public string? County { get; set; }
        public int CityId { get; set; }
        public string? CityName { get; set; }
        public string? Address1 { get; set; }
        public string? Address2 { get; set; }
        public string? PostalCode { get; set; }
        public string? PhoneNumber { get; set; }
        public bool? IsActive { get; set; } = true;
        public string? FullAddress => $"{Address1} {Address2}, {CityName}, {StateName}, {PostalCode}, {CountryName}";
    }
    public class DeleteCustomerAddressBO
    {
        public long CustomerId { get; set; }
        public long AddressId { get; set; }
        public long? UserId { get; set; }
    }
}
