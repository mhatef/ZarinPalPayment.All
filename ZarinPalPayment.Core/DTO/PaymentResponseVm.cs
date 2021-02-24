using System;
using System.Collections.Generic;
using System.Text;

namespace Zarinpal.Core.DTO
{
    public class PaymentResponseVm
    {
        public data data { get; set; }
    }


    public class PaymentErrorResponse
    {
        public errors errors { get; set; }
    }

    public class data
    {
        public string authority { set; get; }
        public int code { set; get; }
        public string message { set; get; }
        public string fee_type { get; set; }
        public int fee { get; set; }
    }

    public class errors
    {
        
        public int code { get; set; }
        public string message { get; set; }

    }

}
