using DAL.DTO.Req;
using DAL.DTO.Res;
using DAL.Models;
using DAL.Repositories.Service.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Repositories.Service
{
    public class RepaymentServices : IRepaymentServices
    {
        private readonly PeerlandingContext _peerLandingContext;

        public RepaymentServices(PeerlandingContext peerLandingContext)
        {
            _peerLandingContext = peerLandingContext;
        }

        public async Task<string> CreateRepayment(ReqCreateRepaymentDto reqCreate)
        {
            var newRepayment = new TrnRepayment
            {
                LoanId = reqCreate.LoanId,
                Amount = reqCreate.Amount,
                RepaidAmount = reqCreate.RepaidAmount,
                BalanceAmount = reqCreate.BalanceAmount,
            };

            await _peerLandingContext.AddAsync(newRepayment);
            await _peerLandingContext.SaveChangesAsync();

            return newRepayment.Id;
        }
        public async Task<List<ResRepaymentDto>> GetRepaymentByUserId(string userId)
        {
            var repayments = await _peerLandingContext.TrnFundings
                 .Join(
                     _peerLandingContext.MstLoans,
                     funding => funding.LoanId,
                     loan => loan.Id,
                     (funding, loan) => new { funding, loan }
                     )
                 .Join(
                     _peerLandingContext.TrnRepayments,
                     funding => funding.loan.Id,
                     repayment => repayment.LoanId,
                     (funding, repayment) => new { funding, repayment }
                     )
                 .Where(x => x.funding.funding.LenderId == userId)
                 .Select(x => new ResRepaymentDto
                 {
                     Id = x.repayment.Id,
                     LoanId = x.repayment.LoanId,
                     LenderId = x.funding.funding.LenderId,
                     BorrowerName = x.funding.loan.User.Name,
                     Amount = x.repayment.Amount,
                     RepaidAmount = x.repayment.RepaidAmount,
                     BalanceAmount = x.repayment.BalanceAmount,
                     RepaidStatus = x.repayment.RepaidStatus,
                     PaidAt = x.repayment.PaidAt,
                 })
                 .ToListAsync();

            if (repayments == null || repayments.Count == 0)
            {
                throw new Exception("Repayment not found");
            }

            return repayments;
        }

    }
}
