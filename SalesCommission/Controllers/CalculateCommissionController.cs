using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SalesCommission.Model;
using SalesCommission.Services;
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
        [Route("calculate")]
        [HttpPost]
        public async Task<ActionResult<ReturnCommission>> CalculateCommission(Request requests)
        {
            SalesCommissionService salesCommissionService = new SalesCommissionService();
            var result = await salesCommissionService.CalculateCommission(requests);

            return Ok(result);
        }
    }
}
