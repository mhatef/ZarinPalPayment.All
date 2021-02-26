using System;
using System.Collections.Generic;
using System.Text;

namespace ZarinPalPayment.Core.DTO
{
    public class VerifyResponse
    {
        public data data { get; set; }
    }

    public class VerifyErrorResponse
    {
        public errors errors { get; set; }
        
    }

    public class data
    {
        public int code { get; set; }
        public int ref_id { get; set; }
        public string message { get; set; }

    }

    public class errors
    {
        public int code { get; set; }
        public string message { get; set; }
    }
}
