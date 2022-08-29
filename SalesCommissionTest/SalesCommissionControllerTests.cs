using FakeItEasy;
using Moq;
using SalesCommission.Controllers;
using SalesCommission.Model;
using SalesCommission.Services;
using SalesCommission.Services.IServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace SalesCommissionTest
{
    public class SalesCommissionControllerTests
    {
        [Fact]
        public void Test()
        {
            var service = new SalesCommissionService();
            var expected = new ReturnCommission();
            var request = new Request();

            double CommissionWithoutBonus = 0;

            request.Requests = new List<Sale>();

            for(int i=0; i<3; i++)
            {
                var sale = new Sale
                {
                    Seller = 1,
                    Date = Convert.ToDateTime("2022-03-01"),
                    Value = 100.0
                };
                request.Requests.Add(sale);
            }

            expected.Commissions = new List<Commission>();
            var commission = new Commission
            {
                Seller = 1,
                Month = 3,
                CommissionValue = 6.0
            };
            expected.Commissions.Add(commission);

            var actual = service.CalculateCommission(request);

            var actualCommission = actual.Result.Commissions[0].CommissionValue - CommissionWithoutBonus;

            Assert.Equal(expected.Commissions[0].CommissionValue, actualCommission);
        }
    }
}
