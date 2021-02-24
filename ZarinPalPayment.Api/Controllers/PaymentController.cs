using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Zarinpal.Core.DTO;
using Zarinpal.Core.Services;
using ZarinPalPayment.Core.DTO;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace ZarinPalPayment.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PaymentController : ControllerBase
    {
        private readonly IRequestServices _requestServices;

        public PaymentController(IRequestServices requestServices)
        {
            _requestServices = requestServices;
        }


        // POST api/<PaymentController>
        [HttpPost("[action]")]
        public async Task<IActionResult> Request([FromBody] ClientRequestVm clientRequest)
        {
            var response = await _requestServices.PaymentRequest(clientRequest);
            var res2 = response.Item1.Content.ReadAsStringAsync().Result;

            try
            {
                if (response.Item1.IsSuccessStatusCode)
                {
                    PaymentResponseVm res = JsonConvert.DeserializeObject<PaymentResponseVm>
                        (res2);
                    if (res.data.code == 100)
                    {
                        if (await _requestServices.AddPaymentAuthority(res,response.Item2))
                        {
                            return Ok(res);
                        }
                    }
                }
            }
            catch
            {
                PaymentErrorResponse err = JsonConvert.DeserializeObject<PaymentErrorResponse>
                    (res2);
                return BadRequest("Response Code : " + err.errors.code
                                                     + Environment.NewLine +
                                                     "Response Message : " + err.errors.message);
            }

            return Problem("Unhandled Exception");
        }


        [HttpPost("[action]")]
        public async Task<IActionResult> Verify(VerifyRequestVm request)
        {
            HttpResponseMessage response = await _requestServices.Verify(request);
            var res2 = response.Content.ReadAsStringAsync().Result;
            try
            {
                if (response.IsSuccessStatusCode)
                {
                    VerifyResponse res = JsonConvert.DeserializeObject<VerifyResponse>
                        (res2);
                    if (res.data.code == 100 || res.data.code == 101)
                    {
                        if (await _requestServices.VerifyRequest(res, request.authority))
                        {
                            return Ok(res);
                        }
                    }
                }
            }
            catch
            {
                VerifyErrorResponse err = JsonConvert.DeserializeObject<VerifyErrorResponse>
                    (res2);
                return BadRequest("Response Code : " + err.error.code
                                                     + Environment.NewLine +
                                                     "Response Message : " + err.error.message);
            }

            return BadRequest(res2);
        }




    }
}
