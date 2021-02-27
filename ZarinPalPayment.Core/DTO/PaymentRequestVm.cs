using System;
using System.Collections.Generic;
using System.Text;

namespace Zarinpal.Core.DTO
{
    public class PaymentRequestVm
    {

        public string merchant_id { get; set; }
        public string callback_url { get; set; }
        public string description { get; set; }
        public long amount { get; set; }
        public string mobile { get; set; }
        public string email { get; set; }
        

    }
}
