using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using ZarinPalPayment.Core.DTO;

namespace ZarinPalPayment.Core.Services
{
    public interface IZarinPalServices:IDisposable
    {
        Task<TerminalResponseDTO> Pay(BankRequestDTO model);
        Task<TerminalResponseDTO> Verify(BankRequestDTO model);
    }
}
