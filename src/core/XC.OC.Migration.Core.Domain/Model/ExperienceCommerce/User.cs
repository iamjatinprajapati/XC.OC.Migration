using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace XC.OC.Migration.Core.Domain.Model.ExperienceCommerce
{
    public class User
    {
        public string UserName { get; set; }

        public string Email { get; set; }

        public DateTime? CreatedDate { get; set; }

        public DateTime? LastLoginDate { get; set; }

        public Guid? UserId { get; set; }

        public bool? IsLockedOut { get; set; }

        public DateTime? LastLockoutDate { get; set; }

        [JsonIgnore]
        public string[]? PropertyNames { get; set; }

        [JsonIgnore]
        public string? PropertyValuesString { get; set; }

        [JsonIgnore]
        public byte[]? PropertyValuesBinary { get; set; }

        public string FullName { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Comment { get; set; }

        public string City { get; set; }

        public string Country { get; set; }

        public string StateProvince { get; set; }

        public string Address1 { get; set; }

        public string Address2 { get; set; }

        public string ZipCode { get; set; }

        public ExtendedProperties ExtendedProperties { get; set; }
    }

    public class ProfileProperty
    {
        public string PropertyName { get; set; }

        public string PropertyValue { get; set; }

        public dynamic ExtendedProperties { get; set; }
    }

    public class ExtendedProperties
    {
        public string UserDataFacet { get; set; }

        public string UserName { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Email { get; set; }

        public string Comment { get; set; }

        public bool? IsDisabled { get; set; }

        //public DateTime? CreationDate { get; set; }

        //public DateTime? LastActivityDate { get; set; }

        //public DateTime? LastLoginDate { get; set; }

        //public DateTime? LastDisabledDate { get; set; }

        public string ExternalId { get; set; }

        public string[]? Shops { get; set; }

        public string[]? Customers { get; set; }
    }
}
