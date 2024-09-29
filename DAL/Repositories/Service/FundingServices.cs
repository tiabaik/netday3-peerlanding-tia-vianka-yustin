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

namespace DAL.Repositories.Service
{
    public class FundingServices : IFundingServices
    {
        private readonly PeerlandingContext _context;
     

        public FundingServices(PeerlandingContext context)
        {
            _context = context;
 

        }
        public async Task<string> FundingApproove(ReqFundingApprove reqFunding, string id)
        {
            var loanData = await _context.MstLoans.FirstOrDefaultAsync(x => x.Id == reqFunding.LoanId);

            if (loanData == null)
            {
                throw new Exception("Loan not found.");
            }

            var lender = await _context.MstUsers.FirstOrDefaultAsync(x => x.Id == id);

            if (loanData.Amount > lender.Balance)
            {
                throw new Exception("Insufficient balance.");
            }
            loanData.Status = "Funded";

            var newFunding = new TrnFunding
            {
                LenderId = id,
                LoanId = reqFunding.LoanId,
                Amount = loanData.Amount
            };

            await _context.TrnFundings.AddAsync(newFunding);
            lender.Balance -= loanData.Amount;


            var monthlyInterestRate = (double)(loanData.InterestRate / 100 / 12);
            var monthlyPayment = (decimal)((monthlyInterestRate * (double)loanData.Amount)
                                           / (1 - Math.Pow((1 + monthlyInterestRate), (-loanData.Duration))));


            var borrower = await _context.MstUsers.FirstOrDefaultAsync(x => x.Id == loanData.BorrowerId);
            borrower.Balance += loanData.Amount;

            var repayment = new TrnRepayment
            {
                LoanId = loanData.Id,
                Amount = monthlyPayment * loanData.Duration,
                RepaidAmount = 0,
                BalanceAmount = monthlyPayment * loanData.Duration,
                RepaidStatus = "On Repay",
                PaidAt = DateTime.UtcNow
            };

            await _context.TrnRepayments.AddAsync(repayment);

            await _context.SaveChangesAsync();
            var repaymentData = await _context.TrnRepayments.FirstOrDefaultAsync(x => x.LoanId == loanData.Id);

            for (int i = 1; i <= loanData.Duration; i++)
            {
                var monthpayment = new TrnMonthlyPayment
                {
                    RepaymentId = repaymentData.Id,
                    Amount = monthlyPayment * 12,
                    Status = false,
                };

                _context.TrnMonthlyPayments.Add(monthpayment);
            }

            await _context.SaveChangesAsync();
            return newFunding.LenderId;
        }

    }

}

    



