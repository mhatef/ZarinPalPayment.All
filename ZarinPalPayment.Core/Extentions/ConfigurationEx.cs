using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Extensions.Configuration;

namespace ZarinPalPayment.Core.Extentions
{
    public static class MyConfigurationExtensions
    {
        public static string GetGatewayInfo(this IConfiguration configuration, string name)
        {
            return configuration?.GetSection("GatewayInfo")?[name];
        }
    }
}
