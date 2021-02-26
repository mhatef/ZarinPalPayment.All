using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Zarinpal.Data.Entities
{
    public class Payment
    {
        [Key]
        public long PaymentID { get; set; }
        [Required]
        public long amount { get; set; }
        [Required]
        public string UserName { get; set; }
        //public string UserPassword { get; set; }
        public string TerminalID { get; set; }
        public string callBackUrl { get; set; }
        [Required]
        public string additionalData { get; set; }
        public string TerminalReference { get; set; }
        public string Message { get; set; }
        public string Reference { get; set; }
        public int StatusID { get; set; }
        public DateTime RequestDatetime { get; set; }

    }
}
