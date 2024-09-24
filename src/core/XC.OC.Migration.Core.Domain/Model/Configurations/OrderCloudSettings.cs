using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XC.OC.Migration.Core.Domain.Model.Configurations
{
    public class OrderCloudSettings
    {
        public const string OrderCloudSettingsName = "OrderCloudSetting";

        public string BaseUrl { get; set; }

        public string MiddlewareClientId { get; set; }

        public string MiddlewareClientSecret { get; set; }

        public string StorefrontClientId { get; set; }

        public string BuyerId { get; set; }
    }
}
