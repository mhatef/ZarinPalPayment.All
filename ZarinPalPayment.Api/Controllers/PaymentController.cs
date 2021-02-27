using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using ZarinPalPayment.Core.DTO;
using ZarinPalPayment.Core.Services;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace ZarinPalPayment.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PaymentController : ControllerBase
    {
        private readonly IZarinPalServices _zarinPalPayment;

        public PaymentController(IZarinPalServices zarinPalPayment)
        {
            _zarinPalPayment = zarinPalPayment;
        }


        // POST api/<PaymentController>
        [HttpPost("[action]")]
        public IActionResult Request([FromBody] BankRequestDTO model)
        {
            TerminalResponseDTO response = _zarinPalPayment.Pay(model);
            if (response.Success)
            {
                return Ok(response);
            }

            return BadRequest(response);
        }


        [HttpPost("[action]")]
        public IActionResult Verify([FromBody] BankRequestDTO model)
        {
            TerminalResponseDTO response = _zarinPalPayment.Verify(model);
            if (response.Success)
            {
                return Ok(response);
            }

            return BadRequest(response);
        }




    }
}
