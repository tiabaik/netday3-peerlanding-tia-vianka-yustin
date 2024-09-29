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
        Task<string> CreateLoan(ReqLoanDto loan, string id);
        Task<string> UpdateLoan(string id, ReqUpdateLoan updateLoan);
        Task<List<ResListLoadDtocs>> LoanList(string Status);
        Task<List<ResListLoadBorrowerAcc>> LoanBorrowerList();
        Task<List<ResListLoanBorrowerbyId>> LoanBorrowerListbyId(string borrowerId);
        Task<ResListLoanDto1> GetLoanById(string loanId);

    }
}
