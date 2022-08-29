using SalesCommission.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SalesCommission.Services.IServices
{
    public interface ISalesCommissionService
    {
        Task<ReturnCommission> CalculateCommission(Request requests);
    }
}
