using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Reflection;
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

namespace Zarinpal.Core.Services
{
    public class RequestServices : IRequestServices
    {
        private readonly Zarinpal_Db_Context _context;
        public IConfiguration Configuration { get; }
        private readonly HttpClient _client;

        public RequestServices(Zarinpal_Db_Context context, IConfiguration configuration)
        {
            _context = context;
            Configuration = configuration;
            _client = new HttpClient();
        }

        public void Dispose()
        {
            _context.DisposeAsync();
        }

        public async Task<Tuple<HttpResponseMessage, int>> PaymentRequest(ClientRequestVm clientRequest)
        {
            try
            {

                int? id = await CreateRequest(clientRequest);
                if (id.HasValue)
                {
                    PaymentRequestVm request = new PaymentRequestVm()
                    {
                        amount = clientRequest.Amount,
                        callback_url = Configuration.GetGatewayInfo("CallBack") + id.Value,
                        description = clientRequest.Description,
                        email = "",
                        mobile = "",
                        merchant_id = Configuration.GetGatewayInfo("Merchant")

                    };

                    var jsonBody = JsonConvert.SerializeObject(request);
                    var content = new StringContent(jsonBody, Encoding.UTF8, "application/json");
                    var response = _client.PostAsync(Configuration.GetGatewayInfo("Request_Url"), content).Result;

                    return Tuple.Create(response,id.Value);
                }
                return null;
            }
            catch
            {
                return null;
            }
        }

        public async Task<HttpResponseMessage> Verify(VerifyRequestVm request)
        {
            try
            {
                var jsonBody = JsonConvert.SerializeObject(request);
                var content = new StringContent(jsonBody, Encoding.UTF8, "application/json");
                var response = _client.PostAsync(Configuration.GetGatewayInfo("Verify_Url"), content).Result;

                return response;
            }
            catch
            {
                return null;
            }
        }

        public async Task<bool> AddPaymentAuthority(PaymentResponseVm response, int requestId)
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

        public async Task<bool> VerifyRequest(VerifyResponse response, string requestAuthority)
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
    }
}
