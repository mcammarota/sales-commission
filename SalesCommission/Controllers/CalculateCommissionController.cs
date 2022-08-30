using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SalesCommission.Model;
using SalesCommission.Services;
using SalesCommission.Services.IServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Threading.Tasks;

namespace SalesCommission.Controllers
{
    [Route("commission/")]
    [ApiController]
    public class CalculateCommissionController : ControllerBase
    {
        private readonly ISalesCommissionService _salesCommissionService;

        public CalculateCommissionController(ISalesCommissionService salesCommissionService)
        {
            _salesCommissionService = salesCommissionService;
        }

        [Route("calculate")]
        [HttpPost]
        public async Task<ActionResult<ReturnCommission>> CalculateCommission(Request requests)
        {
            var result = await _salesCommissionService.CalculateCommission(requests);

            return Ok(result);
        }
    }
}
