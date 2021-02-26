using System;
using System.Collections.Generic;
using System.Text;

namespace ZarinPalPayment.Core.DTO
{
    public class BankRequestDTO
    {
        public long amount { get; set; }
        public long PaymentID { get; set; }
        public string UserName { get; set; }
        public string UserPassword { get; set; }
        public string TerminalID { get; set; }
        public string callBackUrl { get; set; }
        public string additionalData { get; set; }
        public string TerminalReference { get; set; }


    }
}
