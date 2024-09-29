using DAL.DTO.Req;
using DAL.DTO.Res;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Repositories.Service.Interfaces
{
    public interface IFundingServices
    {
        Task<string> FundingApproove(ReqFundingApprove reqFunding, string id);
    }
}
