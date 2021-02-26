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
                //TODO Fill terminalResponse
            };

            try
            {
                ClientRequestVm clientRequest = new ClientRequestVm()
                {
                    Amount = model.amount,
                    Description = model.description,
                    UserID = model.UserID
                };
                int? id = await CreateRequest(clientRequest);
                if (id.HasValue)
                {
                    BankRequestDTO request = new BankRequestDTO()
                    {
                        amount = clientRequest.Amount,
                        callback_url = Configuration.GetGatewayInfo("CallBack") + model.UserID,
                        description = clientRequest.Description,
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
                //TODO Fill terminalResponse
            };

            try
            {
                VerifyRequestVm request = new VerifyRequestVm()
                {
                    amount = model.amount,
                    authority = model.authority,
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



        private async Task<int?> CreateRequest(ClientRequestVm request)
        {
            try
            {
                Request _request = new Request()
                {
                    PaymentAmount = request.Amount,
                    PaymentDescription = request.Description,
                    RequestDatetime = DateTime.Now,
                    RequestStatus = (int)RequestStatus.OnGoing,
                    UserID = request.UserID,
                };

                await _context.Requests.AddAsync(_request);

                await _context.SaveChangesAsync();

                return _request.RequestID;
            }
            catch
            {
                return null;
            }
        }

        private async Task<bool> AddPaymentAuthority(PaymentResponseVm response, int requestId)
        {
            try
            {
                var request = await _context.Requests.FindAsync(requestId);
                request.RequestStatus = (int)RequestStatus.UnVerified;
                request.ResponseAuthority = response.data.authority;
                request.ResponseCode = response.data.code;

                _context.Requests.Update(request);
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
                var request = await _context.Requests.SingleAsync(r => r.ResponseAuthority == requestAuthority);
                request.RequestStatus = (int)RequestStatus.Verified;
                request.ReferenceID = (int)response.data.ref_id;
                request.ResponseCode = response.data.code;

                _context.Requests.Update(request);
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
