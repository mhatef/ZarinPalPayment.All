using System;
using System.Collections.Generic;
using System.Text;
using ZarinPalPayment.Core.DTO;

namespace ZarinPalPayment.Core.Services
{
    public interface IZarinPalServices:IDisposable
    {
        TerminalResponseDTO Pay(BankRequestDTO model);
        TerminalResponseDTO Verify(BankRequestDTO model);
    }
}
