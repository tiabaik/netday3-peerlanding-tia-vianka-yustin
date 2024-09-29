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
        public async Task<string> CreateLoan(ReqLoanDto loan, string id)
        {
            var newLoan = new MstLoans
            {
                BorrowerId = id,
                Amount = loan.Amount,
                InterestRate = loan.InterestRate
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

        public async Task<List<ResListLoadBorrowerAcc>> LoanBorrowerList()
        {
            // Misalkan status yang diinginkan adalah "required"
            string requiredStatus = "requested";

            var listLoan = await _peerlandingContext.MstLoans
                .Where(x => x.Status == requiredStatus)  // Ganti dengan membandingkan status secara langsung
                .OrderByDescending(x => x.CreatedAt)
                .Select(x => new ResListLoadBorrowerAcc
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

        public async Task<List<ResListLoanBorrowerbyId>> LoanBorrowerListbyId( string borrowerId)
        {
            

            
            var listLoan = await _peerlandingContext.MstLoans
                .Where(x => x.BorrowerId == borrowerId) // Filter berdasarkan BorrowerId (UserId)
                .OrderByDescending(x => x.CreatedAt)
                .Select(x => new ResListLoanBorrowerbyId
                {
                    LoanId = x.Id,
                    BorrowerId = x.BorrowerId, // Mengisi BorrowerId
                    Amount = x.Amount,
                    InterestRate = x.InterestRate,
                    Duration = x.Duration,
                    Status = x.Status,
                    CreatedAt = x.CreatedAt,
                    UpdatedAt = x.UpdatedAt
                }).ToListAsync();

            return listLoan;
        }

        public async Task<ResListLoanDto1> GetLoanById(string loanId)
        {
            var loan = await _peerlandingContext.MstLoans
                .Include(l => l.User)
                .SingleOrDefaultAsync(l => l.Id == loanId);

            if (loan == null)
            {
                throw new Exception("Loan not found");
            }

            var result = new ResListLoanDto1
            {
                LoanId = loan.Id,
                User = new User
                {
                    Id = loan.User.Id,
                    Name = loan.User.Name
                },
                Amount = loan.Amount,
                InterestRate = loan.InterestRate,
                Duration = loan.Duration,
                Status = loan.Status,
                CreatedAt = loan.CreatedAt,
                UpdatedAt = loan.UpdatedAt,
            };

            return result;
        }





    }
}
