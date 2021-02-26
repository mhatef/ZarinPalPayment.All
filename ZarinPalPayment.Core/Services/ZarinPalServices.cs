using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Zarinpal.Core.DTO;
using Zarinpal.Core.Enums;
using Zarinpal.Data.Context;
using Zarinpal.Data.Entities;
using ZarinPalPayment.Core.DTO;
using ZarinPalPayment.Core.Extentions;

namespace ZarinPalPayment.Core.Services
{
    public class ZarinPalServices:IZarinPalServices
    {

        private readonly Zarinpal_Db_Context _context;
        public IConfiguration Configuration { get; }
        private readonly HttpClient _client;

        public ZarinPalServices(Zarinpal_Db_Context context, IConfiguration configuration)
        {
            _context = context;
            Configuration = configuration;
            _client = new HttpClient();
        }


        public async Task<TerminalResponseDTO> Pay(BankRequestDTO model)
        {
            TerminalResponseDTO terminalResponse = new TerminalResponseDTO()
            {
                Success = false,
                Message = "unhandled exception occurred"
            };

            try
            {

                long? id = await CreateRequest(model);
                if (id.HasValue)
                {
                    PaymentRequestVm request = new PaymentRequestVm()
                    {
                        amount = model.amount,
                        callback_url = Configuration.GetGatewayInfo("CallBack") + model.UserName,
                        description = model.additionalData,
                        email = "",
                        mobile = "",
                        merchant_id = Configuration.GetGatewayInfo("Merchant")

                    };

                    var jsonBody = JsonConvert.SerializeObject(request);
                    var content = new StringContent(jsonBody, Encoding.UTF8, "application/json");
                    var response = _client.PostAsync(Configuration.GetGatewayInfo("Request_Url"), content).Result;
                    var responseContent = response.Content.ReadAsStringAsync().Result;


                    try
                    {
                        if (response.IsSuccessStatusCode)
                        {
                            PaymentResponseVm res = JsonConvert.DeserializeObject<PaymentResponseVm>
                                (responseContent);
                            if (res.data.code == 100)
                            {
                                if (await AddPaymentAuthority(res, id.Value))
                                {
                                    //TODO Fill terminalResponse
                                }
                            }
                        }
                    }
                    catch
                    {
                        PaymentErrorResponse err = JsonConvert.DeserializeObject<PaymentErrorResponse>
                            (responseContent);

                        //TODO Fill terminalResponse
                    }

                    return terminalResponse;
                }
                return terminalResponse;
            }
            catch
            {
                return terminalResponse;
            }
        }

        public async Task<TerminalResponseDTO> Verify(BankRequestDTO model)
        {
            TerminalResponseDTO terminalResponse = new TerminalResponseDTO()
            {
                Success = false,
                Message = "unhandled exception occurred"

            };

            try
            {
                VerifyRequestVm request = new VerifyRequestVm()
                {
                    amount = model.amount,
                    authority = model.TerminalReference,
                    merchant_id = Configuration.GetGatewayInfo("Merchant")
                };
                var jsonBody = JsonConvert.SerializeObject(request);
                var content = new StringContent(jsonBody, Encoding.UTF8, "application/json");
                var response = _client.PostAsync(Configuration.GetGatewayInfo("Verify_Url"), content).Result;
                var responseContent = response.Content.ReadAsStringAsync().Result;
                try
                {
                    if (response.IsSuccessStatusCode)
                    {
                        VerifyResponse res = JsonConvert.DeserializeObject<VerifyResponse>
                            (responseContent);
                        if (res.data.code == 100 || res.data.code == 101)
                        {
                            if (await VerifyRequest(res, request.authority))
                            {
                                //TODO Fill terminalResponse
                            }
                        }
                    }
                }
                catch
                {
                    VerifyErrorResponse err = JsonConvert.DeserializeObject<VerifyErrorResponse>
                        (responseContent);

                    //TODO Fill terminalResponse

                }
                return terminalResponse;
            }
            catch
            {
                return terminalResponse;
            }
        }

        public void Dispose()
        {
            _client.Dispose();
            _context.DisposeAsync();
        }



        private async Task<long?> CreateRequest(BankRequestDTO request)
        {
            try
            {
                Payment payment = new Payment()
                {
                    amount = request.amount,
                    additionalData = request.additionalData,
                    RequestDatetime = DateTime.Now,
                    StatusID = (int)RequestStatus.OnGoing,
                    UserName = request.UserName,
                    TerminalID = Configuration.GetGatewayInfo("Merchant"),
                    callBackUrl = Configuration.GetGatewayInfo("CallBack")
                };

                await _context.Payments.AddAsync(payment);

                await _context.SaveChangesAsync();

                return payment.PaymentID;
            }
            catch
            {
                return null;
            }
        }

        private async Task<bool> AddPaymentAuthority(PaymentResponseVm response, long paymentID)
        {
            try
            {
                var payment = await _context.Payments.FindAsync(paymentID);
                payment.StatusID = (int)RequestStatus.UnVerified;
                payment.TerminalReference = response.data.authority;

                _context.Payments.Update(payment);
                await _context.SaveChangesAsync();

                return true;
            }
            catch
            {
                return false;
            }
        }

        private async Task<bool> VerifyRequest(VerifyResponse response, string requestAuthority)
        {
            try
            {
                var payment = await _context.Payments.SingleAsync(p=>p.TerminalReference == requestAuthority);
                payment.StatusID = (int)RequestStatus.Verified;
                payment.Reference = response.data.ref_id.ToString();
                
                _context.Payments.Update(payment);
                await _context.SaveChangesAsync();

                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}
