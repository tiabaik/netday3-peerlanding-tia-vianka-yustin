using DAL.DTO.Req;
using DAL.DTO.Res;
using DAL.Models;
using DAL.Repositories.Service.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace DAL.Repositories.Service
{
    public class LoanServices : ILoanSercvices
    {
        private readonly PeerlandingContext _peerlandingContext;

        public LoanServices(PeerlandingContext peerlandingContext)
        {
            _peerlandingContext = peerlandingContext;
        }
        public async Task<string> CreateLoan(ReqLoanDto loan)
        {
            var newLoan = new MstLoans
            {
                BorrowerId = loan.BorrowerId,
                Amount = loan.Amount,
                InterestRate = loan.InterestRate,
                Duration = loan.Duration,
            };

            await _peerlandingContext.AddAsync(newLoan);
            await _peerlandingContext.SaveChangesAsync();

            return newLoan.BorrowerId;
        }

        public async Task<string> UpdateLoan(string id, ReqUpdateLoan updateLoan)
        {
            var loan = await _peerlandingContext.MstLoans.FindAsync(id);
            //var loan = await _peerlandingContext.MstLoans.SingleOrDefaultAsync(l => l.Id == id);

            if (loan == null)
            {
                throw new Exception("Loan not found");
            }

            loan.Status = updateLoan.Status;

            await _peerlandingContext.SaveChangesAsync();

            return loan.BorrowerId;
        }

        public async Task<List<ResListLoadDtocs>> LoanList(string Status)
        {
            var listLoan = await _peerlandingContext.MstLoans
                .Where(x => x.Status.Contains(Status)) 
                .OrderByDescending(x => x.CreatedAt)
                  .Select(x => new ResListLoadDtocs
                  {
                      LoanId = x.Id,
                      BorrowerName = x.User.Name,
                      Amount = x.Amount,
                      InterestRate = x.InterestRate,
                      Duration = x.Duration,
                      Status = x.Status,
                      CreatedAt = x.CreatedAt,
                      UpdatedAt = x.UpdatedAt
                  }).ToListAsync();
            return listLoan;
        }
    }
}
