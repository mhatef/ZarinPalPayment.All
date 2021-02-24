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
        public error error { get; set; }
        
    }

    public class data
    {
        public int code { get; set; }
        public int ref_id { get; set; }
        public string message { get; set; }

    }

    public class error
    {
        public int code { get; set; }
        public string message { get; set; }
    }
}
