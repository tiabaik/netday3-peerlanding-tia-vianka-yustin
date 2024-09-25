using DAL.DTO.Req;
using DAL.DTO.Res;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Repositories.Service.Interfaces
{
    public interface ILoanSercvices
    {
        Task<string> CreateLoan(ReqLoanDto loan);
        Task<string> UpdateLoan(string id, ReqUpdateLoan updateLoan);
        Task<List<ResListLoadDtocs>> LoanList(string Status);

    }
}
