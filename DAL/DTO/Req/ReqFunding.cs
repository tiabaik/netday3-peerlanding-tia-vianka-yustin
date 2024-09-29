using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.DTO.Req
{
    public class ReqFunding
    {
        public string LoanId { get; set; }
        public string LenderId { get; set; }
        public decimal Amount { get; set; }
    }
}
