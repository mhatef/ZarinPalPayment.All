using System;
using System.Collections.Generic;
using System.Text;

namespace ZarinPalPayment.Core.DTO
{
    public class TerminalResponseDTO
    {
        public string Url { get; set; }
        public bool Success { get; set; }
        public string Message { get; set; }
        public string Reference { get; set; }
        public int StatusID { get; set; }
        public string Status { get; set; }

    }
}
