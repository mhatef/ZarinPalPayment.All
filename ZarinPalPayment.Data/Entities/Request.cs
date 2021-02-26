using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Zarinpal.Data.Entities
{
    public class Request
    {
        [Key]
        public int RequestID { get; set; }

        [Required]
        public int UserID { get; set; }

        // Enter In Rial Format
        [Required]
        public long PaymentAmount { get; set; }

        [Required]
        public string PaymentDescription { get; set; }

        public int RequestStatus { get; set; }

        public int ResponseCode { get; set; }

        public string ResponseAuthority { get; set; }
        public string ResponseMessage { get; set; }

        public int ReferenceID { get; set; }

        public DateTime RequestDatetime { get; set; }

    }
}
