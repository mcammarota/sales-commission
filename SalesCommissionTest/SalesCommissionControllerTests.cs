using SalesCommission.Model;
using SalesCommission.Services;
using System;
using System.Collections.Generic;
using Xunit;

namespace SalesCommissionTest
{
    public class SalesCommissionControllerTests
    {
        [Theory]
        [InlineData(1, "2022-03-01", 100.0, 3, 9)] // --- Teste 1
        [InlineData(1, "2022-03-01", 100.0, 1, 1)] // --- Teste 2
        public void CalculateBonusTest(int seller, DateTime date, double value, int count, double commissionValue)
        {
            var service =  new SalesCommissionService();
            var expected = new ReturnCommission();
            var request = new Request();

            CommissionPartial commissionPartial = new CommissionPartial();
            List<CommissionPartial> commissionsPartial = new List<CommissionPartial>();
            double commissionWithoutBonus = 0;

            request.Requests = new List<Sale>();

            for (int i = 0; i < count; i++)
            {
                var sale = new Sale
                {
                    Seller = seller,
                    Date = date,
                    Value = value
                };
                request.Requests.Add(sale);
                commissionPartial = service.CommissionWithoutBonus(sale);
                commissionsPartial.Add(commissionPartial);
                commissionWithoutBonus += commissionPartial.CommissionValue;
            }

            expected.Commissions = new List<Commission>();
            var commission = new Commission
            {
                Seller = seller,
                Month = date.Month,
                CommissionValue = commissionValue
            };
            expected.Commissions.Add(commission);

            var actual = service.CalculateCommission(request);

            var bonusCommission = actual.Result.Commissions[0].CommissionValue - commissionWithoutBonus;

            if(bonusCommission > 0)
            {
                Assert.Equal(expected.Commissions[0].CommissionValue, bonusCommission);
            }
            else
            {
                Assert.Equal(expected.Commissions[0].CommissionValue, actual.Result.Commissions[0].CommissionValue);
            }
        }

        //Teste 3
        [Fact]
        public void CalculateCommissionWithoutBonusTest()
        {
            var expected = 30.0;
            var service = new SalesCommissionService();
            var sale = new Sale
            {
                Seller = 1,
                Date = Convert.ToDateTime("2022-03-01"),
                Value = 1000.0
            };

            CommissionPartial commissionPartial = new CommissionPartial();
            commissionPartial = service.CommissionWithoutBonus(sale);
            var commissionWithoutBonus = commissionPartial.CommissionValue;

            Assert.Equal(expected, commissionWithoutBonus);
        }
    }
}
