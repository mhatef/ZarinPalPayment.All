using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Zarinpal.Core.DTO;
using ZarinPalPayment.Core.DTO;

namespace Zarinpal.Core.Services
{
    public interface IRequestServices:IDisposable
    {
        Task<Tuple<HttpResponseMessage,int>> PaymentRequest(ClientRequestVm request);

        Task<HttpResponseMessage> Verify(VerifyRequestVm request);
        Task<bool> AddPaymentAuthority(PaymentResponseVm response,int requestId);
        Task<bool> VerifyRequest(VerifyResponse response, string requestAuthority);
        //Task<bool> AddPaymentAutority(int requestId, PaymentRequestResponse response);
        //Task<int?> GetRequestAmount(int requestId);
        //Task<int?> VerifyRequest(int requestId, PaymentVerificationResponse response);
    }
}
