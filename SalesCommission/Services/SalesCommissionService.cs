using SalesCommission.Model;
using SalesCommission.Services.IServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SalesCommission.Services
{
    public class SalesCommissionService : ISalesCommissionService
    {
        Goals goalsService = new Goals();
        List<CommissionPartial> commissionsPartial = new List<CommissionPartial>();
        ReturnCommission returnCommission = new ReturnCommission();

        public async Task<ReturnCommission> CalculateCommission(Request requests)
        {
            List<Commission> commissionsFinal = new List<Commission>();
            var goals = goalsService.GetGoals();

            //Comissão sem bônus
            foreach (var sale in requests.Requests)
            {
                CommissionPartial commissionPartial = new CommissionPartial();
                commissionPartial = CommissionWithoutBonus(sale);
                commissionsPartial.Add(commissionPartial);
                commissionPartial.CommissionTotal += sale.Value;
            }

            //Lista de comissões sem bônus
            var result = commissionsPartial.OrderBy(s => s.Seller).ThenBy(m => m.Month).GroupBy(d => new { d.Seller, d.Month })
                  .Select(g => new ResultCommission
                  {
                      Seller = g.Key.Seller,
                      Month = g.Key.Month,
                      Count = g.Count(),
                      CommissionValue = Math.Round(g.Sum(x => x.CommissionValue), 2),
                      CommissionTotal = Math.Round(g.Sum(x => x.CommissionTotal), 2)
                  }).ToList();

            //Total de comissão com bônus por meta
            commissionsFinal = BonusPerGoal(result);

            try
            {
                returnCommission.Commissions = commissionsFinal;
            }
            catch (Exception ex)
            {
                return null;
            }

            return await Task.FromResult(returnCommission);
        }

        public CommissionPartial CommissionWithoutBonus(Sale sale)
        {
            CommissionPartial commissionPartial = new CommissionPartial();
            commissionPartial.Seller = sale.Seller;
            commissionPartial.Month = sale.Date.Month;
            if (sale.Value < 300)
                commissionPartial.CommissionValue = sale.Value * 0.01;
            else if (sale.Value >= 300 && sale.Value <= 1000)
                commissionPartial.CommissionValue = sale.Value * 0.03;
            else
                commissionPartial.CommissionValue = sale.Value * 0.05;
            return commissionPartial;
        }

        public List<Commission> BonusPerGoal(List<ResultCommission> result)
        {
            List<Commission> commissionsFinal = new List<Commission>();
            var goals = goalsService.GetGoals();
            foreach (var commission in result)
            {
                Commission commissionFinal = new Commission();
                for (int i = 0; i < goals.Count; i++)
                {
                    if (commission.Month == goals[i].Item1)
                    {
                        commissionFinal.Seller = commission.Seller;
                        commissionFinal.Month = commission.Month;
                        commissionFinal.CommissionValue = commission.CommissionValue;
                        if (commission.Count >= goals[i].Item2)
                        {
                            commissionFinal.CommissionValue = commissionFinal.CommissionValue + Math.Round(commission.CommissionTotal * 0.03, 2);
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
