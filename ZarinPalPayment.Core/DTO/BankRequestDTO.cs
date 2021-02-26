using System;
using System.Collections.Generic;
using System.Text;

namespace ZarinPalPayment.Core.DTO
{
    public class BankRequestDTO
    {
        public int UserID { get; set; }
        public string merchant_id { get; set; }
        public string callback_url { get; set; }
        public string description { get; set; }
        public long amount { get; set; }
        public string mobile { get; set; }
        public string email { get; set; }
        public string authority { get; set; }

    }
}
