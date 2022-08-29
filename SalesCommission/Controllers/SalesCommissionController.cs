using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SalesCommission.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Threading.Tasks;

namespace SalesCommission.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SalesCommissionController : ControllerBase
    {
        public static List<Sale> listSales = new List<Sale>();

        [HttpGet]
        public List<Sale> GetSales()
        {
            return listSales;
        }

        [HttpPost]
        public List<Commission> CalculateCommission(List<Sale> sales)
        {
            List<Commission> commissions = new List<Commission>();
            List<Commission> commissionsFinal = new List<Commission>();
            List<CommissionPartial> commissionsPartial = new List<CommissionPartial>();

            var metas = new List<(int, int)>
            {
                (1, 5),
                (2, 3),
                (3, 2),
                (4, 2),
                (5, 5),
                (6, 60),
                (8, 2),
                (9, 4),
                (10, 4),
                (11, 7),
                (12, 2)
            };

            foreach (var sale in sales)
            {
                listSales.Add(sale);
                CommissionPartial commissionPartial = new CommissionPartial();
                commissionPartial.Seller = sale.Seller;
                commissionPartial.Month = sale.Date.Month;
                if (sale.Value < 300)
                    commissionPartial.CommissionValue = sale.Value * 0.01;
                else if (sale.Value >= 300 && sale.Value <= 1000)
                    commissionPartial.CommissionValue = sale.Value * 0.03;
                else
                    commissionPartial.CommissionValue = sale.Value * 0.05;
                commissionsPartial.Add(commissionPartial);
                commissionPartial.CommissionTotal += sale.Value;
            }

            var result = commissionsPartial.OrderBy(s => s.Seller).ThenBy(m => m.Month).GroupBy(d => new { d.Seller, d.Month })
                  .Select(g => new { Seller = g.Key.Seller, Month = g.Key.Month, Count = g.Count(), 
                      CommissionValue = Math.Round(g.Sum(x => x.CommissionValue), 2), CommissionTotal = Math.Round(g.Sum(x => x.CommissionTotal), 2) }).ToList();

            foreach(var commission in result)
            {
                Commission commissionFinal = new Commission();
                for (int i = 0; i < metas.Count; i++)
                {
                    if (commission.Month == metas[i].Item1)
                    {
                        commissionFinal.Seller = commission.Seller;
                        commissionFinal.Month = commission.Month;
                        commissionFinal.CommissionValue = commission.CommissionValue;
                        if (commission.Count >= metas[i].Item2)
                        {
                            commissionFinal.CommissionValue = commissionFinal.CommissionValue + Math.Round(commission.CommissionTotal* 0.03, 2);
                        }
                        commissionsFinal.Add(commissionFinal);
                    }
                }
                if (commission.Month == 7)
                {
                    commissionFinal.Seller = commission.Seller;
                    commissionFinal.Month = commission.Month;
                    commissionFinal.CommissionValue = commission.CommissionValue;
                    commissionsFinal.Add(commissionFinal);
                }
            }
            return commissionsFinal;
        }
    }
}
