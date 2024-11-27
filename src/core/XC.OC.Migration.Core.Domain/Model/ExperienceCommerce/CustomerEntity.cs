using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XC.OC.Migration.Core.Domain.Model.ExperienceCommerce
{
    public class CustomerEntity
    {
        [JsonIgnore]
        public dynamic? Entity { get; set; }

        public string AccountStatus
        {
            get
            {
                if (this.Entity == null)
                    return string.Empty;
                return this.Entity.AccountStatus;
            }
        }

        public string AccountNumber
        {
            get
            {
                if (this.Entity == null)
                    return string.Empty;
                return this.Entity.AccountNumber;
            }
        }

        public string DateCreated
        {
            get
            {
                if (this.Entity == null)
                    return string.Empty;
                return this.Entity.DateCreated;
            }
        }

        public string DateUpdated
        {
            get
            {
                if (this.Entity == null)
                    return string.Empty;
                return this.Entity.DateUpdated;
            }
        }

        public string Email
        {
            get
            {
                if (this.Entity == null)
                    return string.Empty;
                return this.Entity.Email;
            }
        }

        public string FirstName
        {
            get
            {
                if (this.Entity == null)
                    return string.Empty;
                return this.Entity.FirstName;
            }
        }

        public string LastName
        {
            get
            {
                if (this.Entity == null)
                    return string.Empty;
                return this.Entity.LastName;
            }
        }

        public bool IsPersisted
        {
            get
            {
                if (this.Entity == null)
                    return false;
                return (bool)this.Entity.IsPersisted;
            }
        }

        public string UniqueId
        {
            get
            {
                if (this.Entity == null)
                    return string.Empty;
                return this.Entity.UniqueId;
            }
        }

        public string UserName
        {
            get
            {
                if (this.Entity == null)
                    return string.Empty;
                return this.Entity.UserName;
            }
        }

        public string Version
        {
            get
            {
                if (this.Entity == null)
                    return string.Empty;
                return this.Entity.Version;
            }
        }

        public IEnumerable<dynamic> Addresses
        {
            get
            {
                if (this.Entity != null)
                {
                    if (this.Entity.Components != null && this.Entity.Components.values.Count > 0)
                    {
                        var addresses = new List<dynamic>();
                        foreach (dynamic component in this.Entity.Components.values)
                        {
                            if (component != null && component.type == "Sitecore.Commerce.Plugin.Customers.AddressComponent, Sitecore.Commerce.Plugin.Customers")
                            {
                                addresses.Add(component.Party);
                            }
                        }
                        return addresses;
                    }
                }
                return Array.Empty<dynamic>();
            }
        }
    }
}
