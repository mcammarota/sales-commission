using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SalesCommission.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SalesCommission.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SalesCommissionController : ControllerBase
    {
        public static List<Sale> listSales = new List<Sale>();
        public static ResponseCommission commission = new ResponseCommission();

        [HttpGet]
        public List<Sale> GetSales()
        {
            return listSales;
        }

        [HttpPost]
        public List<Sale> PostRequest(List<Sale> sales)
        {
            List<Sale> lista = new List<Sale>();
            foreach (var sale in sales)
            {
                listSales.Add(sale);
            }

            lista = sales.OrderBy(s => s.Seller).ThenBy(m => m.Date).GroupBy(d => new { d.Seller, d.Date }).Select(x => x.First()).ToList();
            return lista;
        }
    }
}
