using System;
using System.Collections.Generic;
using System.Text;

namespace ZarinPalPayment.Core.DTO
{
    public class VerifyRequestVm
    {
        public string merchant_id { get; set; }
        public long amount { get; set; }
        public string authority { set; get; }

    }
}
