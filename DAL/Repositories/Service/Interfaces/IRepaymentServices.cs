using DAL.DTO.Req;
using DAL.DTO.Res;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Repositories.Service.Interfaces
{
    public interface IRepaymentServices
    {
        Task<string> CreateRepayment(ReqCreateRepaymentDto reqCreate);
        Task<List<ResRepaymentDto>> GetRepaymentByUserId(string userId);
    }
}
