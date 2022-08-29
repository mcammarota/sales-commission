using SalesCommission.Controllers;
using System;
using Xunit;

namespace SalesCommissionTest
{
    public class SalesCommissionControllerTests
    {
        [Fact]
        public void PostCommission_Return_Bonus()
        {
            var controller = new CalculateCommissionController();
        }
    }
}
