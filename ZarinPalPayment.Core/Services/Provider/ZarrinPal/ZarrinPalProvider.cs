using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using Newtonsoft.Json;
using Zarinpal.Core.DTO;
using ZarinPalPayment.Core.DTO;

namespace ZarinPalPayment.Core.Services.Provider.ZarrinPal
{
    public class ZarrinPalProvider
    {
        private PaymentGatewayClient _gatway;
        private readonly HttpClient _client;

        public ZarrinPalProvider()
        {
            _gatway = new PaymentGatewayClient();
            _client = new HttpClient();
        }



        public TerminalResponseDTO Pay(BankRequestDTO model)
        {
            TerminalResponseDTO terminalResponse = new TerminalResponseDTO()
            {
                Success = false,
                Message = "unhandled exception occurred"
            };

            try
            {

                long? id = CreateRequest(model);
                if (id.HasValue)
                {
                    PaymentRequestVm request = new PaymentRequestVm()
                    {
                        amount = model.amount,
                        callback_url = model.callBackUrl,
                        description = model.additionalData,
                        email = "",
                        mobile = "",
                        merchant_id = model.TerminalId

                    };

                    var jsonBody = JsonConvert.SerializeObject(request);
                    var content = new StringContent(jsonBody, Encoding.UTF8, "application/json");
                    var response = _client.PostAsync("https://api.zarinpal.com/pg/v4/payment/request.json", content).Result;
                    var responseContent = response.Content.ReadAsStringAsync().Result;


                    try
                    {
                        if (response.IsSuccessStatusCode)
                        {
                            PaymentResponseVm res = JsonConvert.DeserializeObject<PaymentResponseVm>
                                (responseContent);
                            if (res.data.code == 100)
                            {
                                if (AddPaymentAuthority(res, id.Value))
                                {
                                    terminalResponse.Message = res.data.message;
                                    terminalResponse.Url = "https://www.zarinpal.com/pg/StartPay/"+res.data.authority;
                                    terminalResponse.Reference = res.data.authority;
                                    terminalResponse.StatusID = res.data.code;
                                    terminalResponse.Success = true;

                                }
                            }
                        }
                    }
                    catch
                    {
                        PaymentErrorResponse err = JsonConvert.DeserializeObject<PaymentErrorResponse>
                            (responseContent);

                        terminalResponse.Success = false;
                        terminalResponse.Message = err.errors.message;
                        terminalResponse.Url = response.RequestMessage.RequestUri.AbsoluteUri;
                        terminalResponse.StatusID = err.errors.code;

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

        public TerminalResponseDTO Verify(BankRequestDTO model)
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
                    merchant_id = model.TerminalId
                };
                var jsonBody = JsonConvert.SerializeObject(request);
                var content = new StringContent(jsonBody, Encoding.UTF8, "application/json");
                var response = _client.PostAsync("https://api.zarinpal.com/pg/v4/payment/verify.json", content).Result;
                var responseContent = response.Content.ReadAsStringAsync().Result;
                try
                {

                    VerifyResponse res = JsonConvert.DeserializeObject<VerifyResponse>
                        (responseContent);
                    if (res.datas.code == 100 || res.datas.code == 101)
                    {
                        if (VerifyRequest(res, request.authority))
                        {
                            terminalResponse.Message = res.datas.message;
                            terminalResponse.Url = response.RequestMessage.RequestUri.AbsoluteUri;
                            terminalResponse.Reference = res.datas.ref_id.ToString();
                            terminalResponse.StatusID = res.datas.code;
                            terminalResponse.Success = true;

                        }
                    }

                }
                catch
                {
                    VerifyErrorResponse err = JsonConvert.DeserializeObject<VerifyErrorResponse>
                        (responseContent);

                    terminalResponse.Success = false;
                    terminalResponse.Message = err.errors.message;
                    terminalResponse.Url = response.RequestMessage.RequestUri.AbsoluteUri;
                    terminalResponse.StatusID = err.errors.code;
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
            //_context.DisposeAsync();
        }



        private long? CreateRequest(BankRequestDTO request)
        {
            try
            {
                //Payment payment = new Payment()
                //{
                //    amount = request.amount,
                //    additionalData = request.additionalData,
                //    RequestDatetime = DateTime.Now,
                //    StatusID = (int)RequestStatus.OnGoing,
                //    UserName = request.UserName,
                //    TerminalID = Configuration.GetGatewayInfo("Merchant"),
                //    callBackUrl = Configuration.GetGatewayInfo("CallBack")
                //};

                //_context.Payments.Add(payment);

                //_context.SaveChanges();

                //return payment.PaymentID;
                return 1;
            }
            catch
            {
                return null;
            }
        }

        private bool AddPaymentAuthority(PaymentResponseVm response, long paymentID)
        {
            try
            {
                //var payment = _context.Payments.Find(paymentID);
                //payment.StatusID = (int)RequestStatus.UnVerified;
                //payment.TerminalReference = response.data.authority;

                //_context.Payments.Update(payment);
                //_context.SaveChanges();

                return true;
            }
            catch
            {
                return false;
            }
        }

        private bool VerifyRequest(VerifyResponse response, string requestAuthority)
        {
            try
            {
                //var payment = _context.Payments.Single(p => p.TerminalReference == requestAuthority);
                //payment.StatusID = (int)RequestStatus.Verified;
                //payment.Reference = response.data.ref_id.ToString();

                //_context.Payments.Update(payment);
                //_context.SaveChanges();

                return true;
            }
            catch
            {
                return false;
            }
        }
    }

}
